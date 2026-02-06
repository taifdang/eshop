import CheckoutItem from "./CheckoutItem";
import { formatCurrency } from "@shared/lib/format";
import s from "./CheckoutSummary.module.css";
import clsx from "clsx";
import { Table, TableHeader, TableHeaderCell, TableBody } from "@/shared/components";

export default function CheckoutSummary({ items = [] }) {
  const totalResult = items?.reduce(
    (sum, item) => (sum = sum + item.regularPrice * item.quantity),
    0,
  );

  return (
    <div>
      {/* Table Container */}
      <div className={clsx(s["checkout-content-table__row"], s["checkout-content__container"])}>
        <Table className={clsx(s["checkout-content-table__row"], s["checkout-content"])}>
          {/* Table Header */}
          <TableHeader>
            <TableHeaderCell
              className={clsx(s["checkout-content-table__col--main"], "text-left")}
              flex="4 1 0%"
            >
              <h2 className={clsx(s["checkout-content-table__row"], s["checkout-content-table-header__text"])}>
                Product Ordered
              </h2>
            </TableHeaderCell>
            <TableHeaderCell className={s["checkout-content-table__col--sub"]} flex="2 1 0%" align="right">
            </TableHeaderCell>
            <TableHeaderCell
              className={clsx(s["checkout-content-table__col--sub"], s["checkout-content-table-content__text"])}
              flex="2 1 0%"
              align="right"
            >
              Unit Price
            </TableHeaderCell>
            <TableHeaderCell
              className={clsx(s["checkout-content-table__col--sub"], s["checkout-content-table-content__text"])}
              flex="2 1 0%"
              align="right"
            >
              Amount
            </TableHeaderCell>
            <TableHeaderCell
              className={clsx(s["checkout-content-table__col--sub"], s["checkout-content-table-content__text"])}
              flex="2 1 0%"
              align="right"
            >
              Item Subtotal
            </TableHeaderCell>
          </TableHeader>
        </Table>
      </div>
      
      {/* ORDER SECTION */}
      <div className={s["checkout-list__section"]}>
        {/* ORDER ITEMS */}
        <TableBody className={s["checkout-list"]}>
          {!items ? (
            <>null</>
          ) : (
            <>
              {items.map((item) => (
                <CheckoutItem key={item.id} item={item} />
              ))}
            </>
          )}
        </TableBody>
        
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
