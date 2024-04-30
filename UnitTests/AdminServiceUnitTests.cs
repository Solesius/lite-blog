using System.Security.Cryptography;
using System.Text;
using kw.liteblog.Database;
using kw.liteblog.Models;
using kw.liteblog.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace MyTests
{
    [TestClass]
    public class AdminServiceUnitTest
    {

        /// <summary>
        /// To prevent issues with the 'Run All Tests' VS Code helper, 
        /// put test dependencies in the test method bodies or run tests individually."
        /// </summary>
        private static readonly IConfiguration _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string?>>() { new("SESSION_KEY", "foo") }.OrderBy(x => x.Key))
                .Build();
        private static readonly Mock<IDataExtractor<BlogAdmin, string, DatabaseResponse>> _mockAdminExtr = new();
        private static readonly Mock<IDataExtractor<AdminSession, string, DatabaseResponse>> _mockSessionExtr = new();
        private static readonly AdminService _svc = new(_mockAdminExtr.Object, _mockSessionExtr.Object, _config);

        [TestMethod]
        public void CanUpdateDefaultAdminPassword()
        {
            var oldPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes("old_password"));
            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes("foo_password"));
            var newFakeHash = CreateKeyHash("foo_password", _config["SESSION_KEY"] ?? "");

            var admin = new BlogAdmin { Key = CreateKeyHash("old_password", _config["SESSION_KEY"] ?? "") };
            _mockAdminExtr.Setup(x => x.ExtractOne("admin")).Returns(admin);

            _svc.UpdateDefaultAdminPassword(oldPassword, password);

            _mockAdminExtr.Verify(x => x.UpdateOne(It.Is<BlogAdmin>(a => a.Key == newFakeHash)));
        }

        [TestMethod]
        public void UpdateWithBadPassFails()
        {
            var oldPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes("old_passwsord"));
            var password = Convert.ToBase64String(Encoding.UTF8.GetBytes("new_password"));
            var newFakeHash = CreateKeyHash(password, _config["SESSION_KEY"] ?? "");

            var admin = new BlogAdmin { Key = CreateKeyHash("old_password", _config["SESSION_KEY"] ?? "") };
            _mockAdminExtr.Setup(x => x.ExtractOne("admin")).Returns(admin);

            _svc.UpdateDefaultAdminPassword(oldPassword, password);

            _mockAdminExtr.Verify(x => x.ExtractOne("admin"), Times.Once);
            _mockAdminExtr.Verify(x => x.UpdateOne(It.IsAny<BlogAdmin>()), Times.Never);
        }

        [TestMethod]
        public void CanGetAdminSession()
        {
            var admin = new BlogAdmin { Key = CreateKeyHash("foo_password", _config["SESSION_KEY"] ?? "") };
            _mockAdminExtr.Setup(x => x.ExtractOne("admin")).Returns(admin);

            var fakePass = Convert.ToBase64String(Encoding.UTF8.GetBytes("foo_password"));
            var session = _svc.EstablishAdminSession(fakePass);

            Assert.IsNotNull(session);

            _mockAdminExtr.Verify(x => x.ExtractOne("admin"), Times.Once());
            _mockSessionExtr.Verify(x => x.InsertOne(It.Is<AdminSession>(a =>
                !string.IsNullOrEmpty(a.SessionId) &&
                a.SessionId == session.SessionId
            )));
        }

        [TestMethod]
        public void EmptyPassReturnsNull()
        {
            var session = _svc.EstablishAdminSession(string.Empty);

            Assert.IsNotNull(session);
            Assert.IsNull(session.SessionId);

            _mockAdminExtr.Verify(x => x.ExtractOne("admin"), Times.Never);
            _mockSessionExtr.Verify(x => x.InsertOne(It.IsAny<AdminSession>()), Times.Never);
        }

        [TestMethod]
        public void BadPassReturnsNull()
        {
            var admin = new BlogAdmin { Key = CreateKeyHash("foo_password", _config["SESSION_KEY"] ?? "") };
            _mockAdminExtr.Setup(x => x.ExtractOne("admin")).Returns(admin);

            var fakePass = Convert.ToBase64String(Encoding.UTF8.GetBytes("foo_passwsord"));

            var session = _svc.EstablishAdminSession(fakePass);
            Assert.IsNotNull(session);
            Assert.IsNull(session.SessionId);

            _mockAdminExtr.Verify(x => x.ExtractOne("admin"), Times.Once());
            _mockSessionExtr.Verify(x => x.InsertOne(It.IsAny<AdminSession>()), Times.Never);
        }

        [TestMethod]
        public void ValidSessionPasses()
        {
            var adminSession = new AdminSession { SessionId = "foobar123456", SessionTime = DateTime.Now.AddMinutes(60) };
            _mockSessionExtr.Setup(x => x.ExtractOne(adminSession.SessionId)).Returns(adminSession);

            Assert.IsTrue(_svc.SessionValid(adminSession.SessionId));

            _mockSessionExtr.Verify(x => x.ExtractOne(adminSession.SessionId), Times.Once());
        }

        [TestMethod]
        public void NotFoundSessionFails()
        {
            _mockSessionExtr.Setup(x => x.ExtractOne(It.IsAny<string>())).Returns(null as AdminSession);

            Assert.IsFalse(_svc.SessionValid(It.IsAny<string>()));
            _mockSessionExtr.Verify(x => x.ExtractOne(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void ExpiredSessionFails()
        {
            var adminSession = new AdminSession { SessionId = "foobar123456", SessionTime = DateTime.Now.AddMinutes(-60) };
            _mockSessionExtr.Setup(x => x.ExtractOne(adminSession.SessionId)).Returns(adminSession);

            Assert.IsFalse(_svc.SessionValid(adminSession.SessionId));

            _mockSessionExtr.Verify(x => x.ExtractOne(adminSession.SessionId), Times.Once());
        }

        private static string CreateKeyHash(string value, string key)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(value));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}
