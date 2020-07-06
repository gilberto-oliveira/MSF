import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NavigationTitleService {

  public title: BehaviorSubject<string>;

  constructor() {
    this.title = new BehaviorSubject('Carregando título...');
  }

  setTitle(title: string, defaultRoute:  boolean = true) {
    this.title.next(title);
  }

}
