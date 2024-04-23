import { Component, Input, OnInit } from '@angular/core';
import { Blog } from '../shared/models/blog.model';
import { BlogService } from '../shared/service/blog.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-blog-view',
  templateUrl: './blog-view.component.html',
})
export class BlogViewComponent implements OnInit {
    @Input()
    blog! : Blog
    
    constructor(
      private route : ActivatedRoute,
      private router : Router,
      private blogService: BlogService,
      private sanitizer: DomSanitizer) {}

    ngOnInit(): void {
      this.blogService.getBlog(1).subscribe(x => {

        //use dom sanitizer to allow rich article content, we scrub in api layer. 
        x.body = this.sanitizer.bypassSecurityTrustHtml(x.body) as any
        this.blog = x
      } );
      
    }
}