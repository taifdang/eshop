import Modal from "@/shared/components/Modal";
import s from "../Checkout.module.css";
import clsx from "clsx";
import ShippingAddressModal from "./ShippingAddressModal";

export default function ShippingAdress({
  isOpen,
  onSetOpen,
  address,
  onSubmitAddress,
}) {
  // FUNCTIONS
  const onSubmit = (data) => {
    onSetOpen(false);
    onSubmitAddress(data);
    console.log(JSON.stringify(data));
  };

  const { fullname, phoneNumber, city, zipCode, street } = address;

  return (
    <>
      <div className={s["shipping-address__separate"]}></div>
      {/* ================= MAIN ================= */}
      <div className={s["shipping-address-section"]}>
        {/* -------- SECTION TITLE  -------- */}
        <div className="flex items-center">
          <div className={s["shipping-address__title-wrap"]}>
            <div className="flex" style={{ marginRight: "8px" }}>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="20"
                height="20"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  d="M12 2c-4.2 0-8 3.22-8 8.2c0 3.18 2.45 6.92 7.34 11.23c.38.33.95.33 1.33 0C17.55 17.12 20 13.38 20 10.2C20 5.22 16.2 2 12 2m0 10c-1.1 0-2-.9-2-2s.9-2 2-2s2 .9 2 2s-.9 2-2 2"
                />
              </svg>
            </div>
            <h2 className={s["shipping-address__title"]}>Delivery Address</h2>
          </div>
        </div>
        {/* -------- SECTION WITH BUTTON  -------- */}
        <div className="flex items-center flex-wrap">
          <div
            className={clsx(
              "flex items-center",
              s["shipping-address__content"]
            )}
          >
            <div style={{ fontWeight: 700 }}>
              {fullname} {phoneNumber}
            </div>
            <div style={{ marginLeft: "20px" }}>
              {city && <span>{city}</span>}
              {zipCode && <span>, {zipCode}</span>}
              {street && <span>, {street}</span>}
            </div>
          </div>
          <div style={{ marginLeft: "40px" }}>
            <button
              className={s["shipping-address__button"]}
              type="button"
              onClick={() => onSetOpen(true)}
            >
              <span>Change</span>
            </button>
          </div>
        </div>
      </div>
      {/* ================= MODAL ================= */}
      <Modal open={isOpen}>
        <ShippingAddressModal
          isOpen={isOpen}
          onSetOpen={onSetOpen}
          address={address}
          onSubmitAddress={onSubmit}
        />
      </Modal>
    </>
  );
}
