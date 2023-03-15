import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import LandingPage from "./components/LandingPage";
import UserDemo from "./pages/UserDemo";
import AdminDemo from "./pages/AdminDemo";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<LandingPage />} />
        <Route path="/user-demo" element={<UserDemo />} />
        <Route path="/admin-demo" element={<AdminDemo />} />
      </Routes>
    </Router>
  );
}

export default App;
