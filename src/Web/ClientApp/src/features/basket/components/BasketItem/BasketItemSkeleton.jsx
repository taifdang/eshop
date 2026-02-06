import s from "./BasketItem.module.css";
import {
  CardSkeleton,
  TextSkeleton,
  TableRow,
  TableCell,
} from "@/shared/components";

export const BasketItemSkeleton = () => {
  return (
    <div className={s["basket-item"]} role="listitem">
      <TableRow>
        {/* Checkbox */}
        <TableCell className={s["div-checkbox"]} flex="0 0 58px">
          <label htmlFor="">
            <input type="text" hidden />
            <div className={s["div-checkbox-wrap-input"]}></div>
          </label>
        </TableCell>

        {/* Product */}
        <TableCell flex="3.5">
          <CardSkeleton className={s["product__image"]} />
          <div className={s["product__info"]}>
            <TextSkeleton />
          </div>
        </TableCell>

        {/* Product Variant */}
        <TableCell
          className={"flex-col items-start"}
          flex="1.75"
          align="center"
        >
          <TextSkeleton className="card-body" />
          <TextSkeleton className="card-body mt-[5px]" />
        </TableCell>

        {/* Unit Price */}
        <TableCell flex="2" align="center">
          <TextSkeleton className="card-body" />
        </TableCell>

        {/* Quantity */}
        <TableCell flex="2" align="center">
          <CardSkeleton className="card-input" />
        </TableCell>

        {/* Total Price */}
        <TableCell flex="1.5" align="center">
          <TextSkeleton className="card-body" />
        </TableCell>

        {/* Actions */}
        <TableCell flex="1.75" align="center">
          <CardSkeleton className="card-body card-button" />
        </TableCell>
      </TableRow>
    </div>
  );
};
