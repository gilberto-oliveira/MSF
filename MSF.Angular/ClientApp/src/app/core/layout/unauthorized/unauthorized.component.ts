import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NavigationTitleService } from '../../services/navigation-title.service';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {

  url: string;

  constructor(private router: ActivatedRoute,
              private titleService: NavigationTitleService) {}

  ngOnInit() {
    this.titleService.setTitle('Acesso Negado');
    this.router.queryParams.subscribe(params => {
      this.url = params.returnUrl;
    });
  }

}
