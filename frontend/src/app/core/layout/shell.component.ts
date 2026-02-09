import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-shell',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="shell">
      <header class="shell__header">
        <div class="brand">
          <div class="brand__mark">HD</div>
          <div class="brand__text">
            <span>HealthDesk</span>
            <small>Clinical Portal</small>
          </div>
        </div>
        <nav class="nav">
          <a routerLink="/app/home" routerLinkActive="is-active">Home</a>
          <a routerLink="/app/clinics" routerLinkActive="is-active">Clinics</a>
        </nav>
      </header>
      <main class="shell__content">
        <router-outlet />
      </main>
    </div>
  `,
  styleUrl: './shell.component.scss'
})
export class ShellComponent {}
