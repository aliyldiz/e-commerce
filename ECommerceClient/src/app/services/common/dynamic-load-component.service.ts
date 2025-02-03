import {ComponentFactory, ComponentFactoryResolver, Injectable, ViewContainerRef} from '@angular/core';
import {BaseComponent} from '../../base/base.component';

@Injectable({
  providedIn: 'root'
})
export class DynamicLoadComponentService {

  // ViewContainerRef => dinamik olarak yüklenecek component'i içersinde barındıran container'dır. Her dinamil yükleme sürecinde önceki view'leri clear etmek gerekiyor.
  // ComponentFactory => component'lerin instance'larını oluşturmak için kullanılan nesnedir.
  // ComponentFactoryResolver => belirli bir component için ComponentFactory'i resolve eden sınıftır. İçersindeki resolveComponentFactory fonksiyonu aracılığıyla ilgili component'e dair bir ComponentFactory nesnesi oluşturup, döner.

  constructor() { }

  async loadComponent(component: ComponentType, viewContainerRef: ViewContainerRef) {
    let _component: any = null;

    switch (component) {
      case ComponentType.BasketsComponent:
        _component = (await import("../../ui/components/baskets/baskets.component")).BasketsComponent;
        break;
    }

    viewContainerRef.clear();
    return viewContainerRef.createComponent(_component);
  }
}


export enum ComponentType {
  BasketsComponent
}
