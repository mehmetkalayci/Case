import React from "react";
import * as ReactDOM from "react-dom/client";

import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import { AuthProvider } from "./providers/AuthProvider";
import ProtectedRoute from "./routes/ProtectedRoute";
import routes from "./routes/routes";

import { Toaster } from 'react-hot-toast';

ReactDOM.createRoot(document.getElementById("root")).render(
  <React.StrictMode>
    <AuthProvider>
      <Router>
        <Routes>
          {routes.map(
            ({ path, component: Component, protected: isProtected }) => (
              <Route
                key={path}
                path={path}
                element={
                  isProtected ? (
                    <ProtectedRoute element={Component} />
                  ) : (
                    <Component />
                  )
                }
              />
            )
          )}
        </Routes>
      </Router>
      <Toaster position="top-center" reverseOrder={false} />
    </AuthProvider>
  </React.StrictMode>
);
