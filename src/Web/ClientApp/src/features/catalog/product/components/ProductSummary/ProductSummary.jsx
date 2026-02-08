import { ProductText } from "./ProductText";
import { ProductPrice } from "./ProductPrice";

export function ProductSummary({ price, name, isPriceLoading }) {
  return (
    <div>
      <ProductText name={name} />
      <ProductPrice price={price} isPriceLoading={isPriceLoading} />
    </div>
  );
}
