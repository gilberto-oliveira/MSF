import { MatSnackBar } from '@angular/material';
import { NavigationTitleService } from './../../core/services/navigation-title.service';

export abstract class BaseComponent {

  constructor(protected snackBar: MatSnackBar,
              protected _titleService: NavigationTitleService) { }

  public openSnackBarBottom(message: string, action: string) {
    this.openSnackBarFull(message, action, 5000, 'bottom');
  }

  public openSnackBarTop(message: string, action: string) {
    this.openSnackBarFull(message, action, 5000, 'top');
  }

  public openSnackBarFull(message: string, action: string, duration: number, verticalPosition: any) {
    this.snackBar.open(message, action, {
      duration: duration,
      verticalPosition: verticalPosition
    });
  }

  public setTitle(title: string) {
    this._titleService.setTitle(title);
  }
}
