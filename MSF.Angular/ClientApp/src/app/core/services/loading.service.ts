import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {

  public inProcess: BehaviorSubject<boolean>;

  constructor() {
    this.inProcess = new BehaviorSubject(false);
  }

  show() {
    this.inProcess.next(true);
  }

  hide() {
    this.inProcess.next(false);
  }
}
