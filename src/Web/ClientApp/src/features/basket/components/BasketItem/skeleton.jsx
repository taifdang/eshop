import clsx from "clsx";
import s from "./index.module.css";

export const Skeleton = () => {
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
            <div className={clsx(s["product__image"], "skeleton")}></div>
            <div className={s["product__info"]}>
              <div
                className={clsx(
                  "line-clamp-2 skeleton skeleton-text",
                  s["product__title"]
                )}
              ></div>
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
            <div className="skeleton skeleton-text skeleton-text__body"></div>
            <div
              className="skeleton skeleton-text"
              style={{ marginTop: "5px" }}
            ></div>
          </div>
          {/* price */}
          <div
            className={clsx(
              "flex items-center justify-center",
              s["table-col"],
              s["table-col--unit"]
            )}
          >
            <span className="skeleton skeleton-text skeleton-text__body"></span>
          </div>
          {/* quantity */}
          <div
            className={clsx(
              "flex relative items-center justify-center",
              s["table-col"],
              s["table-col--quantity"]
            )}
          >
            <div
              className={clsx(
                "skeleton skeleton-input",
                s[""]
              )}
            ></div>
          </div>
          {/* total price */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center",
              s["table-col"],
              s["table-col--total"]
            )}
          >
            <span className="skeleton skeleton-text skeleton-text__body"></span>
          </div>
          {/* actions */}
          <div
            className={clsx(
              "flex flex-col justify-center items-center ",
              s["table-col"],
              s["table-col--actions"]
            )}
          >
            <button
              type="button"
              className="skeleton skeleton-text__body skeleton-button"
            ></button>
          </div>
        </div>
      </div>
    </>
  );
};
