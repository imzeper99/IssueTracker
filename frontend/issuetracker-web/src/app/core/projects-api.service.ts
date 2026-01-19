import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProjectDto {
  id: string;
  name: string;
  createdAt: string;
}

export interface CreateProjectRequest {
  name: string;
}

@Injectable({ providedIn: 'root' })
export class ProjectsApiService {
  private readonly baseUrl = '/api/projects';

  constructor(private http: HttpClient) { }

  getProjects(): Observable<ProjectDto[]> {
    return this.http.get<ProjectDto[]>(this.baseUrl);
  }

  createProject(payload: CreateProjectRequest): Observable<ProjectDto> {
    return this.http.post<ProjectDto>(this.baseUrl, payload);
  }
}
