import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Blog } from '../models/blog.model';
import { Observable } from 'rxjs';
import { SessionRequest } from '../models/session-request.model';
import { SessionCheckRequest } from '../models/session-check-request.model';

@Injectable()
export class AdminService {
  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  getAdminSession(key: string): Observable<string> {
    const request: SessionRequest = {
      key: btoa(key),
    };
    return this.http.post<string>(
      this.baseUrl + 'api/admin/session/create',
      request
    );
  }

  validateAdminSession(key: string): Observable<boolean> {
    const request: SessionCheckRequest = {
      sessionId: key,
    };
    return this.http.post<boolean>(
      this.baseUrl + 'api/admin/session/valid',
      request
    );
  }

  getBlogs(): Observable<Array<Blog>> {
    return this.http.get<Array<Blog>>(this.baseUrl + 'api/blog/list');
  }
}
