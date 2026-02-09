import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { API_BASE_URL } from '../http/api-base-url.token';

export interface AppointmentSummary {
  id: number;
  doctorName: string;
  patientName: string;
  status: string;
  startsAt: string;
  notes?: string | null;
}

@Injectable({ providedIn: 'root' })
export class AppointmentsService {
  constructor(
    private readonly http: HttpClient,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string
  ) {}

  getMyAppointments(): Observable<AppointmentSummary[]> {
    return this.http
      .get<AppointmentSummary[]>(`${this.apiBaseUrl}/Appointments/mine`)
      .pipe(catchError(() => of([])));
  }
}
