import { Component } from '@angular/core';

declare var $: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ECommerceClient';

  ngOnInit() {
    if (typeof window !== 'undefined') {
      $(document).ready(() => {
        alert("Hello World");
      });
    }
  }
}
