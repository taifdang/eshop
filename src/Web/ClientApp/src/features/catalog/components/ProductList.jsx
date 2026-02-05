import ProductCard from "./ProductCard";
import { useState, useEffect } from "react";
import { fetchProducts } from "../services/product-service";
import { useQuery } from "@tanstack/react-query";
import { useLocation, useNavigate } from "react-router-dom";
import { Pagination } from "@/shared/components";

export function ProductList() {
  const location = useLocation();
  const navigate = useNavigate();

  // Parse query params from URL
  const urlParams = new URLSearchParams(location.search);

  const initialPage = 1;
  const [currentPage, setCurrentPage] = useState(initialPage);

  // Set page sizes
  const homePageSize = 10;
  const productPageSize = 16;
  const pageSize = location.pathname === "/" ? homePageSize : productPageSize;

  // useQuery hook
  const { data, isLoading, error } = useQuery({
    queryKey: ["products", currentPage, pageSize],
    queryFn: () => fetchProducts(1, pageSize).then((res) => res.data),
    refetchOnWindowFocus: false,
    retry: false
  });

  useEffect(() => {
    const currentPage = parseInt(urlParams.get("pageNumber") || "1");

    setCurrentPage(currentPage);
  }, [location.search, location.pathname]);

  const totalPages = 1;
  const hasResults = data?.items && data.items.length > 0;

  const isHomePage = location.pathname === "/" && currentPage === 1;

  // only on home page
  const showSeeMore = isHomePage && data && data.count > homePageSize;
  // not on home page
  const showPagination = !isHomePage && totalPages > 1;

  const handlePageChange = () => { };


  return (
    <div>
      {hasResults && (
        <>
          <div className="container-wrapper h-full mx-auto">
            <div className="flex flex-wrap mx-auto">
              {data.items.map((product) => (
                <ProductCard key={product.id} product={product} />
              ))}
            </div>
          </div>
        </>
      )}
      {/* -------- PAGINATION -------- */}
      {/* <Pagination currentPage={page} totalPage={totalPage} onChange={setPage} /> */}
    </div>
  );
}
