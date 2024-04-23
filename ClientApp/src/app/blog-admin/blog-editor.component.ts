import { AfterViewInit, Component } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
import { BlogService } from '../shared/service/blog.service';
import { Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { Blog } from '../shared/models/blog.model';
import { AdminService } from '../shared/service/admin.service';

@Component({
  selector: 'app-blog-editor-component',
  templateUrl: './blog-editor.component.html',
})
export class BlogEditorComponent implements AfterViewInit {
  mode = 'BLOG_EDIT';
  editBlog: any = undefined;
  routeListener: Subscription = new Subscription();

  blogEditForm: FormGroup = this.formBuilder.group({
    title: new FormControl<string>(''),
    summary: new FormControl<string>(''),
    body: new FormControl<string>(''),
  });

  constructor(
    private formBuilder: FormBuilder,
    private blogService: BlogService,
    private adminService: AdminService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngAfterViewInit(): void {
    const currentSession = sessionStorage.getItem('admin-session-id');
    if (!currentSession || currentSession == '' || currentSession == null) {
      this.logout();
    } else {
      this.adminService.validateAdminSession(currentSession).subscribe({
        next: (sessionValid) => {
          if (sessionValid === true) {
            this.setupRouterListener();
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

  loadBlog(blog: Blog) {
    this.blogEditForm.get('title')?.patchValue(blog.title);
    this.blogEditForm.get('summary')?.patchValue(blog.summary);
    this.blogEditForm.get('body')?.patchValue(blog.body);
  }

  private setupRouterListener() {
    this.routeListener = this.route.paramMap.subscribe({
      next: (routeParams) => {
        const blogId = routeParams.get('blogId');
        if (blogId) {
          this.blogService.getBlog(Number(blogId)).subscribe({
            next: (blog) => {
              this.editBlog = blog ? blog : this.editBlog;
              if (this.editBlog) {
                this.loadBlog(this.editBlog);
              }
            },
          });
        }
      },
    });
  }

  backToAdmin() {
    this.router.navigate(['admin']);
  }
}
