/**
 * Footer Component
 * 
 * Site footer with:
 * - Brand information
 * - Quick navigation links
 * - Support section
 * - Legal links (Privacy, Terms)
 * - Copyright notice
 * 
 * Dark themed footer that stays at the bottom of the page.
 * Responsive grid layout (1 col mobile, 4 cols desktop).
 */

/**
 * Footer component
 * @component
 * @returns {JSX.Element} Site footer with links and branding
 */
export function Footer() {
  return (
    <footer className="bg-gray-900 text-white mt-16">
      <div className="container mx-auto px-4 py-12">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
          {/* Brand */}
          <div>
            <h3 className="text-2xl font-bold text-purple-400 mb-4">E-Commerce</h3>
            <p className="text-gray-400">
              Your one-stop shop for quality products
            </p>
          </div>

          {/* Quick Links */}
          <div>
            <h4 className="text-lg font-bold mb-4">Quick Links</h4>
            <ul className="space-y-2 text-gray-400">
              <li><a href="/" className="hover:text-white transition">Products</a></li>
              <li><a href="/" className="hover:text-white transition">Cart</a></li>
              <li><a href="/" className="hover:text-white transition">Account</a></li>
            </ul>
          </div>

          {/* Support */}
          <div>
            <h4 className="text-lg font-bold mb-4">Support</h4>
            <ul className="space-y-2 text-gray-400">
              <li><a href="#" className="hover:text-white transition">Help Center</a></li>
              <li><a href="#" className="hover:text-white transition">Contact Us</a></li>
              <li><a href="#" className="hover:text-white transition">FAQ</a></li>
            </ul>
          </div>

          {/* Legal */}
          <div>
            <h4 className="text-lg font-bold mb-4">Legal</h4>
            <ul className="space-y-2 text-gray-400">
              <li><a href="#" className="hover:text-white transition">Privacy Policy</a></li>
              <li><a href="#" className="hover:text-white transition">Terms of Service</a></li>
              <li><a href="#" className="hover:text-white transition">Cookies</a></li>
            </ul>
          </div>
        </div>

        <div className="border-t border-gray-800 pt-8 text-center text-gray-400">
          <p>&copy; 2026 E-Commerce Platform. All rights reserved.</p>
        </div>
      </div>
    </footer>
  );
}
