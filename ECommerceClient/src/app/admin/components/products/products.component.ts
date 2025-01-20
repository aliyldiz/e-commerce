import {Component, OnInit} from '@angular/core';
import {BaseComponent, SpinnerType} from '../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {HttpClientService} from '../../../services/common/http-client.service';
import {Product} from '../../../contracts/product';

@Component({
  selector: 'app-admin-products',
  templateUrl: './products.component.html',
  styleUrl: './products.component.scss'
})
export class ProductsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService, private httpClient: HttpClientService) {
    super(spinner);
  }

  ngOnInit() {
    this.showSpinner(SpinnerType.BallAtom);

    this.httpClient.get<Product[]>({
      controller: 'products'
    }).subscribe(data => console.log(data));

    // this.httpClient.post({
    //   controller: 'products'
    // }, {
    //   name: 'product 1',
    //   stock: 10,
    //   price: 100
    // }).subscribe(data => console.log(data));
    //
    // this.httpClient.put({
    //   controller: 'products'
    // }, {
    //   id: '979061e0-5c53-4f54-8ae6-0b5947320c5d',
    //   name: 'product 3',
    //   stock: 20,
    //   price: 200
    // }).subscribe(data => console.log(data));
    //
    // this.httpClient.delete({
    //   controller: 'products'
    // }, "acc6a7ab-9f8a-4a9d-8362-fb4d665c2c1d").subscribe();
    //
    // this.httpClient.get({
    //   fullEndPoint: "https://jsonplaceholder.typicode.com/posts"
    // }).subscribe(data => console.log(data));
  }
}
