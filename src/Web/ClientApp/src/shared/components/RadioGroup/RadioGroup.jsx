import { forwardRef } from "react";
import Checkbox from "../Checkbox/Checkbox";

const RadioButton = forwardRef(
  ({ checked, onChange, children, className, ...props }, ref) => {
    return (
      <Checkbox
        ref={ref}
        checked={checked}
        onChange={onChange}
        className={className}
        role="radio"
        {...props}
      >
        {children}
      </Checkbox>
    );
  }
);

RadioButton.displayName = "RadioButton";

const RadioGroup = ({ items, value, onChange, className, buttonClassName }) => {
  return (
    <div role="radiogroup" className={className}>
      {items.map((item) => {
        const isSelected = value === item.id;
        return (
          <span key={item.id}>
            <RadioButton
              checked={isSelected}
              onChange={() => onChange(item.id)}
              className={buttonClassName}
            >
              {item.label}
            </RadioButton>
          </span>
        );
      })}
    </div>
  );
};

export { RadioButton, RadioGroup };
export default RadioGroup;
