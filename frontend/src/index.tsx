import React from "react";
import ReactDOM from "react-dom/client";
import AppRoutes from "./AppRoutes"; // Não é necessário a extensão .tsx
import { BrowserRouter as Router, HashRouter } from "react-router-dom";
import "../node_modules/bootstrap/dist/css/bootstrap.min.css";
import { AuthProvider } from "./context/AuthContext"; // Retirando a extensão .js

const rootElement = document.getElementById("root");

// Verifica se rootElement não é nulo, para garantir a tipagem correta
if (rootElement) {
  const root = ReactDOM.createRoot(rootElement);

  root.render(
    <AuthProvider>
      <HashRouter>
        <AppRoutes />
      </HashRouter>
    </AuthProvider>
  );
}
