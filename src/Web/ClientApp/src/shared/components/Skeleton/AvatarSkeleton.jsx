import { Skeleton } from "./Skeleton";

export function AvatarSkeleton({ className, ...props }) {
  return <Skeleton className={className} {...props} />;
}
