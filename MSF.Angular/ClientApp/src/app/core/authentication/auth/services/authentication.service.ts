import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { User } from './../models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
  private currentUserSubject: BehaviorSubject<User>;
  public currentUser: Observable<User>;

  private helper: JwtHelperService;

  constructor(private http: HttpClient) {
    this.currentUserSubject = new BehaviorSubject<User>(JSON.parse(localStorage.getItem('currentUser')));
    this.currentUser = this.currentUserSubject.asObservable();
    this.helper = new JwtHelperService();
  }

  public get currentUserValue(): User {
    return this.currentUserSubject.value;
  }

  login(email: string, passwordHash: string) {
    return this.http.post<any>(`${environment.apiUrl}/user/login`, { email, passwordHash })
      .pipe(map(user => {
        localStorage.setItem('currentUser', JSON.stringify(user));
        this.currentUserSubject.next(user);
        return user;
      }));
  }

  public refresh(): Observable<User> {
    console.log("Chamada do refresh");
    const refreshToken = this.currentUserValue.refreshToken;

    return this.http.post<User>(`${environment.apiUrl}/user/refresh`, { refreshToken })
      .pipe(map((user: User) => {
        console.log("Retorno do refresh");
        if (user && user.token) {
          localStorage.setItem('currentUser', JSON.stringify({ authenticated: user.authenticated, token: user.token, refreshToken: refreshToken }));
          user.refreshToken = refreshToken;
          this.currentUserSubject.next(user);
        }
        return user;
      }));
  }

  logout() {
    localStorage.removeItem('currentUser');
    this.currentUserSubject.next(null);
  }

  roleMatch(allowedRoles): boolean {
    let isMatch = false;
    const payLoad = this.helper.decodeToken(JSON.parse(localStorage.getItem('currentUser')).token);
    const userRole = payLoad.role;
    allowedRoles.forEach(element => {
      if (userRole === element) {
        isMatch = true;
        return false;
      }
    });
    return isMatch;
  }
}
