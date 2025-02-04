import { Injectable } from '@angular/core';
import {HttpClientService} from '../http-client.service';
import {CreateOrder} from '../../../contracts/order/create-order';
import {firstValueFrom, Observable} from 'rxjs';
import {ListOrder} from '../../../contracts/order/list-order';
import {SingleOrder} from '../../../contracts/order/single-order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private httpClientService: HttpClientService) { }

  async create(order: CreateOrder): Promise<void> {
    const observable: Observable<any> = this.httpClientService.post({
      controller: "orders",
    }, order);
    await firstValueFrom(observable);
  }

  async getAllOrder(page: number = 0, size: number = 5, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void): Promise<{ totalOrderCount: number; orders: ListOrder[] }> {
    const observable: Observable<{ totalOrderCount: number; orders: ListOrder[] }> = this.httpClientService.get({
      controller: "orders",
      queryString: `page=${page}&size=${size}`
    });

    const promiseData = firstValueFrom(observable);
    promiseData.then(value => successCallBack())
      .catch(error => errorCallBack(error));

    return await promiseData;
  }

  async getByIdOrder(id: string, successCallBack?: () => void, errorCallBack?: (errorMessage: string) => void) {
    const observable: Observable<SingleOrder> = this.httpClientService.get<SingleOrder>({
      controller: "orders",
    }, id);

    const promiseData = firstValueFrom(observable);
    promiseData.then(value => successCallBack())
      .catch(error => errorCallBack(error));

    return await promiseData;
  }
}
