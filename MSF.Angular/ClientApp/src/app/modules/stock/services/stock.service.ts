import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Stock } from '../models/stock';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  private baseUrl = `${environment.apiUrl}/stock`;

  constructor(private http: HttpClient) { }

  getLazy(filter: string, take: number, skip: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/Lazy?filter=${filter}&take=${take}&skip=${skip}`);
  }

  create(stock: Stock): Observable<any> {
    return this.http.post(`${this.baseUrl}`, stock);
  }

  edit(stock: Stock): Observable<any> {
    return this.http.put(`${this.baseUrl}`, stock);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}
