import {Component, OnInit} from '@angular/core';
import {BaseComponent, SpinnerType} from '../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {BasketService} from '../../../services/common/models/basket.service';
import {ListBasketItem} from '../../../contracts/basket/list-basket-item';
import {UpdateBasketItem} from '../../../contracts/basket/update-basket-item';
import {OrderService} from '../../../services/common/models/order.service';
import {CreateOrder} from '../../../contracts/order/create-order';
import {CustomToastrService, ToastrMessageType, ToastrPosition} from '../../../services/ui/custom-toastr.service';
import {Router} from '@angular/router';
import {DialogService} from '../../../services/common/dialog.service';
import {
  BasketItemDeleteState,
  BasketItemRemoveDialogComponent
} from '../../../dialogs/basket-item-remove-dialog/basket-item-remove-dialog.component';
import {ShoppingCompleteDialogComponent, ShoppingCompleteState} from '../../../dialogs/shopping-complete-dialog/shopping-complete-dialog.component';

declare var $: any;

@Component({
  selector: 'app-baskets',
  templateUrl: './baskets.component.html',
  styleUrl: './baskets.component.scss'
})
export class BasketsComponent extends BaseComponent implements OnInit {
  constructor(spinner: NgxSpinnerService,
              private basketService: BasketService,
              private orderService: OrderService,
              private toastrService: CustomToastrService,
              private router: Router,
              private dialogService: DialogService) {
    super(spinner);
  }

  basketItems: ListBasketItem[];

  async ngOnInit(): Promise<void> {
    this.showSpinner(SpinnerType.BallAtom);
    this.basketItems = await this.basketService.get();
    this.hideSpinner(SpinnerType.BallAtom);
  }

  async changeQuantity(object: any) {
    this.showSpinner(SpinnerType.BallAtom);
    const basketItemId: string = object.target.attributes["id"].value;
    const quantity: number = object.target.value;
    const basketItem: UpdateBasketItem = new UpdateBasketItem();
    basketItem.basketItemId = basketItemId;
    basketItem.quantity = quantity;
    await this.basketService.updateQuantity(basketItem)
    this.hideSpinner(SpinnerType.BallAtom);
  }

  removeBasketItem(basketItemId: string) {
    $("#basketModal").modal("hide");
    this.dialogService.openDialog({
      componentType: BasketItemRemoveDialogComponent,
      data: BasketItemDeleteState.Yes,
      afterClosed: async () => {
        this.showSpinner(SpinnerType.BallAtom);
        await this.basketService.remove(basketItemId);
        $("." + basketItemId).fadeOut(500, () => this.hideSpinner(SpinnerType.BallAtom));
        $("#basketModal").modal("show");
    }
    });
  }

  shoppingComplete() {
    $("#basketModal").modal("hide");
    this.dialogService.openDialog({
      componentType: ShoppingCompleteDialogComponent,
      data: ShoppingCompleteState.Yes,
      afterClosed: async () => {
        this.showSpinner(SpinnerType.BallAtom);
        const order: CreateOrder = new CreateOrder();
        order.address = "Arizona";
        order.description = "This is a test order";
        await this.orderService.create(order);
        this.hideSpinner(SpinnerType.BallAtom);
        this.toastrService.message("Order is created", "success", {
          messageType: ToastrMessageType.Info,
          position: ToastrPosition.TopRight
        });
        this.router.navigate(["/"]);
      }
    });
  }
}
