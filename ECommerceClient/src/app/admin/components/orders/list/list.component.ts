import {Component, OnInit, ViewChild} from '@angular/core';
import {BaseComponent, SpinnerType} from '../../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {AlertifyService, MessageType, Position} from '../../../../services/admin/alertify.service';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {ListOrder} from '../../../../contracts/order/list-order';
import {OrderService} from '../../../../services/common/models/order.service';
import {DialogService} from '../../../../services/common/dialog.service';
import {OrderDetailDialogComponent,} from '../../../../dialogs/order-detail-dialog/order-detail-dialog.component';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService,
              private orderService: OrderService,
              private alertifyService: AlertifyService,
              private dialogService: DialogService) {
    super(spinner);
  }

  displayedColumns: string[] = ['orderCode', 'userName', 'totalPrice', 'createdDate', 'viewDetail', 'delete'];
  dataSource: MatTableDataSource<ListOrder> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  async getOrders() {
    this.showSpinner(SpinnerType.BallAtom);
    const allOrders: { totalOrderCount: number, orders: ListOrder[] } =
      await this.orderService.getAllOrder(this.paginator ? this.paginator.pageIndex : 0,
        this.paginator ? this.paginator.pageSize : 5,
        () => this.hideSpinner(SpinnerType.BallAtom),
        errorMessage => this.alertifyService.message(errorMessage, {
          dismissOthers: true,
          messageType: MessageType.Error,
          position: Position.BottomRight
        }));

    this.dataSource = new MatTableDataSource<ListOrder>(allOrders.orders);
    this.paginator.length = allOrders.totalOrderCount;
  }


  async pageChanged() {
    await this.getOrders();
  }

  async ngOnInit() {
    await this.getOrders()
  }

  showDetail(id: string) {
    this.dialogService.openDialog({
      componentType: OrderDetailDialogComponent,
      data: id,
      options: {
        width: '50%',
        height: '50%'
      }
    })
  }
}
