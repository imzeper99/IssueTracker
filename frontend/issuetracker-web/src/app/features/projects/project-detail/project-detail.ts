import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { combineLatest, Subject, switchMap, catchError, of, shareReplay, startWith, finalize } from 'rxjs';

import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

import { ProjectsApiService, IssueDto } from '../../../core/projects-api.service';

type IssueStatus = 0 | 1 | 2;

@Component({
  selector: 'app-project-detail',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatSnackBarModule
  ],
  templateUrl: './project-detail.html'
})
export class ProjectDetailComponent {
  private route = inject(ActivatedRoute);
  private api = inject(ProjectsApiService);
  private snack = inject(MatSnackBar);

  private refresh$ = new Subject<void>();

  error: string | null = null;
  busy = false;

  // id del proyecto desde la ruta
  projectId$ = this.route.paramMap.pipe(
    switchMap((p) => of(p.get('id') ?? ''))
  );

  // ViewModel reactivo: cuando llamas refresh$.next(), vuelve a pedir project + issues
  vm$ = this.projectId$.pipe(
    switchMap((id) =>
      this.refresh$.pipe(
        startWith(void 0),
        switchMap(() =>
          combineLatest([
            this.api.getProject(id),
            this.api.getIssues(id)
          ])
        )
      )
    ),
    shareReplay(1),
    catchError((err) => {
      console.error(err);
      this.error = 'Failed to load project detail';
      return of(null);
    })
  );

  // Form para crear issue
  title = new FormControl<string>('', {
    nonNullable: true,
    validators: [Validators.required, Validators.minLength(2), Validators.maxLength(120)]
  });

  description = new FormControl<string>('', { nonNullable: true });

  createIssue(projectId: string): void {
    if (this.title.invalid) return;

    this.busy = true;
    this.error = null;

    this.api.createIssue(projectId, {
      title: this.title.value.trim(),
      description: this.description.value.trim() || null
    })
    .pipe(finalize(() => (this.busy = false)))
    .subscribe({
      next: () => {
        this.title.setValue('');
        this.description.setValue('');
        this.snack.open('Issue created', 'OK', { duration: 2000 });
        this.refresh$.next();
      },
      error: (err) => {
        console.error(err);
        this.error = 'Failed to create issue';
        this.snack.open('Failed to create issue', 'OK', { duration: 2500 });
      }
    });
  }

  updateStatus(issue: IssueDto, status: IssueStatus): void {
    this.busy = true;
    this.error = null;

    this.api.updateIssueStatus(issue.id, status)
      .pipe(finalize(() => (this.busy = false)))
      .subscribe({
        next: () => {
          this.snack.open('Status updated', 'OK', { duration: 1500 });
          this.refresh$.next();
        },
        error: (err) => {
          console.error(err);
          this.error = 'Failed to update issue status';
          this.snack.open('Failed to update status', 'OK', { duration: 2500 });
        }
      });
  }

  trackById(_: number, item: IssueDto): string {
    return item.id;
  }
}
