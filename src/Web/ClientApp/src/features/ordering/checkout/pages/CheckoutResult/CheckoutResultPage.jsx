import { useSearchParams } from "react-router-dom";
import s from "./CheckoutResultPage.module.css";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { fetchCheckoutOrder } from "../../services/order-service";

export const CheckoutResultPage = () => {
  const navigate = useNavigate();
  const [searchParams] = useSearchParams();

  const [showLoading, setShowLoading] = useState(true);
  const [showError, setShowError] = useState(false);

  const handleRedirect = () => {
    navigate("/");
  };

  const orderNumber = searchParams.get("orderNumber") ?? "";

  useEffect(() => {
    if (!orderNumber) {
      setShowError(true);
      return;
    }

    const loadCheckoutOrder = async () => {
      try {
        const res = await fetchCheckoutOrder(orderNumber);
        if (res.status !== 200) {
          setShowError(true);
          return;
        }
        console.log("checkoutResult", res.data);
      } catch (err) {
        setShowError(true);
        return;
      }
    };
    loadCheckoutOrder();
  }, [orderNumber]);

  // page load
  useEffect(() => {
    setShowLoading(true);
    const timer = setTimeout(() => {
      setShowLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, []);

  if (showLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen text-gray-600 text-lg">
        <span className="ml-2 flex items-end gap-1">
          <span className="dot-wave step-1" />
          <span className="dot-wave step-2" />
          <span className="dot-wave step-3" />
        </span>
      </div>
    );
  }

  if (showError) {
    return (
      <>
        <div className={s["w4p-container"]}>
          <div className={s["w4p-wrapper"]}>
            <div className={s["w4p-box"]}>
              <div className={s["w4p-box__subtitle"]}>
                <p>
                  The payment was not successful. Please try again or choose a
                  different payment method.
                </p>
              </div>
              <div className="flex w-100">
                <button
                  onClick={() => handleRedirect()}
                  className={s["w4p__button"]}
                >
                  OK, got it
                </button>
              </div>
            </div>
          </div>
        </div>
      </>
    );
  }

  return (
    <>
      <div className={s["w4p-container"]}>
        <div className={s["w4p-wrapper"]}>
          <div className={s["w4p-box"]}>
            <div className="flex items-center justify-center">
              <div>
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width={24}
                  height={24}
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="currentColor"
                    fillRule="evenodd"
                    d="M22 12c0-5.523-4.477-10-10-10S2 6.477 2 12s4.477 10 10 10s10-4.477 10-10M12 6.25a.75.75 0 0 1 .75.75v6a.75.75 0 0 1-1.5 0V7a.75.75 0 0 1 .75-.75M12 17a1 1 0 1 0 0-2a1 1 0 0 0 0 2"
                    clipRule="evenodd"
                  ></path>
                </svg>
              </div>
              <div className={s["w4p-box__title"]}>Waiting for payment</div>
            </div>
            <div className={s["w4p-box__subtitle"]}>
              <p>
                Your order has been created. The order status updates
                automatically. Track your order in{" "}
                <a href="/" style={{ textDecoration: "underline" }}>
                  My Orders
                </a>
                .
              </p>
            </div>
            <div className="flex w-100">
              <button
                onClick={() => handleRedirect()}
                className={s["w4p__button"]}
              >
                Home
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
