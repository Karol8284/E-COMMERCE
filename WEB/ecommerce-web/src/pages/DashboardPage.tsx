/**
 * User Dashboard Page Component
 * 
 * User account management page showing:
 * - Welcome message with user email
 * - Account statistics (Total Orders, Pending Orders, Total Spent)
 * - User information display
 * - Admin panel (conditionally shown if user role is 'admin')
 * - Recent orders section
 * 
 * Requires user to be logged in - redirects to /login if not authenticated.
 */

import { useAuthStore } from '../stores/authStore';
import { Link } from 'react-router-dom';

/**
 * User dashboard page
 * @component
 * @returns {JSX.Element} Dashboard with user info and orders
 */
export function DashboardPage() {
  const { user, isLoggedIn } = useAuthStore();

  if (!isLoggedIn || !user) {
    return (
      <div className="text-center py-12">
        <p className="text-lg text-gray-600 mb-6">Please log in to access dashboard</p>
        <Link
          to="/login"
          className="inline-block bg-purple-600 text-white px-8 py-3 rounded-lg hover:bg-purple-700"
        >
          Go to Login
        </Link>
      </div>
    );
  }

  return (
    <div>
      <h1 className="text-4xl font-bold mb-8">Dashboard</h1>

      {/* Welcome Card */}
      <div className="bg-gradient-to-r from-purple-500 to-pink-500 text-white rounded-lg p-8 mb-8">
        <h2 className="text-2xl font-bold mb-2">Welcome, {user.email}!</h2>
        <p className="text-purple-100">Role: {user.role}</p>
      </div>

      {/* Quick Stats */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6 mb-8">
        <div className="bg-white rounded-lg shadow p-6">
          <div className="text-gray-500 text-sm mb-2">Total Orders</div>
          <div className="text-3xl font-bold">0</div>
        </div>
        <div className="bg-white rounded-lg shadow p-6">
          <div className="text-gray-500 text-sm mb-2">Pending Orders</div>
          <div className="text-3xl font-bold">0</div>
        </div>
        <div className="bg-white rounded-lg shadow p-6">
          <div className="text-gray-500 text-sm mb-2">Total Spent</div>
          <div className="text-3xl font-bold">$0.00</div>
        </div>
      </div>

      {/* Account Info */}
      <div className="bg-white rounded-lg shadow p-8 mb-8">
        <h2 className="text-2xl font-bold mb-6">Account Information</h2>

        <div className="space-y-4">
          <div>
            <p className="text-sm text-gray-500">Email Address</p>
            <p className="text-lg font-semibold">{user.email}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">User ID</p>
            <p className="text-sm font-mono text-gray-600">{user.id}</p>
          </div>

          <div>
            <p className="text-sm text-gray-500">Role</p>
            <p className="text-lg font-semibold capitalize">
              {user.role === 'admin' ? '👑 Administrator' : 'Customer'}
            </p>
          </div>
        </div>
      </div>

      {/* Admin Panel */}
      {user.role === 'admin' && (
        <div className="bg-white rounded-lg shadow p-8 mb-8 border-l-4 border-purple-600">
          <h2 className="text-2xl font-bold mb-6">👑 Admin Panel</h2>
          <p className="text-gray-600 mb-6">You have administrator privileges</p>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            <button className="p-4 bg-purple-50 border border-purple-200 rounded-lg hover:bg-purple-100 transition">
              Manage Products
            </button>
            <button className="p-4 bg-purple-50 border border-purple-200 rounded-lg hover:bg-purple-100 transition">
              View Orders
            </button>
            <button className="p-4 bg-purple-50 border border-purple-200 rounded-lg hover:bg-purple-100 transition">
              Manage Users
            </button>
            <button className="p-4 bg-purple-50 border border-purple-200 rounded-lg hover:bg-purple-100 transition">
              View Reports
            </button>
          </div>
        </div>
      )}

      {/* Recent Orders Section */}
      <div className="bg-white rounded-lg shadow p-8">
        <h2 className="text-2xl font-bold mb-6">Recent Orders</h2>
        <div className="text-center py-12 text-gray-500">
          <p>No orders yet</p>
          <Link to="/" className="text-purple-600 hover:text-purple-700 font-semibold mt-4 inline-block">
            Start Shopping →
          </Link>
        </div>
      </div>
    </div>
  );
}
