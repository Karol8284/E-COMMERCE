/**
 * API Client Configuration
 * 
 * Centralized Axios instance for all HTTP requests to the C# backend.
 * Automatically handles JWT authentication via interceptors.
 * 
 * Features:
 * - JWT token injection in request headers
 * - Automatic token refresh on 401 errors
 * - Error handling and logging
 * - Base URL configuration via VITE_API_BASE_URL env variable
 * 
 * Backend URL: http://localhost:5094/api (development)
 */

import type { AxiosInstance } from "axios";
import axios from "axios";
import { setupInterceptors } from "./interceptors";

/**
 * Axios instance configured for e-commerce API
 * @type {AxiosInstance}
 */
const apiClient: AxiosInstance = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL || "http://localhost:5094/api",
    timeout: 10000, //  = 10s :) Może potem to zmienie na inną wartość.
    headers: {
        'Content-Type': 'application/json',
    },
});

setupInterceptors(apiClient);

export default apiClient;