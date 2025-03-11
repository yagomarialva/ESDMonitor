import React, { useEffect } from "react";
import "./i18n"; // ts => import './i18n.ts'
import HomePage from "./pages/HomePage/HomePage";
import { Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage/LoginPage";
import ProtectedRoute from "./ProtectedRoute";
import DashboardESD from "./components/ESD/DashboardESD/DashboardESD";
import NotFoundPage from "./components/NotFoundPage/NotFoundPage";


const AppRoutes: React.FC = () => {
  useEffect(() => {
    document.title = "FCT Auto Test";
  }, []);

  return (
    <>
      <Routes>
        <Route
          path="/"
          element={
            <ProtectedRoute>
              <HomePage />
            </ProtectedRoute>
          }
        />
        <Route path="/login" element={<LoginPage />} />
        <Route
          path="/dashboard"
          element={
            // <ProtectedRoute>
              <DashboardESD />
            // </ProtectedRoute>
          }
        />
        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  );
};

export default AppRoutes;
