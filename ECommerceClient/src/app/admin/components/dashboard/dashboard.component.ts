import {Component, OnInit} from '@angular/core';
import {BaseComponent, SpinnerType} from '../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {AlertifyService} from '../../../services/admin/alertify.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent extends BaseComponent implements OnInit {
  constructor(private alertify: AlertifyService, spinner: NgxSpinnerService) {
    super(spinner);
  }

  ngOnInit() {
  }
}
