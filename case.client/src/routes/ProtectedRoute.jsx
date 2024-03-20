import { useContext } from "react";
import { Navigate } from "react-router-dom";
import { AuthContext } from "../providers/AuthProvider";

const ProtectedRoute = ({ component: Component, ...props }) => {
  const { user } = useContext(AuthContext);

  return user ? <Component {...props} /> : <Navigate to="/login" />;
};

export default ProtectedRoute;
