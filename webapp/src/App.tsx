import { BrowserRouter, Route, Routes } from 'react-router-dom';

import LoggedInLayout from './components/LoggedInLayout';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<LoggedInLayout />}>
          <Route path="/" element={<div>Dashboard Content</div>} />
          <Route path="/bookshelves" element={<div>Bookshelves Content</div>} />
          <Route path="/management" element={<div>Management Content</div>} />
        </Route>
        {/* Add other routes here if needed */}
        <Route path="*" element={<div>404 Not Found</div>} /> {/* Fallback route for unmatched paths */}
      </Routes>
    </BrowserRouter>
  );
}

export default App;
