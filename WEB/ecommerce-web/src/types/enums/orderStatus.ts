/**
 * Order status enum - represents the state of an order
 */
export const OrderStatus = {
  Pending: 'Pending',
  Shipped: 'Shipped',
  Delivered: 'Delivered',
  Cancelled: 'Cancelled'
} as const;

export type OrderStatus = typeof OrderStatus[keyof typeof OrderStatus];