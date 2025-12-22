import clsx from "clsx";
import s from "../Checkout.module.css";
export default function OrderItem() {
  return (
    <div className={s.orderItem}>
      <div className={clsx(s.col, s.colMain)}>
        <img
          src="src/assets/images/default.jpg"
          width="40px"
          height="40px"
          className="object-contain"
        />
        <span className={s.orderItemName}>
          <span className={clsx("ellipsis")}>
            Lorem ipsum dolor sitLorem ipsum dolor sitLorem sitLorem ipsum dolor
            sitLorem
          </span>
        </span>
      </div>
      <div className={clsx(s.colSub, s.orderItemVariant)}>
        <span className={clsx("ellipsis", s.orderItemVariantTitle)}>
          Variation: Caro Xanh Nước Đậm,Chăn Đũi 1m8x2m
        </span>
      </div>
      <div className={s.colSub}>0₫</div>
      <div className={s.colSub}>0</div>
      <div className={s.colSub}>0₫</div>
    </div>
  );
}
