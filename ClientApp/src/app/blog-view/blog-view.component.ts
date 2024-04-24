import { AfterViewInit, Component, Input, OnInit } from '@angular/core';
import { Blog } from '../shared/models/blog.model';
import { BlogService } from '../shared/service/blog.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-blog-view',
  templateUrl: './blog-view.component.html',
})
export class BlogViewComponent implements AfterViewInit {
    @Input()
    blog : Blog  | any = {}
    location = ''
    
    routeListener : Subscription = new Subscription();

    constructor(
      private route : ActivatedRoute,
      private router : Router,
      private blogService: BlogService,
      private sanitizer: DomSanitizer) {}

    ngAfterViewInit(): void {
      this.routeListener = this.route.paramMap.subscribe({
        next : (routeParams) => {
          const blogId = routeParams.get("blogId")
          if(blogId) {
            this.blogService.getBlog(+blogId).subscribe({
              next : (blog) => {
                blog.body = this.sanitizer.bypassSecurityTrustHtml(blog.body) as any
                this.blog = blog
                this.location = window.location.toString();
                console.log(this.location)
              }
            })
          }
        }
      })
    }

    toBlogs() {
      this.router.navigate([''])
    }
}