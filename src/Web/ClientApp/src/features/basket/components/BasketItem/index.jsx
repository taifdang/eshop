import { useState } from "react";
import s from "./index.module.css";
import clsx from "clsx";
import fallbackImage from "@/assets/images/default.jpg";
import { formatCurrency } from "@/shared/lib/currency";

export default function BasketItem({ item, onUpdate }) {
  const [quantity, SetQuantity] = useState(1);

  const totalPrice = (price, quantity) => price * quantity;

  return (
    <>
      <div className={s["basket-item"]} role="listitem">
        <div className="flex items-center">
          {/*  */}
          <div className={clsx(s["div-checkbox"], s["table-col--checkbox"])}>
            <label htmlFor="">
              <input type="text" hidden />
              <div className={s["div-checkbox-wrap-input"]}></div>
            </label>
          </div>
          {/* product */}
          <div
            className={clsx("flex", s["table-col"], s["table-col--product"])}
          >
            <a href="#imageUrl">
              <img
                src={item.imageUrl || fallbackImage}
                className={s["product__image"]}
              />
            </a>
            <div className={s["product__info"]}>
              <a
                href="#productId"
                className={clsx("line-clamp-2", s["product__title"])}
              >
                {item.productName}
              </a>
            </div>
          </div>
          {/* produc-variant */}
          <div
            className={clsx(
              "items-center",
              s["table-col"],
              s["table-col--variant"]
            )}
          >
            {item.variantName && (
              <button type="button" className={s["product-variant-selection"]}>
                <div>Variants:</div>
                <div style={{ marginTop: "5px" }}>{item.variantName}</div>
              </button>
            )}
          </div>
          {/* price */}
          <div
            className={clsx(
              "flex items-center justify-center",
              s["table-col"],
              s["table-col--unit"]
            )}
          >
            {/* <span className={s["product__price"]}>105.000₫</span> */}
            <span>{formatCurrency(item.regularPrice)}</span>
          </div>
          {/* quantity */}
          <div
            className={clsx(
              "flex items-center justify-center",
              s["table-col"],
              s["table-col--quantity"]
            )}
          >
            <div className={s["quantity-selector__button-wrapper"]}>
              <button
                type="button"
                aria-label="Decrease"
                onClick={() => onUpdate(item.quantity - 1)}
                className={s["quantity-selector__button"]}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="currentColor"
                    d="M18 12.998H6a1 1 0 0 1 0-2h12a1 1 0 0 1 0 2"
                  />
                </svg>
              </button>
              <input
                aria-label="search-input"
                type="text"
                value={item.quantity}
                readOnly
                className={s["quantity-selector__input"]}
              />
              <button
                type="button"
                aria-label="Increase"
                onClick={() => onUpdate(item.quantity + 1)}
                className={s["quantity-selector__button"]}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="24"
                  height="24"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="currentColor"
                    d="M19 12.998h-6v6h-2v-6H5v-2h6v-6h2v6h6z"
                  />
                </svg>
              </button>
            </div>
          </div>
          {/* total price */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center",
              s["table-col"],
              s["table-col--total"]
            )}
          >
            <span>
              {formatCurrency(totalPrice(item.regularPrice, item.quantity))}
            </span>
          </div>
          {/* actions */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center",
              s["table-col"],
              s["table-col--actions"]
            )}
          >
            <button
              type="button"
              onClick={() => onUpdate(0)}
              style={{ padding: "1px 6px" }}
            >
              Delete
            </button>
          </div>
        </div>
      </div>
    </>
  );
}
