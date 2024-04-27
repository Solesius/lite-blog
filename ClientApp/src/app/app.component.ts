import { Component } from '@angular/core';
import { AppConfigService } from './shared/service/app.config.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  constructor(private configService : AppConfigService) {
  
  }
  title = 'app';
}
