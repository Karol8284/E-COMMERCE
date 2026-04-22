/**
 * Shopping Cart Store (Zustand)
 * 
 * Global state management for shopping cart.
 * Stores selected products and quantities (in-memory, not persisted).
 * 
 * Usage:
 *   const { items, addItem, removeItem, clear } = useCartStore();
 * 
 * Note: Cart is NOT synced with localStorage.
 * Consider adding persistence for production.
 */

import { create } from 'zustand';

/**
 * Single item in the shopping cart
 * @interface CartItem
 * @property {string} productId - Reference to product ID
 * @property {number} quantity - Number of this product in cart
 */
export interface CartItem {
  productId: string;
  quantity: number;
}

interface CartState {
  items: CartItem[];
  addItem(productId: string, quantity: number): void;
  removeItem(productId: string): void;
  updateQuantity(productId: string, quantity: number): void;
  clear(): void;
}

export const useCartStore = create<CartState>((set) => ({
  items: [],

  addItem: (productId, quantity) =>
    set((state) => {
      const existing = state.items.find((item) => item.productId === productId);
      if (existing) {
        return {
          items: state.items.map((item) =>
            item.productId === productId
              ? { ...item, quantity: item.quantity + quantity }
              : item
          ),
        };
      }
      return { items: [...state.items, { productId, quantity }] };
    }),

  removeItem: (productId) =>
    set((state) => ({
      items: state.items.filter((item) => item.productId !== productId),
    })),

  updateQuantity: (productId, quantity) =>
    set((state) => ({
      items: state.items.map((item) =>
        item.productId === productId ? { ...item, quantity } : item
      ),
    })),

  clear: () => set({ items: [] }),
}));
