/**
 * Navigation Bar Component
 * 
 * Top navigation showing:
 * - Logo/home link
 * - Navigation links (Products, Cart)
 * - Authentication status:
 *   - If logged in: User email + Logout button
 *   - If logged out: Login button
 * - Cart item count badge (red circle in top-right)
 * 
 * Sticky positioning - stays at top while scrolling.
 * Responsive design for mobile/tablet/desktop.
 */

import { Link } from 'react-router-dom';
import { useAuthStore } from '../stores/authStore';
import { useCartStore } from '../stores/cartStore';

/**
 * Navigation bar component
 * @component
 * @returns {JSX.Element} Sticky header with navigation
 */
export function Navbar() {
  const { isLoggedIn, user, logout } = useAuthStore();
  const { items: cartItems } = useCartStore();
  const cartCount = cartItems.reduce((sum, item) => sum + item.quantity, 0);

  return (
    <nav className="bg-white shadow-md sticky top-0 z-50">
      <div className="container mx-auto px-4 py-4">
        <div className="flex justify-between items-center">
          {/* Logo */}
          <Link to="/" className="text-2xl font-bold text-purple-600 hover:text-purple-700">
            E-Commerce
          </Link>

          {/* Links */}
          <div className="flex gap-6 items-center">
            <Link to="/" className="text-gray-700 hover:text-purple-600 transition">
              Products
            </Link>

            <Link
              to="/cart"
              className="text-gray-700 hover:text-purple-600 transition flex items-center gap-2"
            >
              🛒 Cart
              {cartCount > 0 && (
                <span className="bg-red-500 text-white text-xs rounded-full w-6 h-6 flex items-center justify-center">
                  {cartCount}
                </span>
              )}
            </Link>

            {isLoggedIn && user ? (
              <>
                <Link
                  to="/dashboard"
                  className="text-gray-700 hover:text-purple-600 transition"
                >
                  Dashboard
                </Link>

                <div className="flex items-center gap-4 border-l pl-6">
                  <span className="text-sm text-gray-600">{user.email}</span>
                  <button
                    onClick={() => {
                      logout();
                      window.location.href = '/login';
                    }}
                    className="bg-red-500 text-white px-4 py-2 rounded-lg hover:bg-red-600 transition"
                  >
                    Logout
                  </button>
                </div>
              </>
            ) : (
              <Link
                to="/login"
                className="bg-purple-600 text-white px-6 py-2 rounded-lg hover:bg-purple-700 transition"
              >
                Login
              </Link>
            )}
          </div>
        </div>
      </div>
    </nav>
  );
}
