import clsx from "clsx";
export default function Checkout({ items, value, onChange }) {
  return (
    <div style={{ display: "contents" }}>
      <div className="checkout-payment-method-main">
        <div className="checkout-payment-method-view__current">
          <div className="checkout-payment-method-view__current-title">
            Payment Method
          </div>
          <div className="checkout-payment-setting__payment-methods-tab">
            <div role="radiogroup">
              {items.map((p) => {
                const __selected = value === p.id;
                return (
                  <span key={p.id}>
                    <button
                      className={clsx(
                        "product-variation option-box-container",
                        __selected
                          ? "selection-box-selected"
                          : "selection-box-unselected"
                      )}
                      aria-checked={__selected}
                      role="radio"
                      onClick={() => onChange(p.id)}
                    >
                      {p.label}
                      {__selected && <div className="selection-box-tick"></div>}
                    </button>
                  </span>
                );
              })}
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
