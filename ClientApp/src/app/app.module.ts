import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
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

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AboutComponent,
    BlogListComponent,
    BlogAdministrationComponent,
    BlogEditorComponent
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
      { path: 'about-me', component: AboutComponent },
      { path: 'admin', component: BlogAdministrationComponent },
      { path: 'admin/blog/edit/:blogId', component: BlogEditorComponent },
    ]),
  ],
  providers: [BlogService],
  bootstrap: [AppComponent],
})
export class AppModule {}
