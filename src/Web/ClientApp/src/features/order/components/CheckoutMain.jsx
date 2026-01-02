import clsx from "clsx";
import s from "../Checkout.module.css";
import OrderItem from "./OrderItem";
import { formatCurrency } from "../../../shared/lib/currency";
export default function CheckoutMain({ items }) {

  const totalResult = items?.reduce((sum, item) => (sum = sum + item.regularPrice * item.quantity),0)

  return (
    <div>
      {/* HEADER */}
      <div className={clsx(s.row, s.checkoutContent)}>
        <div className={clsx(s.row, s.checkoutContentContainer)}>
          <div className={clsx(s.col, s.colMain, "text-left")}>
            <h2 className={clsx(s.row, s.headerText)}>Product Ordered</h2>
          </div>
          <div className={s.colSub}></div>
          <div className={clsx(s.colSub, s.contentText)}>Unit Price</div>
          <div className={clsx(s.colSub, s.contentText)}>Amount</div>
          <div className={clsx(s.colSub, s.contentText)}>Item Subtotal</div>
        </div>
      </div>
      {/* ORDER SECTION */}
      <div className={s.orderListSection}>
        {/* ORDER ITEMS */}
        <div className={s.orderList}>
          {!items ? (
            <>null</>
          ) : (
            <>
              {items.map((item) => (
                <>
                  <OrderItem key={item.id} item={item} />
                </>
              ))}
            </>
          )}
        </div>
        {/* ORDER TOTALS */}
        <div className={s.orderTotalSection}>
          <div className={s.orderTotal}>
            <h3 className={s.orderTotalTitle}>
              <div>Order Total ({items.length} Items):</div>
            </h3>
            <div className={s.orderTotalPrice}>{formatCurrency(totalResult)}</div>
          </div>
        </div>
      </div>
    </div>
  );
}
