/**
 * Main Layout Component
 * 
 * Wrapper component for all protected routes.
 * Provides consistent page structure with:
 * - Sticky navbar at top
 * - Dynamic page content via React Router Outlet
 * - Footer at bottom (sticky to bottom when content is short)
 * 
 * Layout automatically stretches to full viewport height.
 * Pages render in the <main> section.
 */

import { Outlet } from 'react-router-dom';
import { Navbar } from './Navbar';
import { Footer } from './Footer';

/**
 * Main page layout wrapper
 * @component
 * @returns {JSX.Element} Layout with navbar, dynamic content, and footer
 */
export function Layout() {
  return (
    <div className="flex flex-col min-h-screen bg-gray-50">
      <Navbar />
      <main className="flex-1 container mx-auto px-4 py-8 w-full">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}
