import {Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2} from '@angular/core';
import {ProductService} from '../../services/common/models/product.service';
import {NgxSpinnerService} from 'ngx-spinner';
import {BaseComponent, SpinnerType} from '../../base/base.component';

declare var $: any;

@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(private element: ElementRef, private _renderer: Renderer2, private productService: ProductService, private spinner: NgxSpinnerService) {
    const img = _renderer.createElement("img");
    img.setAttribute("src", "assets/icons/delete.png");
    img.setAttribute("width", "24");
    img.setAttribute("height", "24");
    img.setAttribute("style", "cursor: pointer");
    _renderer.appendChild(element.nativeElement, img);
  }

  @Input() id: string;
  @Output() callback: EventEmitter<any> = new EventEmitter();

  @HostListener('click')
  async onClick() {
    this.spinner.show(SpinnerType.BallAtom);
    const td: HTMLTableCellElement = this.element.nativeElement;
    await this.productService.delete(this.id);
    $(td.parentElement).fadeOut(2000, () => {
      this.callback.emit();
    });
  }
}
