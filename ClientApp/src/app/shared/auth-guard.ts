import { Inject, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { AppConfig } from './models/app-config.model';

import { lastValueFrom, of } from 'rxjs';

@Injectable()
export class AdminstratorAuthGuard {
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string
  ) {}

  //use lastValuefrom to work with value directly in authGuard over http
  async canActivate() {
    let config = { configKey: '', configValue: '' };
    try {
      config = await lastValueFrom(
        this.http.get<AppConfig>(
          this.baseUrl + 'api/app/config/' + 'ALLOW_ADMIN'
        )
      );
    } catch {
      return false;
    }

    if (Number(config.configValue) == 1) {
      return true;
    } else {
      this.router.navigate(['not-found']);
      return false;
    }
  }
}
