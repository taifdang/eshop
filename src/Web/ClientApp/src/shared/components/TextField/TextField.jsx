import s from "./TextField.module.css";
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
    <div className={s['text-field__container']}>
      <div className={s['text-field']}>
        <div className={s['text-field__label']}>{label}</div>
        <input
          className={clsx(s['text-field__input'])}
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
      {error && <span className={s['text-field__error']}>{error}</span>}
    </div>
  );
};
