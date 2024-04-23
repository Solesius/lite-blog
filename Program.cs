var builder = WebApplication.CreateBuilder(args);

// Add services to the container and load in our Json
builder.Services.AddControllersWithViews();
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
// app.UseCors(builder =>
// {
//     builder.WithOrigins("*")
//         .WithMethods("GET", "POST", "OPTIONS")
//         .WithHeaders("Content-Type");
// });


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
