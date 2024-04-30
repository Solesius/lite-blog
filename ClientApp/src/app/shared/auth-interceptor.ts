import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const sessionId = sessionStorage.getItem('admin-session-id');
    let authReq = request.clone({});
    if (sessionId) {
      authReq = request.clone({
        setHeaders: {
          'X-SESSION-ID': sessionId,
        },
      });
    }

    return next.handle(authReq);
  }
}
