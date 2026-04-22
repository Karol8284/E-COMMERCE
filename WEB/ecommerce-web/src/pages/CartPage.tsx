/**
 * Shopping Cart Page
 * 
 * Displays selected products with:
 * - Product details (image, name, price)
 * - Quantity adjustment (+/- buttons)
 * - Remove item option
 * - Price calculation (subtotal, tax 10%, total)
 * - Proceed to checkout button
 * - Continue shopping link
 * - Empty cart message
 * 
 * Fetches full product details to show images and prices.
 * Uses local cart store for state management.
 */

import { Link } from 'react-router-dom';
import { useQuery } from '@tanstack/react-query';
import { useCartStore } from '../stores/cartStore';
import { productService, type Product } from '../services/productService';

/**
 * Shopping cart page
 * @component
 * @returns {JSX.Element} Cart items with totals and checkout button
 */
export function CartPage() {
  const { items, removeItem, updateQuantity, clear } = useCartStore();

  // Fetch all products to get details
  const { data: allProducts } = useQuery({
    queryKey: ['all-products'],
    queryFn: async () => {
      let allItems: Product[] = [];
      let page = 1;
      while (true) {
        const response = await productService.getAll(page, 100);
        allItems = [...allItems, ...response.items];
        if (response.items.length < 100) break;
        page++;
      }
      return allItems;
    },
  });

  // Get cart items details
  const cartItems = items
    .map((item) => ({
      ...item,
      product: allProducts?.find((p) => p.id === item.productId),
    }))
    .filter((item) => item.product);

  const subtotal = cartItems.reduce(
    (sum, item) => sum + (item.product?.price || 0) * item.quantity,
    0
  );
  const tax = subtotal * 0.1; // 10% tax
  const total = subtotal + tax;

  if (items.length === 0) {
    return (
      <div className="text-center py-12">
        <h1 className="text-3xl font-bold mb-6">Shopping Cart</h1>
        <p className="text-gray-600 text-lg mb-8">Your cart is empty</p>
        <Link
          to="/"
          className="inline-block bg-purple-600 text-white px-8 py-3 rounded-lg hover:bg-purple-700"
        >
          Continue Shopping
        </Link>
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-4xl font-bold mb-8">Shopping Cart</h1>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Cart Items */}
        <div className="lg:col-span-2">
          <div className="bg-white rounded-lg shadow">
            {cartItems.map((item) => (
              <div
                key={item.productId}
                className="flex gap-4 p-6 border-b hover:bg-gray-50 transition"
              >
                {/* Product Image */}
                {item.product?.imageUrl && (
                  <img
                    src={item.product.imageUrl}
                    alt={item.product.name}
                    className="w-24 h-24 object-cover rounded"
                  />
                )}

                {/* Product Info */}
                <div className="flex-1">
                  <Link
                    to={`/products/${item.productId}`}
                    className="text-lg font-bold hover:text-purple-600 transition"
                  >
                    {item.product?.name}
                  </Link>
                  <p className="text-gray-600 text-sm mb-2">
                    {item.product?.description.substring(0, 50)}...
                  </p>
                  <p className="text-xl font-bold text-purple-600">
                    ${(item.product?.price || 0).toFixed(2)}
                  </p>
                </div>

                {/* Quantity & Actions */}
                <div className="flex flex-col gap-4 items-end">
                  <div className="flex gap-2">
                    <button
                      onClick={() =>
                        updateQuantity(
                          item.productId,
                          Math.max(1, item.quantity - 1)
                        )
                      }
                      className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
                    >
                      −
                    </button>
                    <span className="px-4 py-1">{item.quantity}</span>
                    <button
                      onClick={() => updateQuantity(item.productId, item.quantity + 1)}
                      className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
                    >
                      +
                    </button>
                  </div>
                  <button
                    onClick={() => removeItem(item.productId)}
                    className="text-red-500 hover:text-red-700 font-semibold"
                  >
                    Remove
                  </button>
                  <p className="text-lg font-bold">
                    ${((item.product?.price || 0) * item.quantity).toFixed(2)}
                  </p>
                </div>
              </div>
            ))}
          </div>
        </div>

        {/* Summary */}
        <div>
          <div className="bg-gray-50 rounded-lg p-6 sticky top-8">
            <h2 className="text-2xl font-bold mb-6">Order Summary</h2>

            <div className="space-y-4 mb-6">
              <div className="flex justify-between">
                <span className="text-gray-600">Subtotal</span>
                <span className="font-semibold">${subtotal.toFixed(2)}</span>
              </div>
              <div className="flex justify-between">
                <span className="text-gray-600">Tax (10%)</span>
                <span className="font-semibold">${tax.toFixed(2)}</span>
              </div>
              <div className="border-t pt-4 flex justify-between text-lg">
                <span className="font-bold">Total</span>
                <span className="font-bold text-purple-600">${total.toFixed(2)}</span>
              </div>
            </div>

            <Link
              to="/checkout"
              className="block w-full bg-purple-600 text-white text-center py-3 rounded-lg font-bold hover:bg-purple-700 mb-4 transition"
            >
              Proceed to Checkout
            </Link>

            <button
              onClick={() => clear()}
              className="w-full px-4 py-2 border border-red-500 text-red-500 rounded-lg hover:bg-red-50 transition"
            >
              Clear Cart
            </button>

            <Link
              to="/"
              className="block w-full text-center mt-4 text-purple-600 hover:text-purple-700 font-semibold"
            >
              Continue Shopping
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
}
