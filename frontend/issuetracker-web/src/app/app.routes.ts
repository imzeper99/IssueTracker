import { Routes } from '@angular/router';
import { ShellComponent } from './layout/shell/shell';
import { ProjectsListComponent } from './features/projects/projects-list/projects-list';
import { ProjectDetailComponent } from './features/projects/project-detail/project-detail';

export const routes: Routes = [
  {
    path: '',
    component: ShellComponent,
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'projects' },
      { path: 'projects', component: ProjectsListComponent },
      { path: 'projects/:id', component: ProjectDetailComponent },
      { path: '**', redirectTo: 'projects' }
    ]
  }
];
