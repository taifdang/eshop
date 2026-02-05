import clsx from "clsx";
import s from "./ShippingAddress.module.css";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { shippingAddressSchema } from "../../schema/shippingAddress.schema";
import { TextField } from "@/shared/components";
import { useEffect } from "react";

export default function ShippingAddressModal({
  isOpen,
  onSetOpen,
  address,
  onSubmitAddress,
  onCancel,
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
      <div className={s["shipping-address__modal-section"]}>
        <div className={s["shipping-address__modal"]}>
          <div className={s["shipping-address__modal-title"]}>New Address</div>
          <form onSubmit={handleSubmit(onSubmitAddress)}>
            <div className={s["shipping-address__modal-container"]}>
              <div className={s["shipping-address__form-content"]}>
                {/* FULLNAME AND PHONE NUMBER */}
                <div className="flex 100w">
                  <TextField
                    type="text"
                    label="Fullname"
                    placeholder="Fullname"
                    maxLength="64"
                    {...register("fullname")}
                    error={errors.fullname?.message}
                  />
                  <div className={s["shipping-address__spacer"]}></div>
                  <TextField
                    type="text"
                    label="Phone Number"
                    placeholder="Phone Number"
                    {...register("phoneNumber")}
                    error={errors.phoneNumber?.message}
                  />
                </div>
                {/* CITY AND ZIPCODE */}
                <div className="flex 100w">
                  <div className={clsx(s["shipping-address__col--main"])}>
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
                  <div className={s["shipping-address__spacer"]}></div>
                  <div className={clsx(s["shipping-address__col--sub"])}>
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
                {/* STREET */}
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
              {/* BUTTON GROUP SECTION */}
              <div className={s["shipping-address__button-section"]}>
                <button
                  className={s["shipping-address__button"]}
                  onClick={() => onCancel()}
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={!isValid || isSubmitting}
                  className={clsx(
                    s["shipping-address__button"],
                    s["shipping-address__button--submit"]
                  )}
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
