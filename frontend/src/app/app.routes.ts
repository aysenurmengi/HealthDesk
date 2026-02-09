import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { LoginComponent } from './features/auth/login/login.component';
import { ShellComponent } from './core/layout/shell.component';
import { ClinicsComponent } from './features/clinics/clinics.component';
import { PatientHomeComponent } from './features/home/patient-home.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login', component: LoginComponent },
  {
    path: 'app',
    component: ShellComponent,
    canActivate: [authGuard],
    children: [
      { path: 'home', component: PatientHomeComponent },
      { path: 'clinics', component: ClinicsComponent },
      { path: '', pathMatch: 'full', redirectTo: 'home' }
    ]
  },
  { path: '**', redirectTo: 'login' }
];
