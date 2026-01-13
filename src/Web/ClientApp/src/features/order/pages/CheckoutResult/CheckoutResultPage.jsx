import { useSearchParams } from "react-router-dom";
import s from "./CheckoutResultPage.module.css";
import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

export const CheckoutResultPage = () => {
  const [showLoading, setShowLoading] = useState(true);
  const navigate = useNavigate();

  const [searchParams] = useSearchParams();
  const status = searchParams.get("status") ?? "failure";

  const ORDER_RESULT = [
    {
      status: "confirm",
      titlle: "Confirmation!",
      subtitle: "Everything is working normally.",
      buttonText: "Continue",
    },
    {
      status: "failure",
      titlle: "Error!",
      subtitle: (
        <>
          Oops! <br /> Something went wrong!
        </>
      ),
      buttonText: "Ok, got it",
    },
  ];

  const handleRedirect = () => {
    navigate("/");
  };

  const result =
    ORDER_RESULT.find((x) => x.status === status) ??
    ORDER_RESULT.find((x) => x.status === "failure");

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
              <button
                onClick={() => handleRedirect()}
                className={s["w4p__button"]}
              >
                Order
              </button>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
{
  /* <div className={s["container"]}>
  <div className={s["wrapper"]}>
    <div className={s["box"]}>
      <div className={clsx(s["box__icon"], s[`box__icon--${result.status}`])}>
        {result.status === "failure" ? (
          <>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width={150}
              height={150}
              viewBox="0 0 24 24"
            >
              <path
                fill="currentColor"
                fillRule="evenodd"
                d="M22 12c0 5.523-4.477 10-10 10S2 17.523 2 12S6.477 2 12 2s10 4.477 10 10M8.97 8.97a.75.75 0 0 1 1.06 0L12 10.94l1.97-1.97a.75.75 0 0 1 1.06 1.06L13.06 12l1.97 1.97a.75.75 0 0 1-1.06 1.06L12 13.06l-1.97 1.97a.75.75 0 0 1-1.06-1.06L10.94 12l-1.97-1.97a.75.75 0 0 1 0-1.06"
                clipRule="evenodd"
              ></path>
            </svg>
          </>
        ) : (
          <>
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width={150}
              height={150}
              viewBox="0 0 24 24"
            >
              <path
                fill="currentColor"
                fillRule="evenodd"
                d="M22 12c0 5.523-4.477 10-10 10S2 17.523 2 12S6.477 2 12 2s10 4.477 10 10m-5.97-3.03a.75.75 0 0 1 0 1.06l-5 5a.75.75 0 0 1-1.06 0l-2-2a.75.75 0 1 1 1.06-1.06l1.47 1.47l2.235-2.235L14.97 8.97a.75.75 0 0 1 1.06 0"
                clipRule="evenodd"
              ></path>
            </svg>
          </>
        )}
      </div>
      <div className={s["box__title"]}>{result.titlle}</div>
      <div className={s["box__subtitle"]}>{result.subtitle}</div>
      <button onClick={() => handleRedirect()} className={s["box__button"]}>
        <span className={s["box__button--text"]}>{result.buttonText}</span>
      </button>
    </div>
  </div>
</div>; */
}
