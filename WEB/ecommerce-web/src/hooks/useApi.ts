/**
 * Custom Hook: useApi
 * 
 * Generic hook for making API calls with built-in loading/error handling.
 * Simplifies async operations and provides consistent error handling.
 * 
 * Usage:
 *   const { data, loading, error, call } = useApi<ProductsResponse>();
 *   
 *   const handleFetch = async () => {
 *     await call(() => productService.getAll());
 *   };
 * 
 * Features:
 * - Type-safe API responses
 * - Automatic loading state management
 * - Unified error handling
 * - AxiosError wrapping for API errors
 */

import { useCallback, useState } from 'react';
import { AxiosError } from 'axios';
import type { ApiErrorResponse } from '../api/types';

/**
 * State structure for API calls
 * @interface UseApiState
 * @template T - Type of successful response data
 * @property {T | null} data - Successful response data
 * @property {boolean} loading - Whether API call is in progress
 * @property {ApiErrorResponse | null} error - Error details if call failed
 */
interface UseApiState<T> {
  data: T | null;
  loading: boolean;
  error: ApiErrorResponse | null;
}

/**
 * Custom hook do obsługi API calls z loading i error states
 * 
 * @example
 * const { data, loading, error, call } = useApi()
 * const handleFetch = async () => {
 *   const result = await call(() => productService.getAll())
 * }
 */
export function useApi<T = unknown>() {
  const [state, setState] = useState<UseApiState<T>>({
    data: null,
    loading: false,
    error: null,
  });

  const call = useCallback(async (apiFunction: () => Promise<T>) => {
    setState({ data: null, loading: true, error: null });

    try {
      const result = await apiFunction();
      setState({ data: result, loading: false, error: null });
      return result;
    } catch (err: unknown) {
      let error: ApiErrorResponse = {
        message: 'An unexpected error occurred',
        statusCode: 500,
      };

      if (err instanceof AxiosError) {
        error = (err.response?.data as ApiErrorResponse) || error;
      } else if (err instanceof Error) {
        error.message = err.message;
      }

      setState({ data: null, loading: false, error });
      throw error;
    }
  }, []);

  return {
    ...state,
    call,
  };
}
