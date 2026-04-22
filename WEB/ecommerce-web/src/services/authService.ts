/**
 * Authentication Service
 * 
 * Handles all authentication-related API calls:
 * - User login/logout
 * - Token refresh
 * - Session management
 * 
 * All requests go through the API client which automatically
 * handles JWT token injection and error handling.
 */

import apiClient from "../api/client";

/**
 * User credentials for login
 * @interface LoginCredentials
 * @property {string} email - User email
 * @property {string} password - User password (plain text, sent via HTTPS only)
 */
export interface LoginCredentials {
    email: string;
    password: string;
}

export interface AuthResponse {
    accessToken: string;
    refreshToken: string;
    user: {
        id: string;
        email: string;
        role: string;
    }
}

export const authService = {
    login: async (credentials: LoginCredentials): Promise<AuthResponse> => {
        const response = await apiClient.post<AuthResponse>("/auth/login", credentials);
        const token = response.data.accessToken;
        localStorage.setItem("authToken", token);
        return response.data;
    },
    
    logout: () => {
        localStorage.removeItem("authToken");
        window.location.href = "/login";
    },

    refreshToken: async (): Promise<string> => {
        const refreshToken = localStorage.getItem("refreshToken");
        const response = await apiClient.post<{accessToken: string}>
        ("/auth/refresh", { refreshToken,});
        const newToken = response.data.accessToken;
        localStorage.setItem("authToken", newToken);
        return newToken;
    }
}        

// export interface RegisterCredentials {
//     email: string;
//     password: string;
//     confirmPassword: string;
// }