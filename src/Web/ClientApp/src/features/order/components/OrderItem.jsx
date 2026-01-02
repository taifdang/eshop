import clsx from "clsx";
import s from "../Checkout.module.css";
import fallbackImage from "@/assets/images/default.jpg";
import { formatCurrency } from "../../../shared/lib/currency";

export default function OrderItem({ item }) {
  const totalPrice = (price, quantity) => price * quantity;

  return (
    <div className={s.orderItem}>
      <div className={clsx(s.col, s.colMain)}>
        <img
          src={item.imageUrl || fallbackImage}
          width="40px"
          height="40px"
          className="object-contain"
        />
        <span className={s.orderItemName}>
          <span className={clsx("ellipsis")}>{item.productName}</span>
        </span>
      </div>
      <div className={clsx(s.colSub, s.orderItemVariant)}>
        {item.variantName && (
          <span className={clsx("ellipsis", s.orderItemVariantTitle)}>
            Variation: Caro Xanh Nước Đậm,Chăn Đũi 1m8x2m
          </span>
        )}
      </div>
      <div className={s.colSub}>{formatCurrency(item.regularPrice)}</div>
      <div className={s.colSub}>{item.quantity}</div>
      <div className={s.colSub}>
        {formatCurrency(totalPrice(item.regularPrice, item.quantity))}
      </div>
    </div>
  );
}

//  return (
//     <div className={s.orderItem}>
//       <div className={clsx(s.col, s.colMain)}>
//         <img
//           src={fallbackImage}
//           width="40px"
//           height="40px"
//           className="object-contain"
//         />
//         <span className={s.orderItemName}>
//           <span className={clsx("ellipsis")}>
//             Lorem ipsum dolor sitLorem ipsum dolor sitLorem sitLorem ipsum dolor
//             sitLorem
//           </span>
//         </span>
//       </div>
//       <div className={clsx(s.colSub, s.orderItemVariant)}>
//         <span className={clsx("ellipsis", s.orderItemVariantTitle)}>
//           Variation: Caro Xanh Nước Đậm,Chăn Đũi 1m8x2m
//         </span>
//       </div>
//       <div className={s.colSub}>0₫</div>
//       <div className={s.colSub}>0</div>
//       <div className={s.colSub}>0₫</div>
//     </div>
//   );
