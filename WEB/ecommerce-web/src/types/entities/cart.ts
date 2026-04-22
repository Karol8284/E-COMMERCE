import type { CartItem } from "../../services/cartService";
import type { User } from "./user";

export interface Cart {
    id: string;
    userId: string;
    user: User;
    createdAt: Date | string;
    updatedAt: Date | string;
    items?: CartItem[];
}