import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { AdminService } from '../shared/service/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { timer } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent {
  loginForm = this.fb.group({
    adminKey: ['', [Validators.required]],
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private adminService: AdminService
  ) {}

  login(): void {
    if (this.loginForm.valid) {
      this.loginForm.disable();
      this.adminService
        .getAdminSession(this.loginForm.get('adminKey')?.value || '')
        .subscribe({
          next: (session) => {
            if (session.sessionId != null && session.sessionId) {
              sessionStorage.setItem('admin-session-id', session['sessionId']);
              this.router.navigate(['../admin'], { relativeTo: this.route });
            }
            timer(7000).subscribe(() => {
              this.loginForm.enable();
            });
          },
          error: () => {
            this.loginForm.enable();
          },
        });
    }
  }
}
