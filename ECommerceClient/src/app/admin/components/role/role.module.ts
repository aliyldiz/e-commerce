import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RoleComponent } from './role.component';
import {RouterModule} from '@angular/router';
import {ProductsComponent} from '../products/products.component';
import {MatDrawer, MatDrawerContainer, MatDrawerContent, MatSidenavModule} from '@angular/material/sidenav';
import {ProductsModule} from '../products/products.module';
import {MatFormFieldModule} from '@angular/material/form-field';
import {MatInputModule} from '@angular/material/input';
import {MatButtonModule} from '@angular/material/button';
import {MatTableModule} from '@angular/material/table';
import {MatPaginatorModule} from '@angular/material/paginator';
import { CreateComponent } from './create/create.component';
import { ListComponent } from './list/list.component';
import {DeleteDirectiveModule} from '../../../directives/admin/delete.directive.module';



@NgModule({
  declarations: [
    RoleComponent,
    CreateComponent,
    ListComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
      {path: '', component: RoleComponent}
    ]),
    MatDrawer,
    MatDrawerContainer,
    MatDrawerContent,
    MatSidenavModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatPaginatorModule,
    DeleteDirectiveModule,
  ]
})
export class RoleModule { }
