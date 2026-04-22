import apiClient from '../api/client';

/**
 * Product interface - matches C# CORE.Entities.Product
 * Represents a product available in the e-commerce store
 */
export interface Product {
  /** Unique identifier for the product */
  id: string;

  /** Product name displayed in the store */
  name: string;

  /** Detailed product description */
  description: string;

  /** Product price */
  price: number;

  /** Number of items available in stock */
  stock: number;

  /** URL to product image */
  imageUrl?: string;

  /** Product category (e.g., Electronics, Books, Clothing) */
  category: string;

  /** Foreign key to Company */
  companyId: string;

  /** Timestamp when the product was created */
  createdAt: string;
}

/**
 * Create Product DTO - for creating new products
 */
export interface CreateProductRequest {
  name: string;
  description: string;
  price: number;
  stock: number;
  imageUrl?: string;
  category: string;
  companyId: string;
}

/**
 * Update Product DTO - for updating existing products
 */
export interface UpdateProductRequest {
  name?: string;
  description?: string;
  price?: number;
  stock?: number;
  imageUrl?: string;
  category?: string;
}

/**
 * Paginated Products Response
 */
export interface ProductsResponse {
  items: Product[];
  totalCount: number;
  pageNumber: number;
  pageSize: number;
}

/**
 * Product Service - handles all product-related API calls
 */
export const productService = {
  /**
   * Pobierz wszystkie produkty z paginacją
   */
  getAll: async (
    page: number = 1,
    pageSize: number = 10,
    category?: string,
    sortBy?: 'price' | 'name' | 'createdAt'
  ): Promise<ProductsResponse> => {
    const response = await apiClient.get<ProductsResponse>('/products', {
      params: {
        page,
        pageSize,
        ...(category && { category }),
        ...(sortBy && { sortBy }),
      },
    });
    return response.data;
  },

  /**
   * Pobierz jeden produkt po ID
   */
  getById: async (id: string): Promise<Product> => {
    const response = await apiClient.get<Product>(`/products/${id}`);
    return response.data;
  },

  /**
   * Pobierz produkty z kategorii
   */
  getByCategory: async (category: string, pageSize: number = 10): Promise<ProductsResponse> => {
    const response = await apiClient.get<ProductsResponse>('/products', {
      params: { category, pageSize },
    });
    return response.data;
  },

  /**
   * Utwórz nowy produkt (tylko admin)
   */
  create: async (product: CreateProductRequest): Promise<Product> => {
    const response = await apiClient.post<Product>('/products', product);
    return response.data;
  },

  /**
   * Zaktualizuj produkt (tylko admin/właściciel)
   */
  update: async (id: string, product: UpdateProductRequest): Promise<Product> => {
    const response = await apiClient.put<Product>(`/products/${id}`, product);
    return response.data;
  },

  /**
   * Usuń produkt (tylko admin)
   */
  delete: async (id: string): Promise<void> => {
    await apiClient.delete(`/products/${id}`);
  },

  /**
   * Wyszukaj produkty po nazwie
   */
  search: async (query: string, pageSize: number = 10): Promise<ProductsResponse> => {
    const response = await apiClient.get<ProductsResponse>('/products/search', {
      params: { query, pageSize },
    });
    return response.data;
  },

  /**
   * Aktualizuj stock produktu (zmniejsz ilość)
   */
  decreaseStock: async (id: string, quantity: number): Promise<Product> => {
    const response = await apiClient.patch<Product>(`/products/${id}/stock`, {
      quantity: -quantity,
    });
    return response.data;
  },

  /**
   * Aktualizuj stock produktu (zwiększ ilość)
   */
  increaseStock: async (id: string, quantity: number): Promise<Product> => {
    const response = await apiClient.patch<Product>(`/products/${id}/stock`, {
      quantity,
    });
    return response.data;
  },
};