import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Provider } from './../models/provider';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {

  private baseUrl = `${environment.apiUrl}/provider`;

  constructor(private http: HttpClient) { }

  getLazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?filter=${filter}&take=${take}&skip=${skip}`);
  }

  create(provider: Provider): Observable<any> {
    return this.http.post(`${this.baseUrl}`, provider);
  }

  edit(provider: Provider): Observable<any> {
    return this.http.put(`${this.baseUrl}`, provider);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
