import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectsApiService, ProjectDto } from './core/projects-api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App implements OnInit {
  projects: ProjectDto[] = [];
  loading = false;
  error: string | null = null;

  constructor(private api: ProjectsApiService) { }

  ngOnInit(): void {
    this.reload();
  }

  reload(): void {
    this.loading = true;
    this.error = null;

    this.api.getProjects().subscribe({
      next: (data) => {
        this.projects = data;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load projects. Is the backend running?';
        this.loading = false;
        console.error(err);
      }
    });
  }
}
