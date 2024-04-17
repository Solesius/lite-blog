import { Component, OnInit } from '@angular/core';
import { BlogService } from '../shared/service/blog.service';
import { Blog } from '../shared/models/blog.model';

@Component({
  selector: 'app-blog-admin-component',
  templateUrl: './blog-admin.component.html'
})
export class BlogAdministrationComponent implements OnInit {

  blogs : Array<Blog> = []
  constructor(private blogService : BlogService) {}

  ngOnInit(): void {
    this.blogService.getBlogs().subscribe({
      next : (blogs) => {
        this.blogs = blogs;

        //TODO remove
        for(let i = 0; i < 50 ;i++) {
          this.blogs.push(blogs[0])
        }
      }
    })
  }
}
