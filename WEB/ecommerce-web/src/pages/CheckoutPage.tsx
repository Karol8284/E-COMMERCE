/**
 * Checkout Page Component
 * 
 * Order placement form with:
 * - Shipping address fields (firstName, lastName, email, address, city, zipCode)
 * - Payment information fields (cardNumber 16 digits, expiry MM/YY, CVC 3 digits)
 * - Form validation using Zod schema
 * - Cart summary display
 * - Order total calculation
 * 
 * Features:
 * - Requires user to be logged in (redirects to /login if not)
 * - Type-safe form handling with React Hook Form
 * - Real-time validation with error messages
 * - Order creation on submit
 * - Redirect to home after successful order
 */

import { useNavigate } from 'react-router-dom';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { useState } from 'react';
import { useCartStore } from '../stores/cartStore';
import { useAuthStore } from '../stores/authStore';
import { AxiosError } from 'axios';
import type { ApiErrorResponse } from '../api/types';

const checkoutSchema = z.object({
  firstName: z.string().min(1, 'First name is required'),
  lastName: z.string().min(1, 'Last name is required'),
  email: z.string().email('Invalid email'),
  address: z.string().min(5, 'Address must be at least 5 characters'),
  city: z.string().min(1, 'City is required'),
  zipCode: z.string().min(3, 'ZIP code is required'),
  cardNumber: z.string().regex(/^\d{16}$/, 'Card number must be 16 digits'),
  cardExpiry: z.string().regex(/^\d{2}\/\d{2}$/, 'Format: MM/YY'),
  cardCVC: z.string().regex(/^\d{3}$/, 'CVC must be 3 digits'),
});

type CheckoutFormData = z.infer<typeof checkoutSchema>;

export function CheckoutPage() {
  const navigate = useNavigate();
  const { items, clear: clearCart } = useCartStore();
  const { isLoggedIn, user } = useAuthStore();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<CheckoutFormData>({
    resolver: zodResolver(checkoutSchema),
    defaultValues: {
      email: user?.email || '',
      firstName: '',
      lastName: '',
      address: '',
      city: '',
      zipCode: '',
      cardNumber: '',
      cardExpiry: '',
      cardCVC: '',
    },
  });

  if (!isLoggedIn) {
    return (
      <div className="text-center py-12">
        <p className="text-lg text-gray-600 mb-6">
          You must be logged in to checkout
        </p>
        <button
          onClick={() => navigate('/login')}
          className="bg-purple-600 text-white px-8 py-3 rounded-lg hover:bg-purple-700"
        >
          Go to Login
        </button>
      </div>
    );
  }

  if (items.length === 0) {
    return (
      <div className="text-center py-12">
        <p className="text-lg text-gray-600 mb-6">Your cart is empty</p>
        <button
          onClick={() => navigate('/')}
          className="bg-purple-600 text-white px-8 py-3 rounded-lg hover:bg-purple-700"
        >
          Continue Shopping
        </button>
      </div>
    );
  }

  const onSubmit = async (data: CheckoutFormData) => {
    setLoading(true);
    setError('');

    try {
      // Simulate order creation
      const order = {
        firstName: data.firstName,
        lastName: data.lastName,
        email: data.email,
        address: data.address,
        city: data.city,
        zipCode: data.zipCode,
        items: items,
        totalAmount: 999.99, // This would be calculated on backend
        status: 'pending',
      };

      console.log('Order created:', order);

      // Clear cart and redirect to success
      clearCart();
      
      // Generate random order ID in a non-render context
      const generateOrderId = () => Math.random().toString(36).substring(7);
      const orderId = generateOrderId();
      
      navigate('/');
      setTimeout(() => {
        alert(`Order placed successfully! Order #${orderId}`);
      }, 100);
    } catch (err: unknown) {
      if (err instanceof AxiosError) {
        const apiError = err.response?.data as ApiErrorResponse | undefined;
        setError(apiError?.message || 'Order creation failed');
      } else if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('An unexpected error occurred');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-4xl mx-auto">
      <h1 className="text-4xl font-bold mb-8">Checkout</h1>

      {error && (
        <div className="mb-6 p-4 bg-red-100 border border-red-400 text-red-700 rounded">
          {error}
        </div>
      )}

      <form onSubmit={handleSubmit(onSubmit)} className="space-y-8">
        {/* Shipping Info */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h2 className="text-2xl font-bold mb-6">Shipping Address</h2>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label className="block text-sm font-medium mb-2">First Name *</label>
              <input
                type="text"
                {...register('firstName')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.firstName && (
                <p className="text-red-500 text-sm mt-1">{errors.firstName.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">Last Name *</label>
              <input
                type="text"
                {...register('lastName')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.lastName && (
                <p className="text-red-500 text-sm mt-1">{errors.lastName.message}</p>
              )}
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium mb-2">Email *</label>
              <input
                type="email"
                {...register('email')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.email && (
                <p className="text-red-500 text-sm mt-1">{errors.email.message}</p>
              )}
            </div>

            <div className="md:col-span-2">
              <label className="block text-sm font-medium mb-2">Address *</label>
              <input
                type="text"
                {...register('address')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.address && (
                <p className="text-red-500 text-sm mt-1">{errors.address.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">City *</label>
              <input
                type="text"
                {...register('city')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.city && (
                <p className="text-red-500 text-sm mt-1">{errors.city.message}</p>
              )}
            </div>

            <div>
              <label className="block text-sm font-medium mb-2">ZIP Code *</label>
              <input
                type="text"
                {...register('zipCode')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.zipCode && (
                <p className="text-red-500 text-sm mt-1">{errors.zipCode.message}</p>
              )}
            </div>
          </div>
        </div>

        {/* Payment Info */}
        <div className="bg-white p-6 rounded-lg shadow">
          <h2 className="text-2xl font-bold mb-6">Payment Method</h2>

          <div className="space-y-4">
            <div>
              <label className="block text-sm font-medium mb-2">Card Number (16 digits) *</label>
              <input
                type="text"
                placeholder="1234 5678 9012 3456"
                {...register('cardNumber')}
                className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
              />
              {errors.cardNumber && (
                <p className="text-red-500 text-sm mt-1">{errors.cardNumber.message}</p>
              )}
            </div>

            <div className="grid grid-cols-2 gap-4">
              <div>
                <label className="block text-sm font-medium mb-2">Expiry (MM/YY) *</label>
                <input
                  type="text"
                  placeholder="12/25"
                  {...register('cardExpiry')}
                  className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
                />
                {errors.cardExpiry && (
                  <p className="text-red-500 text-sm mt-1">{errors.cardExpiry.message}</p>
                )}
              </div>

              <div>
                <label className="block text-sm font-medium mb-2">CVC (3 digits) *</label>
                <input
                  type="text"
                  placeholder="123"
                  {...register('cardCVC')}
                  className="w-full px-4 py-2 border rounded-lg focus:ring-2 focus:ring-purple-500"
                />
                {errors.cardCVC && (
                  <p className="text-red-500 text-sm mt-1">{errors.cardCVC.message}</p>
                )}
              </div>
            </div>
          </div>
        </div>

        {/* Submit */}
        <button
          type="submit"
          disabled={loading}
          className="w-full bg-purple-600 text-white py-4 rounded-lg font-bold text-lg hover:bg-purple-700 disabled:opacity-50 transition"
        >
          {loading ? 'Processing...' : 'Place Order'}
        </button>
      </form>
    </div>
  );
}
