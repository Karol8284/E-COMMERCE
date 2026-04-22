/**
 * Authentication Store (Zustand)
 * 
 * Global state management for user authentication.
 * Handles login, logout, and token persistence in localStorage.
 * 
 * Usage:
 *   const { user, isLoggedIn, login, logout } = useAuthStore();
 * 
 * Token Storage:
 *   - authToken: Stored in localStorage for session persistence
 *   - refreshToken: Used for token refresh (stored separately)
 */

import { create } from 'zustand';

/**
 * User data structure
 * @interface User
 * @property {string} id - Unique user identifier from backend
 * @property {string} email - User email address
 * @property {string} role - User role (e.g., 'user', 'admin')
 */
export interface User {
  id: string;
  email: string;
  role: string;
}

interface AuthState {
  user: User | null;
  token: string | null;
  isLoggedIn: boolean;

  // Actions
  setUser(user: User): void;
  setToken(token: string): void;
  logout(): void;
  login(user: User, token: string): void;
}

export const useAuthStore = create<AuthState>((set) => ({
  user: null,
  token: typeof window !== 'undefined' ? localStorage.getItem('authToken') : null,
  isLoggedIn: typeof window !== 'undefined' ? !!localStorage.getItem('authToken') : false,

  setUser: (user) => set({ user }),

  setToken: (token) => {
    if (typeof window !== 'undefined') {
      localStorage.setItem('authToken', token);
    }
    set({ token, isLoggedIn: true });
  },

  logout: () => {
    if (typeof window !== 'undefined') {
      localStorage.removeItem('authToken');
      localStorage.removeItem('refreshToken');
    }
    set({ user: null, token: null, isLoggedIn: false });
  },

  login: (user, token) => {
    if (typeof window !== 'undefined') {
      localStorage.setItem('authToken', token);
    }
    set({ user, token, isLoggedIn: true });
  },
}));
