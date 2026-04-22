/**
 * Global API Response Types
 * 
 * These interfaces define the structure of responses from the C# backend.
 * Used for type-safe error handling and response processing.
 */

/**
 * Standard error response from the API
 * @interface ApiErrorResponse
 * @property {number} statusCode - HTTP status code (e.g., 400, 401, 500)
 * @property {string} message - User-friendly error message
 * @property {string} [error] - Technical error details (optional)
 */
export interface ApiErrorResponse {
    statusCode: number;
    message: string;
    error?: string;
}

export interface ApiResponse<T> {
    statusCode: number;
    data: T;
    message: string;
}