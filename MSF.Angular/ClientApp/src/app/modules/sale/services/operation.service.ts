import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Operation } from './../models/operation';

@Injectable({
  providedIn: 'root'
})
export class OperationService {

  private baseUrl = `${environment.apiUrl}/operation`;

  constructor(private http: HttpClient) { }

  lazy(workCenterId: number, type: string, filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?workCenterId=${workCenterId}&type=${type}&filter=${filter}&take=${take}&skip=${skip}`);
  }

  create(operation: Operation): Observable<any> {
    return this.http.post(`${this.baseUrl}`, operation);
  }

  edit(operation: Operation): Observable<any> {
    return this.http.put(`${this.baseUrl}`, operation);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  find(id: number): Observable<Operation> {
    return this.http.get<Operation>(`${this.baseUrl}/Find?id=${id}`);
  }

  findTotalPriceByWorkCenterControlAndType(workCenterControlId: number, type: string): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}/FindTotalPriceByWorkCenterControlAndType?workCenterControlId=${workCenterControlId}&type=${type}`);
  }
}
