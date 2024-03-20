import Home from "../pages/Home";
import About from "../pages/About";
import Login from "../pages/Login";
import Register from "../pages/Register";
import ErrorPage from "../pages/ErrorPage";

const routes = [
  {
    path: "/login",
    component: Login,
    protected: false,
  },
  {
    path: "/register",
    component: Register,
    protected: false,
  },
  {
    path: "/home",
    component: Home,
    protected: true,
  },
  {
    path: "/about",
    component: About,
    protected: true,
  },
  {
    path: "*",
    component: ErrorPage,
  },
];

export default routes;
