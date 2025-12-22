
import { useState } from "react";
import { ProductCard } from "./components/ProductCard";
import { Pagination } from "../../shared/components/Pagination";

export default function HomePage() {
  const [page, setPage] = useState(1);
  const totalPage = 1;

  return (
    <div>
      <div className="layout-main">
        <div className="suggest container-wrapper">
          <h1 className="suggest__label">Daily Recover</h1>
          <hr className="suggest__divider" />
        </div>
        <div className="container-wrapper h-full mx-auto layout-section">
          <div className="flex flex-wrap mx-auto">
            {Array.from({ length: 18 }).map((_, i) => (
              <ProductCard key={i} />
            ))}
          </div>
        </div>
        {/* -------- PAGINATION -------- */}
        <Pagination
          currentPage={page}
          totalPage={totalPage}
          onChange={setPage}
        />
      </div>
    </div>
  );
}
