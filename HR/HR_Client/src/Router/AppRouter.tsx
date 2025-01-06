import React from "react";
import { Route, Routes, Outlet } from "react-router-dom";
import HomePage from "../Pages/HomePage"; // Home page component
import LoginPage from "../Pages/LoginPage"; // Login page component
import NotFoundPage from "../Pages/NotFoundPage"; // Not Found page component
import Nav from "../Components/Nav/SideNav"; // Nav component
import AuthenticatedLayout from "../Layouts/AuthenticatedLayout";



const AppRoutes = () => {
  return (
    <Routes>
      {/* Routes with Layout (Nav shown) */}
      <Route element={<AuthenticatedLayout />}>
        <Route path="/" element={<HomePage />} /> {/* Home route */}
      </Route>

      {/* Routes without Layout (No Nav) */}
      <Route path="/login" element={<LoginPage />} /> {/* Login route */}
      <Route path="*" element={<NotFoundPage />} /> {/* Catch-all route for unknown paths */}
    </Routes>
  );
};

export default AppRoutes;
