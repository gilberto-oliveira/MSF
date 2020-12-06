import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay, startWith, delay } from 'rxjs/operators';
import { NavigationTitleService } from './../../services/navigation-title.service';
import { LoadingService } from '../../services/loading.service';
import { User } from '../../authentication/auth/models/user';
import { AuthenticationService } from '../../authentication/auth/services/authentication.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material';
import { UserChangePasswordComponent } from 'src/app/shared/components/user-change-password/user-change-password.component';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  public title: string;
  public currentUser: User;
  public fullUserName: string;
  public userName: string;

  helper = new JwtHelperService();

  showLoad$: Observable<boolean> = this.progressService.inProcess
    .pipe(
      startWith(null),
      delay(0),
      map(result => result),
      shareReplay()
    );

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver,
    private titleService: NavigationTitleService,
    private router: Router,
    private authenticationService: AuthenticationService,
    private progressService: LoadingService,
    public dialog: MatDialog) { }

  ngOnInit() {
    this.titleService.title.subscribe(updatedTitle => {
      this.title = updatedTitle;
    });
    this.getCurrentUser();
  }

  private getCurrentUser() {
    this.authenticationService.currentUser.subscribe(u => {
      this.currentUser = u;
      if (this.currentUser && this.currentUser.token) {
        this.userName = (this.helper.decodeToken(this.currentUser.token).sub as string);
        this.fullUserName = (this.helper.decodeToken(this.currentUser.token).full_name as string).trim();
      }
    });
  }

  logout() {
    this.authenticationService.logout();
    this.router.navigate(['/user/login']);
  }

  openRecoveryDialog(user: any = null) {
    const dialogRef = this.dialog.open(UserChangePasswordComponent, {
      disableClose: true,
      data: user
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        console.log(result);
      }
    });
  }
}
