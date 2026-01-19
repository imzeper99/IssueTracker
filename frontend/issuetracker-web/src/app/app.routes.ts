import { Routes } from '@angular/router';
import { ProjectsListComponent } from './features/projects/projects-list/projects-list';
import { ProjectDetailComponent } from './features/projects/project-detail/project-detail';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'projects' },
  { path: 'projects', component: ProjectsListComponent },
  { path: 'projects/:id', component: ProjectDetailComponent },
  { path: '**', redirectTo: 'projects' }
];
