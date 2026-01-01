import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import Library from './pages/Library'; // Import Library component
import BookDetail from './pages/BookDetail'; // Import BookDetail component
import AddBook from './pages/AddBook'; // Import AddBook component
import './App.css'; // Keep existing CSS if any

function App() {
  return (
    <Router>
      <nav>
        <ul>
          <li>
            <Link to="/">Home</Link>
          </li>
          <li>
            <Link to="/login">Login</Link>
          </li>
          <li>
            <Link to="/register">Register</Link>
          </li>
          <li>
            <Link to="/library">Library</Link> {/* Added Library link */}
          </li>
          <li>
            <Link to="/add-book">Add Book</Link> {/* Added Add Book link */}
          </li>
        </ul>
      </nav>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/library" element={<Library />} />
        <Route path="/book/:id" element={<BookDetail />} />
        <Route path="/add-book" element={<AddBook />} /> {/* Added AddBook route */}
      </Routes>
    </Router>
  );
}

export default App;
