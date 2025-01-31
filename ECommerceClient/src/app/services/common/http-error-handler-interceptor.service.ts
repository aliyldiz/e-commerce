import {Injectable} from '@angular/core';
import {HttpEvent, HttpHandler, HttpInterceptor, HttpRequest, HttpStatusCode} from '@angular/common/http';
import {catchError, Observable, of} from 'rxjs';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../ui/custom-toastr.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorHandlerInterceptorService implements HttpInterceptor{
  constructor(private toastrService: CustomToastrService) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(catchError(error => {
      switch (error.status) {
        case HttpStatusCode.Unauthorized:
          this.toastrService.message('Unauthorized', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight
          });
          break;
        case HttpStatusCode.InternalServerError:
          this.toastrService.message('Server Error', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight
          });
          break;
        case HttpStatusCode.BadRequest:
          this.toastrService.message('Bad Request', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight
          });
          break;
        case HttpStatusCode.NotFound:
          this.toastrService.message('Not Found', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight
          });
          break;
        default:
          this.toastrService.message('An error occurred', 'error', {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight
          });
          break;
      }
    return of(error);
    }));
  }
}
