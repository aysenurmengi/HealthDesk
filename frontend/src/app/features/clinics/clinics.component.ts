import { Component } from '@angular/core';

@Component({
  selector: 'app-clinics',
  standalone: true,
  template: `
    <section class="page">
      <header>
        <h2>Clinics</h2>
        <p>Filter by city and pick a clinic to continue.</p>
      </header>
    </section>
  `,
  styleUrl: './clinics.component.scss'
})
export class ClinicsComponent {}
