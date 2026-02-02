import { api } from "@/shared/lib/api-client";

/* OLD: Direct API paths
export const fetchProducts = (page = 1, size = 10) =>
  api.get(
    `/api/v1/catalog/products/get-available/?PageSize=${size}&PageIndex=${page}`
  );

export const fetchProductById = (id) =>
  api.get(`/api/v1/catalog/products/${id}`);

export const fetchVariantByOptions = (productId, optionValueMap) =>
  api.post(
    `/api/v1/catalog/products/variants/by-options`,
    optionValueMap,
    {
      params: {
        ProductId: productId, // query params
      },
    }
  );
*/

// NEW: BFF API proxy paths (BFF routes to /bff/api/*)
export const fetchProducts = (page = 1, size = 10) =>
  api.get(
    `/api/v1/catalog/products/get-available/?PageSize=${size}&PageIndex=${page}`
  );

export const fetchProductById = (id) =>
  api.get(`/api/v1/catalog/products/${id}`);

export const fetchVariantByOptions = (productId, optionValueMap) =>
  api.post(
    `/api/v1/catalog/products/variants/by-options`,
    optionValueMap,
    {
      params: {
        ProductId: productId, // query params
      },
    }
  );
