import { Component, Input } from '@angular/core';
import { Blog } from '../shared/models/blog.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
})
export class BlogListComponent {
    @Input()
    blogs : Blog[]  = []
    
    constructor(private router: Router) {}
    
    showBlog(blogId : number) {
      this.router.navigate(['blog/' + blogId])
    }
}