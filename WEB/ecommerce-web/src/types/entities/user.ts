import type { Role } from '../enums';
import type { Cart } from './cart';
import type { Order } from './order';

/**
 * User model - represents an application user with authentication and profile information
 * Aggregate root for user account management and order/cart tracking
 * NOTE: passwordHash is NEVER sent to frontend for security reasons
 */
export interface User {
  /** Unique identifier for the user (primary key) */
  id: string;
  
  /** User's email address; used for login and communication (must be unique) */
  email: string;
  
  /** User's display name shown in the application (e.g., "John Doe") */
  displayName: string;
  
  /** Optional URL to user's profile avatar/profile picture */
  avatarUrl?: string | null;
  
  /** User's role determining permissions (Admin, User) */
  role: Role;
  
  /** Indicates whether the user account is active or deactivated */
  isActive: boolean;
  
  /** Indicates whether the user's email address has been verified */
  isEmailConfirmed: boolean;
  
  /** Timestamp when the user account was created in UTC */
  createdAt: Date | string;
  
  /** Collection of carts for this user */
  carts?: Cart[];
  
  /** Collection of orders for this user */
  orders?: Order[];
}