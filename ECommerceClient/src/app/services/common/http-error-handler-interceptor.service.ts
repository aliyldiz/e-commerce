import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode} from '@angular/common/http';
import {catchError, Observable, of} from 'rxjs';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../ui/custom-toastr.service';
import {UserAuthService} from './models/user-auth.service';
import {ActivatedRoute, Router} from '@angular/router';
import {NgxSpinnerService} from 'ngx-spinner';
import {SpinnerType} from '../../base/base.component';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor{
  constructor(private toastrService: CustomToastrService, private userAuthService: UserAuthService, private router: Router, private spinner: NgxSpinnerService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {
      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          this.userAuthService.refreshTokenLogin(localStorage.getItem('refreshToken'), (state) => {
            debugger;
            if (!state) {
              const url = this.router.url;
              if (url == "/products")
                this.toastrService.message('If you want cart add product, you must login', 'warning', {
                  messageType: ToastrMessageType.Warning,
                  position: ToastrPosition.BottomRight
                });
              else
                this.toastrService.message('Unauthorized', 'error', {
                  messageType: ToastrMessageType.Error,
                  position: ToastrPosition.BottomRight
                });
            }
          }).then(data => {});
          break;
        case HttpStatusCode.InternalServerError:
          this.toastrService.message('Server Error', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.BottomRight
          });
          break;
        case HttpStatusCode.BadRequest:
          this.toastrService.message('Bad Request', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.BottomRight
          });
          break;
        case HttpStatusCode.NotFound:
          this.toastrService.message('Not Found', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.BottomRight
          });
          break;
        default:
          // this.toastrService.message('Unknown Error', 'error', {
          //   messageType: ToastrMessageType.Error,
          //   position: ToastrPosition.BottomRight
          // });
          break;
      }
      this.spinner.hide(SpinnerType.BallAtom);
    return of(error);
    }));
  }
}
