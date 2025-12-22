import styles from "../Checkout.module.css";
export default function CheckoutHeader() {
  return (
    <div className={styles.checkoutSection}>
      <div className={styles.checkoutHeaderMain}>
        <div className={styles.checkoutHeader}>
          <a className={styles.checkoutBrand} href="/">
            <div></div>
            <img
              src="src/assets/images/logo-brand-no-bg.png"
              width="162px"
              height="50px"
              alt=""
            />
            <div className={styles.checkoutBrandName}>Checkout</div>
          </a>
        </div>
      </div>
    </div>
  );
}
