import type { User } from "../../stores/authStore";

export interface Order{
    id: string;
    userId: string;
    user: User;
    status: string; // OrderStatus enum
    totalPrice: number; //decimal
    createdAt: Date | string;
    deliveryDate?: Date | string | null;
}