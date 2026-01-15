import { NavBar } from "@/shared/components/layout/NavBar";
import {
  CheckoutHeader,
  ShippingAddress as ShippingAdress,
  CheckoutContent,
} from "../../components";

import s from "./CheckoutPage.module.css";
import clsx from "clsx";

import PaymentProvider from "@/shared/components/PaymentProvider";

import { useEffect, useState } from "react";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useNavigate } from "react-router-dom";

import { fetchBasket } from "../../../basket/services/basket-service";
import { formatCurrency } from "@/shared/lib/currency";
import { placeOrder, createPaymentUrl } from "../../services/order-service";
import Modal from "@/shared/components/Modal";

export default function CheckoutPage() {
  const navigate = useNavigate();

  const handleOrderSuccess = async (res) => {
    console.log("order response:", res);
    // If online payment selected (method === 2) and provider is not 0, create payment URL and redirect
    if (paymentProvider?.method === 2 && paymentProvider?.provider !== 0) {
      try {
        const orderNumber = res?.orderNumber;
        const amount = res?.amount || 0;

        const resp = await createPaymentUrl(
          orderNumber,
          amount,
          res?.paymentProvider
        );

        var payment = resp?.data;

        if (!payment.status) {
          setIsValid(true);
          console.log("error:", payment.error);
        }
        const paymentUrl = payment?.data || "";
        if (paymentUrl) {
          window.location.href = paymentUrl;
        }
      } catch (err) {
        setIsValid(true);
        console.error("createPaymentUrl error:", err);
      }
    }

    navigate("/checkout/result?status=confirm");
  };
  const handleOrderFailure = () => {
    setIsValid(true);
    // navigate("/checkout/result?status=failure");
  };

  //* VARIABLES
  const PAYMENT_PROVIDERS = [
    { id: "cod", label: "Cash on Delivery", method: 1, provider: 0 },
    { id: "vnpay", label: "VnPay", method: 2, provider: 1 },
    { id: "stripe", label: "Stripe", method: 2, provider: 2 },
  ];
  const initialAddress = {
    fullname: "",
    phoneNumber: "",
    city: "",
    zipCode: "",
    street: "",
  };

  //* HOOKS
  const [open, setOpen] = useState(false);

  const [paymentMethod, setPaymentMethod] = useState(PAYMENT_PROVIDERS[0].id);
  const [paymentProvider, setPaymentProvider] = useState(PAYMENT_PROVIDERS[0]);

  const [shippingAddress, setShippingAddress] = useState(initialAddress);
  const [formValidated, setFormValidated] = useState(false);
  const [isValid, setIsValid] = useState(false);

  const handleChangePayment = (methodId) => {
    setPaymentMethod(methodId);
    const selectedProvider = PAYMENT_PROVIDERS.find((p) => p.id === methodId);
    setPaymentProvider(selectedProvider);
  };

  // get basket item, set init data
  const { data: basket } = useQuery({
    queryKey: ["basket"],
    queryFn: () => fetchBasket().then((res) => res.data),
    retry: false,
    refetchOnWindowFocus: false,
    initialData: {
      id: "",
      customerId: "",
      items: [],
      createdAt: new Date().toDateString(),
      lastModified: null,
    },
  });
  // mutation price: reduce(func, initValue)
  const totalResult = basket.items?.reduce(
    (sum, item) => (sum = sum + item.regularPrice * item.quantity),
    0
  );

  // first load page: open modal if no validated
  useEffect(() => {
    if (!formValidated) {
      setOpen(true); //open modal
    }
  }, []);

  const hasEmptyField = Object.values(shippingAddress).some(
    (value) => !value || value.trim() === ""
  );

  const mutationPlaceOrder = useMutation({
    mutationFn: ({ customerId, method, provider, street, city, zipCode }) =>
      placeOrder(customerId, method, provider, street, city, zipCode),
    onSuccess: (res) => {
      handleOrderSuccess(res.data);
      console.log("order success");
    },
    onError: (err) => {
      handleOrderFailure();
      console.log("order error: ", err);
    },
  });

  const handlePlaceOrder = async () => {
    if (hasEmptyField) return;
    if (basket.customerId === "");

    mutationPlaceOrder.mutate({
      customerId: basket.customerId,
      method: paymentProvider.method,
      provider: paymentProvider.provider,
      street: shippingAddress.street,
      city: shippingAddress.city,
      zipCode: shippingAddress.zipCode,
    });
  };

  return (
    <div>
      <NavBar />
      <div>
        {/* HEADER */}
        <div className="bg-white" style={{ marginBottom: "12px" }}>
          <CheckoutHeader />
        </div>
        <div
          role="main"
          className="container-wrapper mx-auto"
          style={{ fontSize: "14px", lineHeight: "16.8px" }}
        >
          {/* SHIPPING ADDRESS*/}
          <ShippingAdress
            isOpen={open}
            onSetOpen={setOpen}
            address={shippingAddress}
            status={formValidated}
            onSetStatus={setFormValidated}
            onSubmitAddress={setShippingAddress}
          />
          {/* CHECKOUT CONTENT */}
          <div style={{ marginTop: "12px", backgroundColor: "white" }}>
            <CheckoutContent items={basket?.items} />
          </div>
          <div className={s["checkout-section__footer"]}>
            {/* PAYMENT SECTION  */}
            <div className={s["checkout-footer-with-payment-section"]}>
              {/* this section is global component, it's can use in other page */}
              <PaymentProvider
                items={PAYMENT_PROVIDERS}
                value={paymentMethod}
                onChange={handleChangePayment}
              />
            </div>
            <div className={s["checkout-footer"]}>
              <h3
                className={clsx(
                  s["checkout-footer-grid-per-row"],
                  s["checkout-footer__title"]
                )}
              >
                Total Payment
              </h3>
              <div
                className={clsx(
                  s["checkout-footer-grid-per-row"],
                  s["checkout-footer-total-price"]
                )}
              >
                {formatCurrency(totalResult)}
              </div>
              <div
                className={clsx(
                  s["checkout-footer-grid-per-row"],
                  s["checkout-footer-with-button-order"]
                )}
              >
                <div></div>
                <button
                  onClick={() => {
                    handlePlaceOrder();
                  }}
                  className={s["checkout-footer-button-order"]}
                >
                  Place Order
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
      {isValid && (
        <Modal open={isValid} onClose={() => setIsValid(false)}>
          <div className={s["w4p-container"]}>
            <div className={s["w4p-wrapper"]}>
              <div className={s["w4p-box"]}>
                <div className={s["w4p-box__subtitle"]}>
                  <p>
                    Oops! We couldn’t process your order. Please check the
                    following:
                    <br />
                    1. All items in your order must use the same payment method.
                    <br />
                    2. Please review your delivery address.
                    <br />
                    3. Please review your payment details or choose a different
                    payment option.
                  </p>
                </div>
                <div className="flex w-100">
                  <button
                    onClick={() => setIsValid(false)}
                    className={s["w4p__button"]}
                  >
                    OK, got it
                  </button>
                </div>
              </div>
            </div>
          </div>
        </Modal>
      )}
    </div>
  );
}
