import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';
import { API_BASE_URL } from '../http/api-base-url.token';

export interface PrescriptionSummary {
  id: number;
  doctorName: string;
  patientName: string;
  content: string;
  createdAt: string;
}

@Injectable({ providedIn: 'root' })
export class PrescriptionsService {
  constructor(
    private readonly http: HttpClient,
    @Inject(API_BASE_URL) private readonly apiBaseUrl: string
  ) {}

  getMyPrescriptions(): Observable<PrescriptionSummary[]> {
    return this.http
      .get<PrescriptionSummary[]>(`${this.apiBaseUrl}/Prescriptions/by-patient`)
      .pipe(catchError(() => of([])));
  }
}
