import clsx from "clsx";
import s from "./BasketItem.module.css";
import { Skeleton } from "@/shared/components/LoadingSkeleton";

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
            <Skeleton className={s["product__image"]} />
            <div className={s["product__info"]}>
              <Skeleton className={"card-text"} />
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
            <Skeleton className="card-text card-body" />
            <Skeleton className="card-text card-body mt-[5px]" />
          </div>
          {/* price */}
          <div
            className={clsx(
              "flex items-center justify-center",
              s["table-col"],
              s["table-col--unit"]
            )}
          >
            <Skeleton className="card-text card-body" />
          </div>
          {/* quantity */}
          <div className={clsx(s["table-col"], s["table-col--quantity"])}>
            <Skeleton className="card-input" />
          </div>
          {/* total price */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center",
              s["table-col"],
              s["table-col--total"]
            )}
          >
            <Skeleton className="card-text card-body" />
          </div>
          {/* actions */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center ",
              s["table-col"],
              s["table-col--actions"]
            )}
          >
            <Skeleton className="card-body card-button" />
          </div>
        </div>
      </div>
    </>
  );
};
