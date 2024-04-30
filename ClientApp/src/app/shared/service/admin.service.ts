import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Blog } from '../models/blog.model';
import { Observable } from 'rxjs';
import { SessionRequest } from '../models/session-request.model';
import { AdminSession } from '../models/admin-session';

@Injectable()
export class AdminService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  getAdminSession(key: string): Observable<AdminSession> {
    const request: SessionRequest = {
      key: btoa(key),
    };
    return this.http.post<AdminSession>(
      this.baseUrl + 'api/admin/session/create',
      request
    );
  }

  validateAdminSession(): Observable<boolean> {
    return this.http.post<boolean>(
      this.baseUrl + 'api/admin/session/valid',
      {}
    );
  }

  createNewBlog(blog: Blog): Observable<Blog> {
    return this.http.post<Blog>(this.baseUrl + 'api/blog/create', blog);
  }

  updateBlog(blog: Blog): Observable<Blog> {
    return this.http.post<Blog>(this.baseUrl + 'api/blog/update', blog);
  }

  deleteBlog(blogId: number): Observable<void> {
    return this.http.delete<void>(this.baseUrl + 'api/blog/delete/' + blogId);
  }
}
