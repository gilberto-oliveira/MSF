import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User, UserChangePassword } from './../../auth/models/user';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private baseUrl = `${environment.apiUrl}/user`;

  constructor(private http: HttpClient) { }

  getLazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?filter=${filter}&take=${take}&skip=${skip}`);
  }

  create(user: User): Observable<any> {
    return this.http.post(`${this.baseUrl}`, user);
  }

  edit(user: User): Observable<any> {
    return this.http.put(`${this.baseUrl}`, user);
  }
  
  changePassword(user: UserChangePassword): Observable<any> {
    return this.http.put(`${this.baseUrl}/ChangePassword`, user);
  }

  resetPassword(userId: number): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/ResetPassword`, userId);
  }

  lazyById(userId: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/LazyById?userId=${userId}`);
  }
}
