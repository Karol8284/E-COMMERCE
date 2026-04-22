/**
 * Main Router Configuration
 * 
 * Defines all application routes:
 * - Public routes: /login
 * - Protected routes (with Layout): /, /products/:id, /cart, /checkout, /dashboard
 * - 404 fallback route
 * 
 * Route Structure:
 * /                    -> ProductsPage (shows all products)
 * /products/:id        -> ProductDetailPage (single product details)
 * /cart                -> CartPage (shopping cart)
 * /checkout            -> CheckoutPage (order placement)
 * /login               -> LoginPage (authentication)
 * /dashboard           -> DashboardPage (user profile & orders)
 * 
 * Protected routes require user to be logged in (enforced by frontend check).
 */

import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { LoginPage } from '../pages/LoginPage';
import { ProductsPage } from '../pages/ProductsPage';
import { ProductDetailPage } from '../pages/ProductDetailPage';
import { CartPage } from '../pages/CartPage';
import { CheckoutPage } from '../pages/CheckoutPage';
import { DashboardPage } from '../pages/DashboardPage';
import { Layout } from '../components/Layout';

export function AppRouter() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Publiczne - bez layout */}
        <Route path="/login" element={<LoginPage />} />

        {/* Z Layout */}
        <Route element={<Layout />}>
          {/* Home & Products */}
          <Route path="/" element={<ProductsPage />} />
          <Route path="/products/:id" element={<ProductDetailPage />} />

          {/* Cart & Checkout */}
          <Route path="/cart" element={<CartPage />} />
          <Route path="/checkout" element={<CheckoutPage />} />

          {/* User Dashboard */}
          <Route path="/dashboard" element={<DashboardPage />} />
        </Route>

        {/* 404 */}
        <Route
          path="*"
          element={
            <div className="text-center py-12">
              <h1 className="text-3xl font-bold mb-4">404 - Page Not Found</h1>
            </div>
          }
        />
      </Routes>
    </BrowserRouter>
  );
}