import {Component} from '@angular/core';
import {AuthService} from './services/common/auth.service';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from './services/ui/custom-toastr.service';
import {Router} from '@angular/router';
import {HttpClientService} from './services/common/http-client.service';

declare var $: any

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(public authService: AuthService, private toastrService: CustomToastrService, private router: Router, private httpClientService: HttpClientService) {
    httpClientService.put({
      controller: "baskets"
    }, {
      basketItemId: "bf85f967-eb89-4242-aa3f-b1e0e384e597",
      quantity: 17
    }).subscribe(data =>{
      debugger;
    });

    this.authService.identityCheck();
  }

  signOut() {
    localStorage.removeItem("token");
    this.authService.identityCheck();
    this.router.navigate([""]);
    this.toastrService.message("You have successfully signed out", "Signed Out", {
      messageType: ToastrMessageType.Warning,
      position: ToastrPosition.TopRight
    });
  }
}
