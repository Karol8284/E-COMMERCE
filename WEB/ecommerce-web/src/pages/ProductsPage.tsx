/**
 * Products Listing Page
 * 
 * Displays all products with:
 * - Pagination (10 products per page)
 * - Category filtering
 * - Product grid layout (responsive)
 * - Loading and error states
 * - Links to product detail pages
 * 
 * Uses React Query for automatic data fetching and caching.
 * Cache is automatically invalidated when page/category changes.
 */

import { useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { Link } from 'react-router-dom';
import { productService } from '../services/productService';

/**
 * Main products listing page
 * @component
 * @returns {JSX.Element} Product grid with pagination
 */
export function ProductsPage() {
  const [page, setPage] = useState(1);
  const [category, setCategory] = useState('');

  const { data, isLoading, error } = useQuery({
    queryKey: ['products', page, category],
    queryFn: () =>
      productService.getAll(page, 10, category || undefined),
  });

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading products...</p>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="text-center py-12">
        <p className="text-red-500 text-lg">Error loading products</p>
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-4xl font-bold mb-8">Products</h1>

      {/* Filter */}
      <div className="mb-8 p-4 bg-white rounded-lg shadow">
        <input
          type="text"
          placeholder="Filter by category..."
          value={category}
          onChange={(e) => {
            setCategory(e.target.value);
            setPage(1);
          }}
          className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500"
        />
      </div>

      {/* Products Grid */}
      {data?.items && data.items.length > 0 ? (
        <>
          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            {data.items.map((product) => (
              <Link
                key={product.id}
                to={`/products/${product.id}`}
                className="bg-white rounded-lg shadow hover:shadow-xl transition overflow-hidden group"
              >
                {product.imageUrl && (
                  <div className="h-48 overflow-hidden bg-gray-100">
                    <img
                      src={product.imageUrl}
                      alt={product.name}
                      className="w-full h-full object-cover group-hover:scale-110 transition"
                    />
                  </div>
                )}
                <div className="p-4">
                  <h2 className="text-lg font-bold text-gray-800 group-hover:text-purple-600">
                    {product.name}
                  </h2>
                  <p className="text-gray-600 text-sm mb-3 line-clamp-2">
                    {product.description}
                  </p>
                  <div className="flex justify-between items-center">
                    <p className="text-2xl font-bold text-purple-600">
                      ${product.price.toFixed(2)}
                    </p>
                    <p
                      className={`text-sm font-semibold ${
                        product.stock > 0 ? 'text-green-600' : 'text-red-600'
                      }`}
                    >
                      {product.stock > 0 ? `${product.stock} left` : 'Out of stock'}
                    </p>
                  </div>
                  <p className="text-xs text-gray-500 mt-2">{product.category}</p>
                </div>
              </Link>
            ))}
          </div>

          {/* Pagination */}
          <div className="flex gap-4 justify-center items-center py-8">
            <button
              onClick={() => setPage(Math.max(1, page - 1))}
              disabled={page === 1}
              className="px-6 py-2 bg-purple-600 text-white rounded-lg disabled:opacity-50 hover:bg-purple-700 transition"
            >
              ← Previous
            </button>
            <span className="text-gray-600 font-semibold">Page {page}</span>
            <button
              onClick={() => setPage(page + 1)}
              disabled={!data || data.items.length < 10}
              className="px-6 py-2 bg-purple-600 text-white rounded-lg disabled:opacity-50 hover:bg-purple-700 transition"
            >
              Next →
            </button>
          </div>
        </>
      ) : (
        <div className="text-center py-12">
          <p className="text-gray-600 text-lg">No products found</p>
        </div>
      )}
    </div>
  );
}
