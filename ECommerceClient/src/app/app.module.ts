import { NgModule } from '@angular/core';
import { BrowserModule, provideClientHydration } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {AdminModule} from './admin/admin.module';
import {UiModule} from './ui/ui.module';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import {NgxSpinnerModule} from 'ngx-spinner';
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AdminModule, UiModule,
    ToastrModule.forRoot({
      timeOut: 5000,
    }),
    NgxSpinnerModule,
    HttpClientModule
  ],
  providers: [
    { provide: "baseUrl", useValue: "https://localhost:7092/api", multi: true},
    provideClientHydration(),
    provideAnimationsAsync()
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
