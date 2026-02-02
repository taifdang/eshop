import { apiAuth } from "@/shared/lib/api-auth";

/* OLD: Direct API paths
export const fetchBasket = () => apiAuth.get(`/api/v1/basket`);

export const updateBasket = (variantId, quantity) =>
  apiAuth.post(`api/v1/basket`, {
    variantId: variantId,
    quantity: quantity,
  });
*/

// NEW: BFF API proxy paths (BFF routes to /bff/api/*)
export const fetchBasket = () => apiAuth.get(`/api/v1/basket`);

export const updateBasket = (variantId, quantity) =>
  apiAuth.post(`/api/v1/basket`, {
    variantId: variantId,
    quantity: quantity,
  });
