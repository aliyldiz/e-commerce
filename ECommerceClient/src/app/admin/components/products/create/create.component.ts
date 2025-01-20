import {Component, EventEmitter, Output} from '@angular/core';
import {Create_Product} from '../../../../contracts/create-product';
import {ProductService} from '../../../../services/common/models/product.service';
import {BaseComponent, SpinnerType} from '../../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {AlertifyService, MessageType, Position} from '../../../../services/admin/alertify.service';

@Component({
  selector: 'app-create',
  templateUrl: './create.component.html',
  styleUrl: './create.component.scss'
})
export class CreateComponent extends BaseComponent {
  constructor(spinner: NgxSpinnerService, private productService: ProductService, private alertify: AlertifyService ) {
    super(spinner);
  }

  @Output() createdProduct: EventEmitter<Create_Product> = new EventEmitter();

  create(name: HTMLInputElement, stock: HTMLInputElement, price: HTMLInputElement) {
    this.showSpinner(SpinnerType.BallAtom);
    const createProduct: Create_Product = new Create_Product();
    createProduct.name = name.value;
    createProduct.stock = parseInt(stock.value);
    createProduct.price = parseFloat(price.value);

    this.productService.create(createProduct, () => {
      this.hideSpinner(SpinnerType.BallAtom);
      this.alertify.message("Product created successfully", {
        dismissOthers: true,
        messageType: MessageType.Success,
        position: Position.TopRight
      });
      this.createdProduct.emit(createProduct);
    }, errorMessage => {
      this.alertify.message(errorMessage, {
        dismissOthers: true,
        messageType: MessageType.Error,
        position: Position.TopRight
      });
    });
  }
}
