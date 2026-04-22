import type { CartItem } from "../../services/cartService";
import type { Company } from "./company";
import type { OrderItem } from "./orderItem";

export interface Product {
    id: string;
    name: string;
    description: string;
    price: number; //decimal
    stock: number;
    imageUrl: string;
    category: string;
    companyId: string;
    company?: Company;
    createdAt: Date | string;
    cartItem?: CartItem[];
    orderItems?: OrderItem[];    
}