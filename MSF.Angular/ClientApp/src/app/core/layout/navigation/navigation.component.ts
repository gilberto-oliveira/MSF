import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';
import { Observable } from 'rxjs';
import { map, shareReplay } from 'rxjs/operators';
import { NavigationTitleService } from './../../services/navigation-title.service';
import { LoadingService } from '../../services/loading.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit  {

  public title: string;
  public showLoad: boolean;

  isHandset$: Observable<boolean> = this.breakpointObserver.observe(Breakpoints.Handset)
    .pipe(
      map(result => result.matches),
      shareReplay()
    );

  constructor(private breakpointObserver: BreakpointObserver,
              private titleService: NavigationTitleService,
              private progressService: LoadingService) {}

  ngOnInit(): void {
    this.titleService.title.subscribe(updatedTitle => {
      this.title = updatedTitle;
    });
    
    this.progressService.inProcess.subscribe(toggle => {
      this.showLoad = toggle;
    });
  }
}
