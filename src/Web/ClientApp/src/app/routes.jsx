
import NoPage from "@/pages/NoPage";
import Layout from "@/shared/components/layout/Layout";
import ProductDetailPage from "@/features/catalog/pages/ProductDetailPage";
import LoginPage from "@/features/identity/pages/LoginPage";
import RegisterPage from "@/features/identity/pages/RegisterPage";
import CartPage from "../features/basket/pages/CartPage";
import CheckoutPage from "../features/order/pages/CheckoutPage";
import HomePage from "../pages/Home/HomePage";

export const routes = [
  {
    element: <Layout />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/detail", element: <ProductDetailPage /> },
    ],
    errorElement: <NoPage />,
  },
  {
    path: "/login",
    element: <LoginPage />,
  },
  {
    path: "/signup",
    element: <RegisterPage />,
  },
  {
    path: "/cart",
    element: <CartPage />,
  },
  {
    path: "/checkout",
    element: <CheckoutPage />,
  },
];
