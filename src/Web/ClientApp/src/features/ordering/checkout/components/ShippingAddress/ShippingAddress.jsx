import { Modal } from "@/shared/components";
import s from "./ShippingAddress.module.css";
import clsx from "clsx";
import ShippingAddressModal from "./ShippingAddressModal";
import { useNavigate } from "react-router-dom";

export default function ShippingAddress({
  isOpen,
  onSetOpen,
  address,
  onSubmitAddress,
  status,
  onSetStatus,
}) {
  const navigate = useNavigate();

  // function
  const onSubmit = (data) => {
    onSetOpen(false);
    onSubmitAddress(data);
    onSetStatus(true);
  };

  const handleCancel = () => {
    if (status) {
      onSetOpen(false);
    } else {
      navigate("/cart");
    }
  };

  const { fullname, phoneNumber, city, zipCode, street } = address;

  return (
    <>
      <div className={s["shipping-address__separate"]}></div>
      {/* MAIN */}
      <div className={s["shipping-address-section"]}>
        {/* SECTION TITLE */}
        <div className="flex items-center">
          <div className={s["shipping-address__title-wrap"]}>
            <div className={clsx("flex", s["shipping-address__icon"])}>
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
        {/* SECTION WITH BUTTON */}
        <div className="flex items-center flex-wrap">
          <div
            className={clsx(
              "flex items-center",
              s["shipping-address__content"]
            )}
          >
            <div className={s["shipping-address__fullname"]}>
              {fullname} {phoneNumber}
            </div>
            <div className={s["shipping-address__address"]}>
              {city && <span>{city}</span>}
              {zipCode && <span>, {zipCode}</span>}
              {street && <span>, {street}</span>}
            </div>
          </div>
          <div className={s["shipping-address__change-btn-wrap"]}>
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
      {/* Modals */}
      <Modal open={isOpen}>
        <ShippingAddressModal
          isOpen={isOpen}
          onSetOpen={onSetOpen}
          address={address}
          onSubmitAddress={onSubmit}
          onCancel={handleCancel}
        />
      </Modal>
    </>
  );
}
