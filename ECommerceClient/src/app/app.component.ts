import {Component} from '@angular/core';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from './services/ui/custom-toastr.service';
import {AlertifyService, MessageType, Position} from './services/admin/alertify.service';

declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ECommerceClient';

  constructor(private toastrService: CustomToastrService) {

  }

  ngOnInit() {

  }

  m() {
    this.toastrService.message('Hello world!', 'Toastr fun!', { messageType: ToastrMessageType.Error, position: ToastrPosition.TopRight, timeOut: 5000 });
    this.toastrService.message('Hello world!', 'Toastr fun!', { messageType: ToastrMessageType.Warning, position: ToastrPosition.TopCenter, timeOut: 6000 });
    this.toastrService.message('Hello world!', 'Toastr fun!', { messageType: ToastrMessageType.Success, position: ToastrPosition.BottomRight, timeOut: 7000 });
    this.toastrService.message('Hello world!', 'Toastr fun!', { messageType: ToastrMessageType.Success, position: ToastrPosition.BottomCenter, timeOut: 8000 });
    this.toastrService.message('Hello world!', 'Toastr fun!', { messageType: ToastrMessageType.Success, position: ToastrPosition.BottomLeft, timeOut: 9000 });
  }

}
