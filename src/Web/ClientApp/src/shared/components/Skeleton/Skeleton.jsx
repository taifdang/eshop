import clsx from "clsx";
import s from "./Skeleton.module.css";

export function Skeleton({ className, ...props }) {
  return <div className={clsx(s["skeleton"], className)} {...props} />;
}
