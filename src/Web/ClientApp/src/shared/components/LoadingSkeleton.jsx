import clsx from "clsx";
export function Skeleton({ className, ...props }) {
  return <div className={clsx("card-skeleton", className)} {...props} />;
}

export function PageLoadingSkeleton() {
  return (
    <>
      <div className="flex items-center justify-center min-h-screen text-gray-600 text-lg">
        <span className="ml-2 flex items-end gap-1">
          <span className="dot-wave step-1" />
          <span className="dot-wave step-2" />
          <span className="dot-wave step-3" />
        </span>
      </div>
    </>
  );
}
