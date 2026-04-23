import type { Product } from "./product";
import type { Cart } from "./cart";

export interface CartItem {
    id: string;
    cartId: string;
    quantity: number;
    cart?: Cart;
    product?: Product
}