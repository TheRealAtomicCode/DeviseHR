import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./Router/AppRouter"; // Import the routes component

const App = () => {
  return (
    <Router> {/* This is the Router that wraps your routes */}
      <AppRoutes /> {/* Your routing logic */}
    </Router>
  );
};

export default App;
