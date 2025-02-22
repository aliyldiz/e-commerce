import {Component, OnInit, ViewChild} from '@angular/core';
import {BaseComponent, SpinnerType} from '../../../../base/base.component';
import {NgxSpinnerService} from 'ngx-spinner';
import {AlertifyService, MessageType, Position} from '../../../../services/admin/alertify.service';
import {DialogService} from '../../../../services/common/dialog.service';
import {MatTableDataSource} from '@angular/material/table';
import {MatPaginator} from '@angular/material/paginator';
import {ListUser} from '../../../../contracts/users/list-user';
import {UserService} from '../../../../services/common/models/user.service';
import {AuthorizeUserDialogComponent} from '../../../../dialogs/authorize-user-dialog/authorize-user-dialog.component';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrl: './list.component.scss'
})
export class ListComponent extends BaseComponent implements OnInit {

  constructor(spinner: NgxSpinnerService,
              private userService: UserService,
              private alertifyService: AlertifyService,
              private dialogService: DialogService) {
    super(spinner);
  }

  displayedColumns: string[] = ['userName', 'fullName', 'email', 'twoFactorEnabled', 'role', 'delete'];
  dataSource: MatTableDataSource<ListUser> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;

  async getUsers() {
    this.showSpinner(SpinnerType.BallAtom);
    const allUsers: { totalUsersCount: number, users: ListUser[] } =
      await this.userService.getAllUsers(this.paginator ? this.paginator.pageIndex : 0,
        this.paginator ? this.paginator.pageSize : 5,
        () => this.hideSpinner(SpinnerType.BallAtom),
        errorMessage => this.alertifyService.message(errorMessage, {
          dismissOthers: true,
          messageType: MessageType.Error,
          position: Position.BottomRight
        }));

    this.dataSource = new MatTableDataSource<ListUser>(allUsers.users);
    this.paginator.length = allUsers.totalUsersCount;
  }


  async pageChanged() {
    await this.getUsers();
  }

  async ngOnInit() {
    await this.getUsers()
  }

  assignRole(id: string) {
    this.dialogService.openDialog({
      componentType: AuthorizeUserDialogComponent,
      data: id,
      options: {
        width: "750px"
      },
      afterClosed: () => {
        this.alertifyService.message("Role assigned successfully", {
          messageType: MessageType.Success,
          position: Position.TopRight
        })
      }
    });
  }
}
