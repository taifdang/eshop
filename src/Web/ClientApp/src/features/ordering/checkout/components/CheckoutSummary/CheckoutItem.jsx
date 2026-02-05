import clsx from "clsx";
import s from "./CheckoutSummary.module.css";
import fallbackImage from "@/assets/images/default.jpg";
import { formatCurrency } from "../../../../../shared/lib/format";

export default function CheckoutItem({ item }) {
  const totalPrice = (price, quantity) => price * quantity;

  return (
    <div className={s["checkout-item"]}>
      <div className={clsx(s["checkout-content-table__col"], s["checkout-content-table__col--main"])}>
        <img
          src={item.imageUrl || fallbackImage}
          width="40px"
          height="40px"
          className="object-contain"
        />
        <span className={s["checkout-item__title"]}>
          <span className={clsx("ellipsis")}>{item.productName}</span>
        </span>
      </div>
      <div className={clsx(s["checkout-content-table__col--sub"], s["checkout-item-variant"])}>
        {item.variantName && (
          <span className={clsx("ellipsis", s["checkout-item-variant__title"])}>
            Variation: {item.variantName}
          </span>
        )}
      </div>
      <div className={s["checkout-content-table__col--sub"]}>{formatCurrency(item.regularPrice)}</div>
      <div className={s["checkout-content-table__col--sub"]}>{item.quantity}</div>
      <div className={s["checkout-content-table__col--sub"]}>
        {formatCurrency(totalPrice(item.regularPrice, item.quantity))}
      </div>
    </div>
  );
}