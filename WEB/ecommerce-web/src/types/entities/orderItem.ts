import type { Order } from "../../services/orderService";
import type { Product } from "../../services/productService";

export interface OrderItem {
    id: string;
    orderId: string;
    productId: string;
    quantity: number;
    price: number; //decimal
    order?: Order;
    product?: Product;
}