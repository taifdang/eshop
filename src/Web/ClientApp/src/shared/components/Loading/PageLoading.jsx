import s from "./Loading.module.css";
import clsx from "clsx";

export function PageLoading() {
  return (
    <div className="flex items-center justify-center min-h-screen text-gray-600 text-lg">
      <span className="ml-2 flex items-end gap-1">
        <span className={clsx(s["dot-wave"], s["dot-1"])} />
        <span className={clsx(s["dot-wave"], s["dot-2"])} />
        <span className={clsx(s["dot-wave"], s["dot-3"])} />
      </span>
    </div>
  );
}
