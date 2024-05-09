import { Component, Input, OnInit } from '@angular/core';
import { Blog } from '../shared/models/blog.model';
import { BlogService } from '../shared/service/blog.service';
import { DomSanitizer } from '@angular/platform-browser';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-blog-view',
  templateUrl: './blog-view.component.html',
})
export class BlogViewComponent implements OnInit {
  @Input()
  blog: Blog | any = {};
  location = '';

  routeListener: Subscription = new Subscription();

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private blogService: BlogService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.routeListener = this.route.paramMap.subscribe({
      next: (routeParams) => {
        const blogId = routeParams.get('blogId');
        if (blogId) {
          if (!Number(blogId)) {
            this.router.navigate(['not-found']);
          }
          this.blogService.getBlog(+blogId).subscribe({
            next: (blog) => {
              if (!blog || blog == null) {
                this.router.navigate(['not-found']);
              } else {
                blog.body = this.sanitizer.bypassSecurityTrustHtml(
                  blog.body
                ) as any;
                this.blog = blog;
                this.location = window.location.toString();
              }
            },
            error: () => {
              this.router.navigate(['not-found']);
            },
          });
        }
      },
    });
  }

  toBlogs() {
    this.router.navigate(['']);
  }
}
