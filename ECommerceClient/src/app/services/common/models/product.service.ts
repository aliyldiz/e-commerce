import { Injectable } from '@angular/core';
import {Create_Product} from '../../../contracts/create-product';
import {HttpClientService} from '../http-client.service';
import {HttpErrorResponse} from '@angular/common/http';
import {List_Product} from '../../../contracts/list-product';
import {first, firstValueFrom, lastValueFrom, Observable} from 'rxjs';
import {List_Product_Image} from '../../../contracts/list-product-image';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  constructor(private httpClientService: HttpClientService) { }

  create(product: Create_Product, successCallback?: () => void, errorCallback?: (errorMessage: string) => void) {
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

  async read(page: number = 0, size: number = 5, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void): Promise<{ totalProductCount: number; products: List_Product[] }> {
    const promiseData: Promise<{ totalProductCount: number; products: List_Product[] }> = this.httpClientService.get<{ totalProductCount: number; products: List_Product[] }>({
      controller: "products",
      queryString: `page=${page}&size=${size}`
    }).toPromise();
    promiseData.then(d => successCallBack())
      .catch((errorResponse: HttpErrorResponse) => errorCallBack(errorResponse.message))
    return await promiseData;
  }


  async delete(id: string) {
    const deleteObservable: Observable<any> = this.httpClientService.delete<any>({
      controller: 'products'
    }, id);
    await firstValueFrom(deleteObservable);
  }

  async readImages(id: string, successCallback?: () => void): Promise<List_Product_Image[]> {
    const getObservable: Observable<List_Product_Image[]> = this.httpClientService.get<List_Product_Image[]>({
      action: 'getproductimages',
      controller: 'products'
    }, id);

    const images: List_Product_Image[] = await firstValueFrom(getObservable);
    successCallback();
    return images;
  }

  async deleteImage(id: string, imageId: string, successCallback?: () => void) {
    const deleteObservable = this.httpClientService.delete({
      action: 'deleteproductimage',
      controller: 'products',
      queryString: `imageId=${imageId}`
    }, id)
    await firstValueFrom(deleteObservable);
    successCallback();
  }

  async changeShowcaseImage(imageId: string, productId: string, successCallback?: () => void): Promise<void> {
    const changeShowcaseImageObservable = this.httpClientService.get({
      action: 'changeshowcaseimage',
      controller: 'products',
      queryString: `imageId=${imageId}&productId=${productId}`
    });
    await firstValueFrom(changeShowcaseImageObservable);
    successCallback();
  }
}
