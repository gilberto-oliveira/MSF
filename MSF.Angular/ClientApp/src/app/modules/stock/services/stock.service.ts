import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Stock } from '../models/stock';
import { Provider } from './../../provider/models/provider';
import { Product } from '../../product/models/product';

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

  findProviderByFilterAndProduct(filter: string, productId: number): Observable<Provider[]> {
    return this.http.get<any>(`${this.baseUrl}/FindProviderByFilterAndProduct?filter=${filter}&productId=${productId}`);
  }

  findProductByFilter(filter: string): Observable<Product[]> {
    return this.http.get<any>(`${this.baseUrl}/FindProductByFilter?filter=${filter}`);
  }

  findTotalPriceByProductAndProvider(productId: number, providerId: number): Observable<number> {
    return this.http.get<any>(`${this.baseUrl}/FindTotalPriceByProductAndProvider?productId=${productId}&providerId=${providerId}`);
  }
}
