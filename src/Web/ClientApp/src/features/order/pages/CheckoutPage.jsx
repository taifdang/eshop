import { NavBar } from "@/shared/components/layout/NavBar";
import CheckoutHeader from "../components/CheckoutHeader";
import ShippingAdress from "../components/ShippingAddress";
import CheckoutMain from "../components/CheckoutMain";
import CheckoutFooter from "../components/CheckoutFooter";
import { useEffect, useState } from "react";
import s from "../Checkout.module.css";
import clsx from "clsx";
import Checkout from "@/shared/components/Checkout";
import { useMutation, useQuery } from "@tanstack/react-query";
import { fetchBasket } from "../../basket/services/basket-service";
import { formatCurrency } from "@/shared/lib/currency";
import { profileStorage } from "@/shared/storage/profile-storage";
import { placeOrder } from "../services/order-service";

export default function CheckoutPage() {

  //* VARIABLES
  const PAYMENT_PROVIDERS = [
    { id: "cod", label: "Cash on Delivery" },
    { id: "vnpay", label: "VnPay" },
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

  //
  useEffect(() => {
    if (!formValidated) {
      setOpen(true); //open modal
    }
  }, []);

  const hasEmptyField = Object.values(shippingAddress).some(value => !value || value.trim() === "")

  const mutationPlaceOrder = useMutation({
    mutationFn: ({ customerId, street, city, zipCode }) =>
      placeOrder(customerId, street, city, zipCode),
    onSuccess: () => {
      console.log("order success");
    },
    onError: (err) => {
      console.log("order error: ", err);
    },
  });

  const handlePlaceOrder = async () => {
    if (hasEmptyField) return;
    if(basket.customerId === "");

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
        {/* ================= HEADER================= */}
        <div className="bg-white" style={{ marginBottom: "12px" }}>
          <CheckoutHeader />
        </div>
        <div
          role="main"
          className="container-wrapper mx-auto"
          style={{ fontSize: "14px", lineHeight: "16.8px" }}
        >
          {/* -------- SHIPPING ADDRESS -------- */}
          <ShippingAdress
            isOpen={open}
            onSetOpen={setOpen}
            address={shippingAddress}
            status={formValidated}
            onSetStatus={setFormValidated}
            onSubmitAddress={setShippingAddress}
          />
          {/* ================= MAIN CONTENT ================= */}
          <div style={{ marginTop: "12px", backgroundColor: "white" }}>
            <CheckoutMain items={basket?.items} />
          </div>
          {/* ================= FOOTER ================= */}
          <div className={s.checkoutFooterSection}>
            {/* -------- PAYMENT PROVIDER SECTION -------- */}
            <div className={s.checkoutFooterWithPaymentSection}>
              <Checkout
                items={PAYMENT_PROVIDERS}
                value={paymentMethod}
                onChange={setPaymnetMethod}
              />
            </div>
            <div className={s.checkoutFooter}>
              <h3 className={clsx(s.row, s.checkoutFooterTitle)}>
                Total Payment
              </h3>
              <div className={clsx(s.row, s.checkoutFooterTotalPrice)}>
                {formatCurrency(totalResult)}
              </div>
              <div className={clsx(s.row, s.checkoutFooterWithButtonOrder)}>
                <div></div>
                <button
                  onClick={() => {
                    handlePlaceOrder();
                  }}
                  className={s.checkoutFooterButtonOrder}
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
