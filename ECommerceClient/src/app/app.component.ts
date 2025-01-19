import {Component} from '@angular/core';

declare var $: any

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ECommerceClient';

  constructor() { }

  ngOnInit() {
    $.get("https://localhost:7092/api/Products", data => {
      console.log(data)
    });
  }
}
