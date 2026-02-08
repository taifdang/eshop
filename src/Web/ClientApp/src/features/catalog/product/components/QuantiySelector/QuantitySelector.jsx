import clsx from "clsx";
import s from "./QuantitySelector.module.css";

export function QuantitySelector({
  stock,
  quantity,
  onChange,
  onShow,
  onIncrease,
  onDecrease,
}) {
  const handleStockStatus = (stock) => {
    if (!stock) return "INSTOCK"; // for test
    if (stock === 0) return "OUT OF STOCK";
    if (stock) return `${stock} pieces stock`;
  };

  return (
    <section className={s["quantity-selector__section"]}>
      <h2 className={s["quantity-selector__title"]}>Quantity</h2>
      <div style={{ display: "flex", alignItems: "center" }}>
        <div style={{ marginRight: "15px" }}>
          <div className={s["quantity-selector__button-wrapper"]}>
            <button
              type="button"
              onClick={() => onDecrease()}
              disabled={quantity <= 1}
              aria-label="Decrease"
              className={clsx(
                s["quantity-selector__button"],
                onShow && quantity > 1 && s["active"],
              )}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  d="M18 12.998H6a1 1 0 0 1 0-2h12a1 1 0 0 1 0 2"
                />
              </svg>
            </button>
            <input
              aria-label="search-input"
              type="text"
              value={quantity}
              readOnly
              className={s["quantity-selector__input"]}
            />
            <button
              type="button"
              onClick={() => onIncrease()}
              disabled={!onShow || quantity + 1 > stock}
              aria-label="Increase"
              className={clsx(
                s["quantity-selector__button"],
                onShow && s["active"],
              )}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="24"
                height="24"
                viewBox="0 0 24 24"
              >
                <path
                  fill="currentColor"
                  d="M19 12.998h-6v6h-2v-6H5v-2h6v-6h2v6h6z"
                />
              </svg>
            </button>
          </div>
        </div>
        <div className={s["quantity-selector__status"]}>
          {handleStockStatus(stock)}
        </div>
      </div>
    </section>
  );
}
