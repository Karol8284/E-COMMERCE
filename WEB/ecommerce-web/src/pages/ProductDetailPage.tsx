/**
 * Product Detail Page Component
 * 
 * Shows detailed view of a single product:
 * - Product image
 * - Name, description, price
 * - Stock status
 * - Quantity selector (1 to stock max)
 * - Add to cart button
 * - Product metadata (category, ID, created date)
 * - Back to products link
 * 
 * Fetches product data by ID from URL parameter.
 * Uses React Query for caching and loading states.
 */

import { useParams, useNavigate } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { useState } from 'react';
import { productService } from '../services/productService';
import { useCartStore } from '../stores/cartStore';

/**
 * Product detail page
 * @component
 * @returns {JSX.Element} Full product information and add-to-cart
 */
export function ProductDetailPage() {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [quantity, setQuantity] = useState(1);
  const [addedToCart, setAddedToCart] = useState(false);
  const { addItem } = useCartStore();

  const { data: product, isLoading, error } = useQuery({
    queryKey: ['product', id],
    queryFn: () => (id ? productService.getById(id) : null),
    enabled: !!id,
  });

  if (isLoading) {
    return (
      <div className="flex justify-center items-center min-h-screen">
        <div className="text-center">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-600 mx-auto mb-4"></div>
          <p className="text-gray-600">Loading product...</p>
        </div>
      </div>
    );
  }

  if (error || !product) {
    return (
      <div className="text-center py-12">
        <p className="text-red-500 text-lg mb-4">Product not found</p>
        <button
          onClick={() => navigate('/')}
          className="bg-purple-600 text-white px-6 py-2 rounded-lg hover:bg-purple-700"
        >
          Back to Products
        </button>
      </div>
    );
  }

  const handleAddToCart = () => {
    addItem(product.id, quantity);
    setAddedToCart(true);
    setTimeout(() => setAddedToCart(false), 2000);
  };

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
      {/* Image */}
      <div>
        {product.imageUrl ? (
          <img
            src={product.imageUrl}
            alt={product.name}
            className="w-full rounded-lg shadow-lg"
          />
        ) : (
          <div className="w-full h-96 bg-gray-200 rounded-lg flex items-center justify-center">
            <span className="text-gray-400">No image available</span>
          </div>
        )}
      </div>

      {/* Info */}
      <div>
        <h1 className="text-4xl font-bold mb-4">{product.name}</h1>
        <p className="text-gray-600 text-lg mb-6">{product.description}</p>

        {/* Price & Stock */}
        <div className="mb-8 p-6 bg-gray-50 rounded-lg">
          <p className="text-5xl font-bold text-purple-600 mb-4">
            ${product.price.toFixed(2)}
          </p>
          <p
            className={`text-lg font-semibold ${
              product.stock > 0 ? 'text-green-600' : 'text-red-600'
            }`}
          >
            {product.stock > 0
              ? `${product.stock} in stock`
              : 'Out of stock'}
          </p>
        </div>

        {/* Quantity & Add to Cart */}
        <div className="flex gap-4 mb-8">
          <input
            type="number"
            min="1"
            max={product.stock}
            value={quantity}
            onChange={(e) => setQuantity(Math.max(1, parseInt(e.target.value) || 1))}
            className="w-24 px-4 py-3 border border-gray-300 rounded-lg text-center text-lg"
          />
          <button
            onClick={handleAddToCart}
            disabled={product.stock === 0}
            className={`flex-1 py-3 rounded-lg font-bold text-white text-lg transition ${
              product.stock === 0
                ? 'bg-gray-400 cursor-not-allowed'
                : 'bg-purple-600 hover:bg-purple-700'
            }`}
          >
            {addedToCart ? '✓ Added to Cart!' : 'Add to Cart'}
          </button>
        </div>

        {/* Details */}
        <div className="border-t pt-6 space-y-4">
          <div>
            <p className="text-sm text-gray-500">Category</p>
            <p className="text-lg font-semibold">{product.category}</p>
          </div>
          <div>
            <p className="text-sm text-gray-500">Created</p>
            <p className="text-lg">
              {new Date(product.createdAt).toLocaleDateString()}
            </p>
          </div>
          <div>
            <p className="text-sm text-gray-500">Product ID</p>
            <p className="text-sm font-mono text-gray-600">{product.id}</p>
          </div>
        </div>

        {/* Back Button */}
        <button
          onClick={() => navigate('/')}
          className="w-full mt-8 px-6 py-3 border border-purple-600 text-purple-600 rounded-lg hover:bg-purple-50 transition"
        >
          ← Back to Products
        </button>
      </div>
    </div>
  );
}
