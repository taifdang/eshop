import { Skeleton } from "./Skeleton";

export function CardSkeleton({ className, ...props }) {
  return <Skeleton className={className} {...props} />;
}
