import s from "../TextField/index.module.css";
import clsx from "clsx";

export const TextField = ({
  label,
  name,
  value,
  onChange,
  placeholder,
  type = "text",
  maxLength,
  autoComplete,
  error,
  className,
  ...rest
}) => {
  return (
    <div className={s.textFieldContainer}>
      <div className={s.textField}>
        <div className={s.textFieldLabel}>{label}</div>
        <input
          className={clsx(s.textFieldInput)}
          type={type}
          name={name}
          value={value}
          onChange={onChange}
          placeholder={placeholder}
          maxLength={maxLength}
          autoComplete={autoComplete}
          {...rest}
        />
      </div>
      {error && <span className={s.textFieldInputError}>{error}</span>}
    </div>
  );
};
