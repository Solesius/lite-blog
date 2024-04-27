import { HttpClient } from '@angular/common/http';
import { Inject, Injectable, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AppConfigService {

  config : any[] = []

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    //should we allow admin route;
    this.http.get(this.baseUrl + "app/config" + "ALLOW_ADMIN")
  }
  

}
