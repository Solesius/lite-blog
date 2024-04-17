import { Component, OnInit } from '@angular/core';
import { Blog } from '../shared/models/blog.model';
import { BlogService } from '../shared/service/blog.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent  implements OnInit{

  blogs : Array<Blog> = []
  constructor(private blogService: BlogService) {}

  ngOnInit(): void {
    this.blogService.getBlogs().subscribe({
      next : (blogs) => {
        console.log(blogs);
        this.blogs = blogs
        //TODO remove
        for(let i = 0; i < 50 ;i++) {
          this.blogs.push(blogs[0])
        }
      }
    })
  }
}
