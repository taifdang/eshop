import CheckoutItem from "./CheckoutItem";
import { formatCurrency } from "../../../../../shared/lib/format";
import s from "./CheckoutSummary.module.css";
import clsx from "clsx";

export default function CheckoutSummary({ items = [] }) {
  const totalResult = items?.reduce(
    (sum, item) => (sum = sum + item.regularPrice * item.quantity),
    0,
  );

  return (
    <div>
      {/* HEADER */}
      <div
        className={clsx(
          s["checkout-content-table__row"],
          s["checkout-content__container"],
        )}
      >
        <div
          className={clsx(
            s["checkout-content-table__row"],
            s["checkout-content"],
          )}
        >
          <div
            className={clsx(
              s["checkout-content-table__col"],
              s["checkout-content-table__col--main"],
              "text-left",
            )}
          >
            <h2
              className={clsx(
                s["checkout-content-table__row"],
                s["checkout-content-table-header__text"],
              )}
            >
              Product Ordered
            </h2>
          </div>
          <div className={s["checkout-content-table__col--sub"]}></div>
          <div
            className={clsx(
              s["checkout-content-table__col--sub"],
              s["checkout-content-table-content__text"],
            )}
          >
            Unit Price
          </div>
          <div
            className={clsx(
              s["checkout-content-table__col--sub"],
              s["checkout-content-table-content__text"],
            )}
          >
            Amount
          </div>
          <div
            className={clsx(
              s["checkout-content-table__col--sub"],
              s["checkout-content-table-content__text"],
            )}
          >
            Item Subtotal
          </div>
        </div>
      </div>
      {/* ORDER SECTION */}
      <div className={s["checkout-list__section"]}>
        {/* ORDER ITEMS */}
        <div className={s["checkout-list"]}>
          {!items ? (
            <>null</>
          ) : (
            <>
              {items.map((item) => (
                <>
                  <CheckoutItem key={item.id} item={item} />
                </>
              ))}
            </>
          )}
        </div>
        {/* ORDER TOTALS */}
        <div className={s["checkout-total__section"]}>
          <div className={s["checkout-total"]}>
            <h3 className={s["checkout-total__title"]}>
              <div>Order Total ({items.length} Items):</div>
            </h3>
            <div className={s["checkout-total__price"]}>
              {formatCurrency(totalResult)}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
