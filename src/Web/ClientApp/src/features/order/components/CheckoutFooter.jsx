import clsx from "clsx";
import s from "../Checkout.module.css";
import Checkout from "@/shared/components/Checkout";
export default function CheckoutFooter() {
  return (
    <div className={s.checkoutFooterSection}>
      <div className={s.checkoutFooterWithPaymentSection}>
        <div className={clsx(s.row, s.checkoutFooterWithPayment)}>
          <div className={s.checkoutFooterPaymentMethodTitle}>Payment Method</div>
          <div>Cash on Delivery</div>
          <button className={s.checkoutFooterPaymentButton}>
            <span>CHANGE</span>
          </button>
        </div>
        {/* <Checkout/> */}
      </div>
      <div className={s.checkoutFooter}>
        <h3 className={clsx(s.row, s.checkoutFooterTitle)}>Total Payment</h3>
        <div className={clsx(s.row, s.checkoutFooterTotalPrice)}>231.499₫</div>
        <div className={clsx(s.row, s.checkoutFooterWithButtonOrder)}>
          <div></div>
          <button className={s.checkoutFooterButtonOrder}>Place Order</button>
        </div>
      </div>
    </div>
  );
}
