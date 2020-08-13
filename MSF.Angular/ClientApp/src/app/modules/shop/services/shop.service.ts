import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Shop } from './../models/shop';

@Injectable({
  providedIn: 'root'
})
export class ShopService {

  private baseUrl = `${environment.apiUrl}/shop`;

  constructor(private http: HttpClient) { }

  getLazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?filter=${filter}&take=${take}&skip=${skip}`);
  }

  create(Shop: Shop): Observable<any> {
    return this.http.post(`${this.baseUrl}`, Shop);
  }

  edit(Shop: Shop): Observable<any> {
    return this.http.put(`${this.baseUrl}`, Shop);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  find(filter: string): Observable<Shop[]> {
    return this.http.get<Shop[]>(`${this.baseUrl}/Find?filter=${filter}`);
  }

  findByUserRole(userId: number, roleId: number): Observable<Shop[]> {
    return this.http.get<Shop[]>(`${this.baseUrl}/FindByUserRole?userId=${userId}&roleId=${roleId}`);
  }

  findByCurrentUser(): Observable<Shop[]> {
    return this.http.get<Shop[]>(`${this.baseUrl}/FindByCurrentUser`);
  }
}
