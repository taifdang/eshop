import { Skeleton } from "./Skeleton";

export function ImageSkeleton({ className, ...props }) {
  return <Skeleton className={className} {...props} />;
}
