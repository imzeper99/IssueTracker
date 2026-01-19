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

export interface IssueDto {
  id: string;
  projectId: string;
  title: string;
  description?: string | null;
  status: number; // 0 Open, 1 InProgress, 2 Done (según tu enum backend)
  createdAt: string;
  updatedAt?: string | null;
}

export interface CreateIssueRequest {
  title: string;
  description?: string | null;
}

export interface UpdateIssueStatusRequest {
  status: number;
}

@Injectable({ providedIn: 'root' })
export class ProjectsApiService {
  private readonly projectsUrl = '/api/projects';
  private readonly issuesUrl = '/api/issues';

  constructor(private http: HttpClient) {}

  // Projects
  getProjects(): Observable<ProjectDto[]> {
    return this.http.get<ProjectDto[]>(this.projectsUrl);
  }

  createProject(payload: CreateProjectRequest): Observable<ProjectDto> {
    return this.http.post<ProjectDto>(this.projectsUrl, payload);
  }

  getProject(id: string): Observable<ProjectDto> {
    return this.http.get<ProjectDto>(`${this.projectsUrl}/${id}`);
  }

  // Issues (por proyecto)
  getIssues(projectId: string): Observable<IssueDto[]> {
    return this.http.get<IssueDto[]>(`${this.projectsUrl}/${projectId}/issues`);
  }

  createIssue(projectId: string, payload: CreateIssueRequest): Observable<IssueDto> {
    return this.http.post<IssueDto>(`${this.projectsUrl}/${projectId}/issues`, payload);
  }

  // Issues (endpoint directo por issueId)
  updateIssueStatus(issueId: string, status: number): Observable<IssueDto> {
    const body: UpdateIssueStatusRequest = { status };
    return this.http.patch<IssueDto>(`${this.issuesUrl}/${issueId}/status`, body);
  }
}
