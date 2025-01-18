import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { FooterComponent } from './footer/footer.component';
import {ProductsModule} from '../../components/products/products.module';
import {OrdersModule} from '../../components/orders/orders.module';
import {CustomersModule} from '../../components/customers/customers.module';
import {DashboardComponent} from '../../components/dashboard/dashboard.component';
import {DashboardModule} from '../../components/dashboard/dashboard.module';
import {RouterLink, RouterModule} from '@angular/router';



@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    FooterComponent
  ],
  imports: [
    CommonModule,
    ProductsModule,
    OrdersModule,
    CustomersModule,
    DashboardModule,
    RouterLink,
    RouterModule
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,
    FooterComponent
  ]
})
export class ComponentsModule { }
