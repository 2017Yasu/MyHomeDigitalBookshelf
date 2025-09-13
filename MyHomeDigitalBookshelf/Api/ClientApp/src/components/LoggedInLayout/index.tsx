import { Nav } from 'react-bootstrap';
import { Outlet, useLocation } from 'react-router-dom';

const navLinks = Object.freeze([
  { link: '/bookshelves', label: 'Bookshelves' },
  { link: '/management', label: 'Management' },
]);

export default function LoggedInLayout() {
  const location = useLocation();

  return (
    <div>
      {/* Sidebar Navigation */}
      <Nav className="sidebar flex-column" variant="pills">
        <Nav.Item>
          <Nav.Link href="/" active={location.pathname === '/'}>
            Dashboard
          </Nav.Link>
        </Nav.Item>
        <hr />

        {/* Dynamically generated navigation links */}
        {navLinks.map(({ link, label }) => (
          <Nav.Item key={link}>
            <Nav.Link href={link} active={location.pathname.startsWith(link)}>
              {label}
            </Nav.Link>
          </Nav.Item>
        ))}
      </Nav>

      {/* Main Content Area */}
      <div className="main-content">
        <header className="header-content">
          {/* You can add user info, notifications, etc. here */}
          Header
        </header>
        <main className="p-4">
          {/* Render children or placeholder for now */}
          <Outlet />
        </main>
      </div>
    </div>
  );
}
