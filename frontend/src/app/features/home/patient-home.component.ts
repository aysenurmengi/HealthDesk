import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { combineLatest, map } from 'rxjs';
import { AppointmentsService } from '../../core/data/appointments.service';
import { PrescriptionsService } from '../../core/data/prescriptions.service';
import { UserService } from '../../core/data/user.service';

@Component({
  selector: 'app-patient-home',
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
    <section class="home">
      <div class="home__left">
        <div class="cta-card">
          <div>
            <h2>Yeni bir randevu al</h2>
            <p>Uygun klinikleri inceleyip doktor sec.</p>
          </div>
          <a class="cta-card__action" routerLink="/clinics">Randevu Al</a>
        </div>

        <div class="panel">
          <div class="panel__header">
            <h3>Son randevular</h3>
            <span>En guncel 3 kayit</span>
          </div>
          <div class="panel__body" *ngIf="vm$ | async as vm">
            <div *ngIf="vm.appointments.length === 0" class="empty">
              Henuz randevu yok.
            </div>
            <div class="appointment" *ngFor="let appt of vm.appointments">
              <div>
                <p class="title">{{ appt.doctorName }}</p>
                <span>{{ appt.startsAt | date: 'dd MMM yyyy, HH:mm' }}</span>
              </div>
              <span class="status">{{ appt.status }}</span>
            </div>
          </div>
        </div>
      </div>

      <aside class="home__right" *ngIf="vm$ | async as vm">
        <div class="panel">
          <div class="panel__header">
            <h3>Kisisel Bilgilerim</h3>
          </div>
          <div class="panel__body" *ngIf="vm.me; else noProfile">
            <div class="info-row">
              <span>Ad Soyad</span>
              <strong>{{ vm.me.fullName }}</strong>
            </div>
            <div class="info-row">
              <span>Email</span>
              <strong>{{ vm.me.email }}</strong>
            </div>
            <div class="info-row">
              <span>Rol</span>
              <strong>{{ vm.me.role }}</strong>
            </div>
          </div>
          <ng-template #noProfile>
            <div class="empty">Profil bilgisi bulunamadi.</div>
          </ng-template>
        </div>

        <div class="panel">
          <div class="panel__header">
            <h3>Recetelerim</h3>
            <span>Son 3 recete</span>
          </div>
          <div class="panel__body">
            <div *ngIf="vm.prescriptions.length === 0" class="empty">
              Henuz recete yok.
            </div>
            <div class="prescription" *ngFor="let rx of vm.prescriptions">
              <p class="title">{{ rx.doctorName }}</p>
              <span>{{ rx.createdAt | date: 'dd MMM yyyy' }}</span>
              <p class="note">{{ rx.content }}</p>
            </div>
          </div>
        </div>
      </aside>
    </section>
  `,
  styleUrl: './patient-home.component.scss'
})
export class PatientHomeComponent {
  private readonly userService = inject(UserService);
  private readonly appointmentsService = inject(AppointmentsService);
  private readonly prescriptionsService = inject(PrescriptionsService);

  vm$ = combineLatest([
    this.userService.getMe(),
    this.appointmentsService.getMyAppointments(),
    this.prescriptionsService.getMyPrescriptions()
  ]).pipe(
    map(([me, appointments, prescriptions]) => ({
      me,
      appointments: appointments
        .slice()
        .sort((a, b) => new Date(b.startsAt).getTime() - new Date(a.startsAt).getTime())
        .slice(0, 3),
      prescriptions: prescriptions
        .slice()
        .sort((a, b) => new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime())
        .slice(0, 3)
    }))
  );
}
