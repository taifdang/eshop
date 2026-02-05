import { RadioGroup } from "@/shared/components";
import styles from "./PaymentMethod.module.css";

export default function PaymentMethod({ items, value, onChange }) {
  return (
    <div className={styles["payment-method"]}>
      <div className={styles["payment-method__view"]}>
        <div className={styles["payment-method__title"]}>
          Payment Method
        </div>
        <div>
          <RadioGroup
            items={items}
            value={value}
            onChange={onChange}
            className={styles["payment-method__tabs"]}
            buttonClassName={styles["payment-method__option"]}
          />
        </div>
      </div>
    </div>
  );
}
