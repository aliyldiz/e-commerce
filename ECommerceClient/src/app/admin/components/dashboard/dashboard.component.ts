import {Component, OnInit} from '@angular/core';
import {BaseComponent} from '../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {AlertifyService, MessageType, Position} from '../../../services/admin/alertify.service';
import {SignalrService} from '../../../services/common/signalr.service';
import {ReceiveFunctions} from '../../../constants/receive-functions';
import {HubUrls} from '../../../constants/hub-urls';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.scss'
})
export class DashboardComponent extends BaseComponent implements OnInit {
  constructor(private alertify: AlertifyService, spinner: NgxSpinnerService, private signalrService: SignalrService) {
    super(spinner);
  }

  ngOnInit() {
    this.signalrService.on(HubUrls.ProductHub, ReceiveFunctions.ProductAddedMessageReceiveFuntion, message => {
      this.alertify.message(message, {
        messageType: MessageType.Notify,
        position: Position.TopCenter
      })
    });
    this.signalrService.on(HubUrls.OrderHub, ReceiveFunctions.OrderAddedMessageReceiveFuntion, message => {
      this.alertify.message(message, {
        messageType: MessageType.Notify,
        position: Position.TopCenter
      })
    });
  }
}
