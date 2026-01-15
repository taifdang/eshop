import { apiAuth } from "@/shared/lib/api-auth";
import { api } from "@/shared/lib/api-client";

export const fetchOrder = () => apiAuth.get(`/api/v1/orders`);

export const placeOrder = (_customerId, _method, _provider, _street, _city, _zipCode, ) =>
  apiAuth.post(`api/v1/orders`, {
    customerId: _customerId,
    method: _method,
    provider: _provider,
    street: _street,
    city: _city,
    zipCode: _zipCode,
  });

export const createPaymentUrl = (_orderNumber, _amount, _provider) =>
  api.post(`api/v1/payment/create`, {
    orderNumber: _orderNumber,
    amount: _amount,
    provider: _provider,
  });
