import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { State } from '../models/State';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StateService {

  private baseUrl = `${environment.apiUrl}/state`;

  constructor(private http: HttpClient) { }

  public get(): Observable<State[]> {
    return this.http.get<State[]>(`${this.baseUrl}`);
  }

  public lazyStates(filter: string): Observable<State[]> {
    return this.http.get<State[]>(`${this.baseUrl}/Lazy?filter=${filter}`);
  }

}
