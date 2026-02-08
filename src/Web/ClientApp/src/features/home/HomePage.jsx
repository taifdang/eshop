import { ProductList } from "../catalog/product/components";
import s from "./HomePage.module.css";
import clsx from "clsx";

export default function HomePage() {
  return (
    <div>
      <div className={s["layout-main"]}>
        <div className={clsx(s["suggest"], "container-wrapper")}>
          <h1 className={s["suggest__label"]}>DAILY DISCOVER</h1>
          <hr className={s["suggest__divider"]} />
        </div>
        {/*  Main product list */}
        <div className={s["layout-section"]}>
          <ProductList />
        </div>
      </div>
    </div>
  );
}
