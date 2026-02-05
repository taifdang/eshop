import { apiAuth } from "@/shared/lib/api-auth";

export const fetchBasket = () => apiAuth.get(`/api/v1/basket`);

export const updateBasket = (variantId, quantity) =>
  apiAuth.post(`/api/v1/basket`, {
    variantId: variantId,
    quantity: quantity,
  });
