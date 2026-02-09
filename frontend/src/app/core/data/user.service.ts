import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { API_BASE_URL } from '../http/api-base-url.token';

export interface CurrentUser {
  id: number;
  fullName: string;
  email: string;
  role: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  constructor(
    private readonly http: HttpClient,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string
  ) {}

  getMe(): Observable<CurrentUser | null> {
    return this.http.get<CurrentUser>(`${this.apiBaseUrl}/Auth/me`).pipe(catchError(() => of(null)));
  }
}
