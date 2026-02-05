import clsx from "clsx";
import s from "./BasketItem.module.css";
import { CardSkeleton, TextSkeleton } from "@/shared/components";

export const BasketItemSkeleton = () => {
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
            <CardSkeleton className={s["product__image"]} />
            <div className={s["product__info"]}>
              <TextSkeleton />
            </div>
          </div>
          {/* produc-variant */}
          <div
            className={clsx(
              "flex flex-col items-start",
              s["table-col"],
              s["table-col--variant"]
            )}
          >
            <TextSkeleton className="card-body" />
            <TextSkeleton className="card-body mt-[5px]" />
          </div>
          {/* price */}
          <div
            className={clsx(
              "flex items-center justify-center",
              s["table-col"],
              s["table-col--unit"]
            )}
          >
            <TextSkeleton className="card-body" />
          </div>
          {/* quantity */}
          <div className={clsx(s["table-col"], s["table-col--quantity"])}>
            <CardSkeleton className="card-input" />
          </div>
          {/* total price */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center",
              s["table-col"],
              s["table-col--total"]
            )}
          >
            <TextSkeleton className="card-body" />
          </div>
          {/* actions */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center ",
              s["table-col"],
              s["table-col--actions"]
            )}
          >
            <CardSkeleton className="card-body card-button" />
          </div>
        </div>
      </div>
    </>
  );
};
