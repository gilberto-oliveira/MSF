import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  private baseUrl = `${environment.apiUrl}/role`;

  constructor(private http: HttpClient) { }

  lazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy/?filter=${filter}&take=${take}&skip=${skip}`);
  }

  lazyByUser(filter: string, take: number, skip: number, userId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/LazyByUser/?filter=${filter}&take=${take}&skip=${skip}&userId=${userId}`);
  }

  getWithoutUser(userId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/GetWithoutUser/?userId=${userId}`);
  }

  create(userId: number, roleId: number): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/Create`, { userId, roleId });
  }

  createUserRoleShop(userId: number, roleId: number, shopId: number): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/CreateUserRoleShop`, { userId, roleId, shopId });
  }

  delete(userId: number, roleId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/Delete/?userId=${userId}&roleId=${roleId}`);
  }

  deleteUserRoleShop(userId: number, roleId: number, shopId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/DeleteUserRoleShop/?userId=${userId}&roleId=${roleId}&shopId=${shopId}`);
  }
}
