import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Blog } from '../shared/models/blog.model';
import { BlogService } from '../shared/service/blog.service';
import { DomSanitizer } from '@angular/platform-browser';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  encapsulation: ViewEncapsulation.None,
})
export class HomeComponent implements OnInit {
  blogs: Array<Blog> = [];
  constructor(private blogService: BlogService) {}

  ngOnInit(): void {
    this.blogService.getBlogs().subscribe({
      next: (blogs) => {
        this.blogs = blogs;
      },
    });
  }
}
