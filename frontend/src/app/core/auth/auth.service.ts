import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { API_BASE_URL } from '../http/api-base-url.token';
import { TokenStorageService } from './token-storage.service';

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  id: number;
  fullName: string;
  email: string;
  accessToken: string;
  refreshToken: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  constructor(
    private readonly http: HttpClient,
    private readonly tokenStorage: TokenStorageService,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string
  ) {}

  login(payload: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiBaseUrl}/Auth/login`, payload)
      .pipe(
        tap((response) => {
          this.tokenStorage.setTokens(response.accessToken, response.refreshToken);
        })
      );
  }

  logout(): void {
    this.tokenStorage.clear();
  }

  isAuthenticated(): boolean {
    const token = this.tokenStorage.getAccessToken();
    if (!token) {
      return false;
    }

    const expiresAt = this.getJwtExpiry(token);
    if (!expiresAt || expiresAt <= Date.now()) {
      this.tokenStorage.clear();
      return false;
    }

    return true;
  }

  private getJwtExpiry(token: string): number | null {
    try {
      const parts = token.split('.');
      if (parts.length < 2) {
        return null;
      }

      const payload = JSON.parse(atob(parts[1].replace(/-/g, '+').replace(/_/g, '/'))) as {
        exp?: number;
      };

      if (!payload.exp || typeof payload.exp !== 'number') {
        return null;
      }

      return payload.exp * 1000;
    } catch {
      return null;
    }
  }
}
