import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="login">
      <div class="login__panel">
        <div class="login__brand">
          <div class="mark">HD</div>
          <div>
            <h1>HealthDesk</h1>
            <p>Clinical workflow starts here.</p>
          </div>
        </div>

        <form [formGroup]="form" (ngSubmit)="submit()" class="login__form">
          <label>
            <span>Email</span>
            <input type="email" formControlName="email" placeholder="name@clinic.com" />
          </label>

          <label>
            <span>Password</span>
            <input type="password" formControlName="password" placeholder="********" />
          </label>

          <div class="login__error" *ngIf="errorMessage">{{ errorMessage }}</div>

          <button type="submit" [disabled]="form.invalid || isLoading">
            {{ isLoading ? 'Signing in...' : 'Sign In' }}
          </button>
        </form>

        <div class="login__hint">
          <span>Patient access is private.</span>
          <strong>Contact admin for doctor onboarding.</strong>
        </div>
      </div>

      <div class="login__visual">
        <div class="orb"></div>
        <div class="grid"></div>
        <div class="insights">
          <div class="insight">
            <p>Local time</p>
            <h2>{{ now | date: 'HH:mm:ss' }}</h2>
          </div>
          <div class="insight">
            <p>Security</p>
            <h2>Tokenized</h2>
          </div>
          <div class="insight">
            <p>Session mode</p>
            <h2>Clinical</h2>
          </div>
        </div>
      </div>
    </div>
  `,
  styleUrl: './login.component.scss'
})
export class LoginComponent implements OnInit, OnDestroy {
  isLoading = false;
  errorMessage = '';
  now = new Date();
  private timerId?: number;
  private readonly fb = inject(FormBuilder);
  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]]
  });

  constructor(
    private readonly authService: AuthService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.timerId = window.setInterval(() => {
      this.now = new Date();
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.timerId) {
      window.clearInterval(this.timerId);
    }
  }

  submit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.errorMessage = '';
    this.isLoading = true;
    const value = this.form.getRawValue();
    this.authService.login({ email: value.email ?? '', password: value.password ?? '' }).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigateByUrl('/app/home');
      },
      error: () => {
        this.isLoading = false;
        this.errorMessage = 'Email veya sifre hatali. Tekrar deneyin.';
      }
    });
  }
}

