import clsx from "clsx";
import s from "./CheckoutSummary.module.css";
import fallbackImage from "@/assets/images/default.jpg";
import { formatCurrency } from "../../../../../shared/lib/format";
import { TableRow, TableCell } from "@/shared/components";

export default function CheckoutItem({ item }) {
  const totalPrice = (price, quantity) => price * quantity;

  return (
    <div className={s["checkout-item"]}>
      <TableRow>
        {/* Product with Image and Name */}
        <TableCell 
        className={clsx(s["checkout-content-table__col--main"])} 
        flex="4 1 0%"
      >
        <img
          src={item.imageUrl || fallbackImage}
          width="40px"
          height="40px"
          className="object-contain"
          alt={item.productName}
        />
        <span className={s["checkout-item__title"]}>
          <span className={clsx("ellipsis")}>{item.productName}</span>
        </span>
      </TableCell>
      
      {/* Variant */}
      <TableCell 
        className={clsx(s["checkout-content-table__col--sub"], s["checkout-item-variant"])} 
        flex="2 1 0%"
        align="right"
      >
        {item.variantName && (
          <span className={clsx("ellipsis", s["checkout-item-variant__title"])}>
            Variation: {item.variantName}
          </span>
        )}
      </TableCell>
      
      {/* Unit Price */}
      <TableCell 
        className={s["checkout-content-table__col--sub"]} 
        flex="2 1 0%"
        align="right"
      >
        {formatCurrency(item.regularPrice)}
      </TableCell>
      
      {/* Quantity */}
      <TableCell 
        className={s["checkout-content-table__col--sub"]} 
        flex="2 1 0%"
        align="right"
      >
        {item.quantity}
      </TableCell>
      
      {/* Item Subtotal */}
      <TableCell 
        className={s["checkout-content-table__col--sub"]} 
        flex="2 1 0%"
        align="right"
      >
        {formatCurrency(totalPrice(item.regularPrice, item.quantity))}
      </TableCell>
      </TableRow>
    </div>
  );
}