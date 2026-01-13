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
import { placeOrder } from "../../services/order-service";

export default function CheckoutPage() {
  const navigate = useNavigate();

  const handleOrderSuccess = () => {
    navigate("checkout/result?status=confirm");
  };
  const handleOrderFailure = () => {
    navigate("checkout/result?status=failure");
  };

  //* VARIABLES
  const PAYMENT_PROVIDERS = [
    { id: "cod", label: "Cash on Delivery", status:"" },
    { id: "vnpay", label: "VnPay", status:"" },
    { id: "stripe", label: "Stripe", status:"" },
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
  const [paymentMethod, setPaymnetMethod] = useState(PAYMENT_PROVIDERS[0].id);
  const [shippingAddress, setShippingAddress] = useState(initialAddress);

  const [formValidated, setFormValidated] = useState(false);

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
    mutationFn: ({ customerId, street, city, zipCode }) =>
      placeOrder(customerId, street, city, zipCode),
    onSuccess: () => {
      handleOrderSuccess();
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
                onChange={setPaymnetMethod}
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
    </div>
  );
}
