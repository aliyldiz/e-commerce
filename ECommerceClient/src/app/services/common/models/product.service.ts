import { Injectable } from '@angular/core';
import {Create_Product} from '../../../contracts/create-product';
import {HttpClientService} from '../http-client.service';
import {HttpErrorResponse} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private httpClientService: HttpClientService) { }

  create(product: Create_Product, successCallback?: any, errorCallback?: (errorMessage: string) => void) {
    this.httpClientService.post({
      controller: 'products'
    }, product).subscribe(result => {
      successCallback();
      }, (errorResponse: HttpErrorResponse) => {
        const _err: Array<{key: string, value: Array<string>}> = errorResponse.error;
        let message = '';
        _err.forEach((v, index) => {
          v.value.forEach((_v, _index) => {
            message += `${_v} <br>`;
          });
        });
      errorCallback(message);
      }
    );
  }
}
