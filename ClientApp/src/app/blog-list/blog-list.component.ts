import { Component, Input } from '@angular/core';
import { Blog } from '../shared/models/blog.model';

@Component({
  selector: 'app-blog-list',
  templateUrl: './blog-list.component.html',
})
export class BlogListComponent {
    @Input()
    blogs : Blog[]  = []
    
    constructor() {}
}