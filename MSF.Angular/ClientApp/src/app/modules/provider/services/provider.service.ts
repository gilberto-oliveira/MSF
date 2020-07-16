import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProviderService {

  private baseUrl = `${environment.apiUrl}/provider`;

  constructor(private http: HttpClient) { }

  getLazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?filter=${filter}&take=${take}&skip=${skip}`);
  }
}
