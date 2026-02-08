import HomePage from "@/features/home/HomePage";
import { BasketPage } from "../features/basket/pages/Basket/BasketPage";
import { ProductDetailPage } from "../features/catalog/product/pages/ProductDetail/ProductDetailPage";

import NotFound from "./not-found";

import { CheckoutPage } from "../features/ordering/checkout/pages/Checkout/CheckoutPage";
import { CheckoutResultPage } from "../features/ordering/checkout/pages/CheckoutResult/CheckoutResultPage";
import StoreFrontLayout from "../layouts/storefront/layout";

export const routes = [
  {
    element: <StoreFrontLayout />,
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
