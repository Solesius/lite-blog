import { Component, OnInit } from '@angular/core';
import { BlogService } from '../shared/service/blog.service';
import { Blog } from '../shared/models/blog.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blog-admin-component',
  templateUrl: './blog-admin.component.html',
})
export class BlogAdministrationComponent implements OnInit {
  blogs: Array<Blog> = [];
  constructor(private blogService: BlogService, private router: Router) {}

  ngOnInit(): void {
    this.blogService.getBlogs().subscribe({
      next: (blogs) => {
        this.blogs = blogs;
      },
    });
  }

  editBlog(blogId: number) {
    this.router.navigate(['admin/blog/edit', blogId]);
  }
}
