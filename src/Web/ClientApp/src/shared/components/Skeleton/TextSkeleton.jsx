import { Skeleton } from "./Skeleton";

export function TextSkeleton({ className, ...props }) {
  return <Skeleton className={`card-text ${className || ""}`} {...props} />;
}
