import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';

@Injectable({ providedIn: 'root' })
export class AuthGuardService implements CanActivate {
    constructor(
        private router: Router,
        private authenticationService: AuthenticationService
    ) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        const currentUser = this.authenticationService.currentUserValue;
        if (currentUser) {
            const roles = route.data.permittedRoles as Array<string>;
            if (roles) {
                if (this.authenticationService.roleMatch(roles)) {
                    return true;
                } else {
                    this.router.navigate(['/unauthorized'], { queryParams: { returnUrl: state.url } });
                    return false;
                }
            }
            return true;
        }
        this.router.navigate(['/user/login'], { queryParams: { returnUrl: state.url } });
        return false;
    }
}
