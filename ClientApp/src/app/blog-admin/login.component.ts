import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { AdminService } from '../shared/service/admin.service';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap, timer } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
})
export class LoginComponent implements OnInit {
  loginForm = this.fb.group({
    adminKey: ['', [Validators.required]],
  });

  resetForm = this.fb.group({
    confirmKey: [new FormControl<string>(''), [Validators.required]],
    newKey: [new FormControl<string>(''), [Validators.required]],
  });

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private adminService: AdminService
  ) {}

  ngOnInit(): void {}

  login(): void {
    if (this.loginForm.valid) {
      this.loginForm.disable();
      this.adminService
        .getAdminSession(this.loginForm.get('adminKey')?.value || '')
        .subscribe({
          next: (response: any) => {
            if (response['sessionId'] != null && response['sessionId']) {
              sessionStorage.setItem('admin-session-id', response['sessionId']);
              this.router.navigate(['../admin'], { relativeTo: this.route });
            }
            timer(7000).subscribe(() => {
              this.loginForm.enable(); // re-enable the form after 7 seconds
            });
          },
          error: () => {
            this.loginForm.enable();
          },
        });
    }
  }
}
