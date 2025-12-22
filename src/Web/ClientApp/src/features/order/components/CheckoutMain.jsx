import clsx from "clsx";
import s from "../Checkout.module.css";
import OrderItem from "./OrderItem";
export default function CheckoutMain() {
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
          <OrderItem />
          <OrderItem />
          <OrderItem />
          <OrderItem />
          <OrderItem />
        </div>
        {/* ORDER TOTALS */}
        <div className={s.orderTotalSection}>
          <div className={s.orderTotal}>
            <h3 className={s.orderTotalTitle}>
              <div>Order Total (0 Items):</div>
            </h3>
            <div className={s.orderTotalPrice}>0₫</div>
          </div>
        </div>
      </div>
    </div>
  );
}
