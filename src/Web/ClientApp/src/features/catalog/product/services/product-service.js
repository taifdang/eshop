import { apiClient } from "@/shared/lib/api-client";

export const fetchProducts = (page = 1, size = 10) =>
  apiClient.get(
    `/api/v1/catalog/products/get-available/?PageSize=${size}&PageIndex=${page}`
  );

export const fetchProductById = (id) =>
  apiClient.get(`/api/v1/catalog/products/${id}`);

export const fetchVariantByOptions = (productId, optionValueMap) =>
  apiClient.post(
    `/api/v1/catalog/products/variants/by-options`,
    optionValueMap,
    {
      params: {
        ProductId: productId, // query params
      },
    }
  );
