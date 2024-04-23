import { Component, OnInit } from '@angular/core';
import { BlogService } from '../shared/service/blog.service';
import { Blog } from '../shared/models/blog.model';
import { Router } from '@angular/router';
import { AdminService } from '../shared/service/admin.service';

@Component({
  selector: 'app-blog-admin-component',
  templateUrl: './blog-admin.component.html',
})
export class BlogAdministrationComponent implements OnInit {
  blogs: Array<Blog> = [];
  constructor(
    private blogService: BlogService,
    private adminSerivce: AdminService,
    private router: Router
  ) {}

  ngOnInit(): void {
    //confirm session before allowing access to admin
    const currentSession = sessionStorage.getItem('admin-session-id');
    if (!currentSession || currentSession == '' || currentSession == null) {
      this.logout();
    } else {
      //validate the session
      this.adminSerivce.validateAdminSession(currentSession).subscribe({
        next: (sessionValid) => {
          if (sessionValid === true) {
            this.loadBlogs();
          } else {
            this.logout();
          }
        },
        error: (error) => {
          this.logout();
        },
      });
    }
  }

  logout() {
    this.router.navigate(['login']);
  }

  editBlog(blogId: number) {
    this.router.navigate(['admin/blog/edit', blogId]);
  }

  private loadBlogs() {
    this.blogService.getBlogs().subscribe({
      next: (blogs) => {
        this.blogs = blogs;
      },
    });
  }
}
