import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { NgxWigModule } from 'ngx-wig';
import { AboutComponent } from './about/about.component';
import { BlogListComponent } from './blog-list/blog-list.component';
import { BlogService } from './shared/service/blog.service';
import { BlogAdministrationComponent } from './blog-admin/blog-admin.component';
import { CommonModule } from '@angular/common';
import { BlogEditorComponent } from './blog-admin/blog-editor.component';
import { AdminService } from './shared/service/admin.service';
import { LoginComponent } from './blog-admin/login.component';
import { NotFoundComponent } from './not-found-component';
import { BlogViewComponent } from './blog-view/blog-view.component';
import { AuthInterceptor } from './shared/service/interceptors';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AboutComponent,
    BlogListComponent,
    BlogViewComponent,
    BlogAdministrationComponent,
    BlogEditorComponent,
    LoginComponent,
    NotFoundComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    NgxWigModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'blog/:blogId', component: BlogViewComponent },
      { path: 'about-me', component: AboutComponent },
      { path: 'admin', component: BlogAdministrationComponent },
      { path: 'admin/blog/edit/:blogId', component: BlogEditorComponent },
      { path: 'admin/blog/add', component: BlogEditorComponent},
      { path: 'login', component: LoginComponent },
      { path: '**', component: NotFoundComponent },
    ]),
  ],
  providers: [
    BlogService, 
    AdminService,
    { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
