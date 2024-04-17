import { Inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Blog } from '../models/blog.model';
import { Observable } from 'rxjs';

@Injectable()
export class BlogService {
  
  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {}

  getBlog(id : number) : Observable<Blog> {
    return this.http.get<Blog>(this.baseUrl + "api/blog/" +id)
  }

}
