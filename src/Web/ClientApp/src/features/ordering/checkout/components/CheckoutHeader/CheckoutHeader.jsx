import s from "./CheckoutHeader.module.css";
export default function CheckoutHeader() {
  return (
    <div className={s["checkout-section"]}>
      <div className={s["checkout-section__header"]}>
        <div className={s["checkout-header"]}>
          <a className={s["checkout-header-brand"]} href="/">
            <div></div>
            <img
              src="src/assets/images/logo-brand-no-bg.png"
              width="162px"
              height="50px"
              alt=""
            />
            <div className={s["checkout-header-brand__title"]}>Checkout</div>
          </a>
        </div>
      </div>
    </div>
  );
}
