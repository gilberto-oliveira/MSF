import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WorkCenterControl } from './../models/work-center-control';

@Injectable({
  providedIn: 'root'
})
export class WorkCenterControlService {
  
  private baseUrl = `${environment.apiUrl}/workcentercontrol`;

  constructor(private http: HttpClient) { }

  lazyOpenedByWorkCenterId(workCenterId: number): Observable<WorkCenterControl> {
    return this.http.get<any>(`${this.baseUrl}/LazyOpenedByWorkCenter?workCenterId=${workCenterId}`);
  }

  finishSaleProcess(workCenterControlId: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/FinishSaleProcess`, workCenterControlId);
  }
}
