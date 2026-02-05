import HomePage from "@/features/home/HomePage";
import { ProductDetailPage } from "@/features/catalog/pages/ProductDetail/ProductDetailPage";
import { BasketPage } from "../features/basket/pages/Basket/BasketPage";

import NotFound from "./not-found";
import UserLayout from "../layouts/user/layout";

import { CheckoutPage } from "../features/ordering/checkout/pages/Checkout/CheckoutPage";
import { CheckoutResultPage } from "../features/ordering/checkout/pages/CheckoutResult/CheckoutResultPage";

export const routes = [
  {
    element: <UserLayout />,
    children: [
      { path: "/", element: <HomePage /> },
      { path: "/product/:id", element: <ProductDetailPage /> },
    ],
    errorElement: <NotFound />,
  },
  { path: "/cart", element: <BasketPage /> },
  { path: "/checkout", element:  <CheckoutPage /> },
  { path: "/checkout/result", element:  <CheckoutResultPage /> },
];
