import type { Product } from "./product";

export interface CartItem {
    id: string;
    cartId: string;
    quantity: number;
    cart?: Cart;
    product?: Product
}