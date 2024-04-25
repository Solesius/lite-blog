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
  routeBlogId = -1;
  blog: Blog | any = undefined;
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

  fillForm(blog: Blog) {
    this.blogEditForm.get('title')?.patchValue(blog.title);
    this.blogEditForm.get('summary')?.patchValue(blog.summary);
    this.blogEditForm.get('body')?.patchValue(blog.body);
  }

  addBlog() {
    const blog = this.extractBlogFromForm();
    this.adminService.createNewBlog(blog).subscribe(x => {this.router.navigate(['/admin',{}])})
  }
  
  editBlog() {
    const blog = this.extractBlogFromForm();
    blog.blogId = this.blog?.blogId
    this.adminService.updateBlog(blog).subscribe(x => {this.router.navigate(['/admin',{}])})
  }

  private extractBlogFromForm() : Blog {
    let blog : Blog | any = {}
    blog.title = this.removeScriptTags(this.blogEditForm.get('title')?.value)
    blog.summary =  this.removeScriptTags(this.blogEditForm.get('summary')?.value)
    blog.body = this.removeScriptTags(this.blogEditForm.get('body')?.value)
    return blog;
  }

  private setupRouterListener() {
    this.routeListener = this.route.paramMap.subscribe({
      next: (routeParams) => {
        const blogId = routeParams.get('blogId');
        if (blogId) { this.routeBlogId = +blogId }
        this.detectModeParameter()
      },
    });
  }

  private detectModeParameter() {
    this.route.queryParamMap.subscribe({
      next : (queryParameters) => {
        const modeParameter = queryParameters.get("mode")
        if(modeParameter && modeParameter == "add") {
          this.mode = "BLOG_ADD"
          console.log(this.mode)
        } else {
          this.loadBlog(this.routeBlogId)
        }
      } 
    })
  }

  private loadBlog(blogId: number) {
    this.blogService.getBlog(Number(blogId)).subscribe({
      next: (blog) => {
        this.blog = blog ? blog : this.blog;
        if (this.blog) {
          this.fillForm(this.blog);
        }
      },
    });
  }

  private removeScriptTags(content : string) {
    // Regular expression to match script tags
    var scriptRegex = /<script\b[^<]*(?:(?!<\/script>)<[^<]*)*<\/script>/gi;
    // Replace script tags with an empty string
    var cleanedHtml = content.replace(scriptRegex, "");
    return cleanedHtml;
}

  backToAdmin() {
    this.router.navigate(['admin']);
  }
}
