import clsx from "clsx";
import s from "../Checkout.module.css";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { shippingAddressSchema } from "../utils/shippingAddress.schema";
import { TextField } from "@/shared/components/TextField/TextField";
import { useEffect } from "react";

export default function ShippingAddressModal({
  isOpen,
  onSetOpen,
  address,
  onSubmitAddress,
  onCancel
}) {
  // REACT HOOK FORM
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isValid, isSubmitting },
  } = useForm({
    resolver: zodResolver(shippingAddressSchema),
    mode: "onBlur",
    reValidateMode: "onChange",
  });
  // SIDE-EFFECT
  useEffect(() => {
    if (isOpen) {
      reset(address);
    }
  }, [isOpen, address, reset]);

  return (
    <div className="z-1">
      <div className={s.shippingAddressModalSection}>
        <div className={s.shippingAddressModal}>
          <div className={s.shippingAddressModalTitle}>New Address</div>
          <form onSubmit={handleSubmit(onSubmitAddress)}>
            <div className={s.modalFormContainer}>
              <div className={s.modalFormContent}>
                {/* -------- FULLNAME AND PHONE NUMBER  -------- */}
                <div className="flex 100w">
                  <TextField
                    type="text"
                    label="Fullname"
                    placeholder="Fullname"
                    maxLength="64"
                    {...register("fullname")}
                    error={errors.fullname?.message}
                  />
                  <div style={{ width: "16px" }}></div>
                  <TextField
                    type="text"
                    label="Phone Number"
                    placeholder="Phone Number"
                    {...register("phoneNumber")}
                    error={errors.phoneNumber?.message}
                  />
                </div>
                {/* -------- CITY AND ZIPCODE  -------- */}
                <div className="flex 100w">
                  <div className={clsx(s.colMain)}>
                    <TextField
                      type="text"
                      label="City"
                      placeholder="City"
                      maxLength="128"
                      autoComplete=""
                      {...register("city")}
                      error={errors.city?.message}
                    />
                  </div>
                  <div style={{ width: "16px" }}></div>
                  <div className={clsx(s.colSub)}>
                    <TextField
                      type="text"
                      label="zipCode"
                      placeholder="zipCode"
                      maxLength="128"
                      autoComplete=""
                      {...register("zipCode")}
                      error={errors.zipCode?.message}
                    />
                  </div>
                </div>
                {/* -------- STREET  -------- */}
                <div className="flex 100w">
                  <TextField
                    type="text"
                    label="Street"
                    placeholder="Street"
                    maxLength="128"
                    autoComplete=""
                    {...register("street")}
                    error={errors.street?.message}
                  />
                </div>
              </div>
              {/* -------- BUTTON GROUP SECTION  -------- */}
              <div className={s.modalFormButtonSection}>
                <button
                  className={s.modalFormButton}
                  onClick={() => onCancel()}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={!isValid || isSubmitting}
                  className={clsx(s.modalFormButton, s.modalFormButtonSubmit)}
                >
                  Submit
                </button>
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
}
