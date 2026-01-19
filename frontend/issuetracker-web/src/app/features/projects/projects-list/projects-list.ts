import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { catchError, of, shareReplay } from 'rxjs';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';

import { ProjectsApiService, ProjectDto } from '../../../core/projects-api.service';
import { NewProjectDialogComponent, NewProjectDialogResult } from '../new-project-dialog/new-project-dialog';

@Component({
  selector: 'app-projects-list',
  standalone: true,
  imports: [CommonModule, RouterLink, MatTableModule, MatButtonModule, MatDialogModule],
  templateUrl: './projects-list.html'
})
export class ProjectsListComponent {
  private api = inject(ProjectsApiService);
  private dialog = inject(MatDialog);
  private router = inject(Router);

  error: string | null = null;
  displayedColumns = ['name', 'createdAt', 'actions'];

  projects$ = this.api.getProjects().pipe(
    shareReplay(1),
    catchError((err) => {
      console.error(err);
      this.error = 'Failed to load projects';
      return of([] as ProjectDto[]);
    })
  );

  openNewProjectDialog(): void {
    const ref = this.dialog.open<NewProjectDialogComponent, void, NewProjectDialogResult>(
      NewProjectDialogComponent,
      { width: '420px' }
    );

    ref.afterClosed().subscribe((result) => {
      if (!result) return;

      this.api.createProject({ name: result.name }).subscribe({
        next: (created) => this.router.navigate(['/projects', created.id]),
        error: (err) => {
          console.error(err);
          this.error = 'Failed to create project';
        }
      });
    });
  }
}
