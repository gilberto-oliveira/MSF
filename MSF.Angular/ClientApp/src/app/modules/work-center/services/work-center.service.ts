import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { WorkCenter } from '../models/work-center';

@Injectable({
  providedIn: 'root'
})
export class WorkCenterService {

  private baseUrl = `${environment.apiUrl}/workcenter`;

  constructor(private http: HttpClient) { }

  getLazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?filter=${filter}&take=${take}&skip=${skip}`);
  }

  create(workcenter: WorkCenter): Observable<any> {
    return this.http.post(`${this.baseUrl}`, workcenter);
  }

  edit(workcenter: WorkCenter): Observable<any> {
    return this.http.put(`${this.baseUrl}`, workcenter);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
