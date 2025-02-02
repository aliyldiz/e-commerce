import {List_Product_Image} from './list-product-image';

export class List_Product {
    id: string;
    name: string;
    stock: number;
    price: number;
    createdDate: Date;
    modifiedDate: Date;
    productImageFiles?: List_Product_Image[];
    imagePath: string;
}
