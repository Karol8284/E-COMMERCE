import apiClient from '../api/client';

export interface CartItem {
  productId: string;
  quantity: number;
}

export interface CartResponse {
  id: string;
  userId: string;
  items: CartItem[];
  totalPrice: number;
  createdAt: string;
  updatedAt: string;
}

export interface AddToCartRequest {
  productId: string;
  quantity: number;
}

/**
 * Cart Service - handles shopping cart API calls
 */
export const cartService = {
  /**
   * Pobierz koszyk użytkownika
   */
  getCart: async (): Promise<CartResponse> => {
    const response = await apiClient.get<CartResponse>('/cart');
    return response.data;
  },

  /**
   * Dodaj produkt do koszyka
   */
  addToCart: async (productId: string, quantity: number): Promise<CartResponse> => {
    const response = await apiClient.post<CartResponse>('/cart/items', {
      productId,
      quantity,
    });
    return response.data;
  },

  /**
   * Usuń produkt z koszyka
   */
  removeFromCart: async (productId: string): Promise<CartResponse> => {
    const response = await apiClient.delete<CartResponse>(`/cart/items/${productId}`);
    return response.data;
  },

  /**
   * Aktualizuj ilość produktu w koszyku
   */
  updateCartItem: async (productId: string, quantity: number): Promise<CartResponse> => {
    const response = await apiClient.put<CartResponse>(`/cart/items/${productId}`, {
      quantity,
    });
    return response.data;
  },

  /**
   * Wyczyść koszyk
   */
  clearCart: async (): Promise<void> => {
    await apiClient.delete('/cart');
  },

  /**
   * Pobierz ilość elementów w koszyku
   */
  getCartCount: async (): Promise<number> => {
    const cart = await cartService.getCart();
    return cart.items.reduce((sum, item) => sum + item.quantity, 0);
  },
};
