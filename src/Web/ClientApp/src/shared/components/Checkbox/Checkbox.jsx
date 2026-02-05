import { forwardRef } from "react";
import styles from "./Checkbox.module.css";
import clsx from "clsx";

const Checkbox = forwardRef(
  ({ checked, onChange, children, className, ...props }, ref) => {
    return (
      <button
        ref={ref}
        type="button"
        role="checkbox"
        aria-checked={checked}
        onClick={onChange}
        className={clsx(
          styles.checkbox,
          checked ? styles["checkbox--checked"] : styles["checkbox--unchecked"],
          className,
        )}
        {...props}
      >
        {children}
        {checked && <div className={styles["checkbox__icon"]}></div>}
      </button>
    );
  },
);

Checkbox.displayName = "Checkbox";

export default Checkbox;
