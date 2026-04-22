import apiClient from '../api/client';

export interface Order {
  id: string;
  userId: string;
  orderNumber: string;
  items: Array<{
    productId: string;
    productName: string;
    quantity: number;
    price: number;
  }>;
  totalAmount: number;
  status: 'pending' | 'processing' | 'shipped' | 'delivered' | 'cancelled';
  shippingAddress: {
    firstName: string;
    lastName: string;
    address: string;
    city: string;
    zipCode: string;
  };
  createdAt: string;
  updatedAt: string;
}

export interface CreateOrderRequest {
  firstName: string;
  lastName: string;
  email: string;
  address: string;
  city: string;
  zipCode: string;
}

export interface OrdersResponse {
  items: Order[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

/**
 * Order Service - handles order-related API calls
 */
export const orderService = {
  /**
   * Utwórz nowe zamówienie
   */
  createOrder: async (data: CreateOrderRequest): Promise<Order> => {
    const response = await apiClient.post<Order>('/orders', data);
    return response.data;
  },

  /**
   * Pobierz wszystkie zamówienia użytkownika
   */
  getOrders: async (page: number = 1, pageSize: number = 10): Promise<OrdersResponse> => {
    const response = await apiClient.get<OrdersResponse>('/orders', {
      params: { page, pageSize },
    });
    return response.data;
  },

  /**
   * Pobierz szczegóły jednego zamówienia
   */
  getOrderById: async (id: string): Promise<Order> => {
    const response = await apiClient.get<Order>(`/orders/${id}`);
    return response.data;
  },

  /**
   * Anuluj zamówienie
   */
  cancelOrder: async (id: string): Promise<Order> => {
    const response = await apiClient.put<Order>(`/orders/${id}/cancel`, {});
    return response.data;
  },

  /**
   * Pobierz status zamówienia
   */
  getOrderStatus: async (id: string): Promise<string> => {
    const order = await orderService.getOrderById(id);
    return order.status;
  },
};
