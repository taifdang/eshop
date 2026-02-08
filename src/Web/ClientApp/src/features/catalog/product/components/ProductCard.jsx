import { formatCurrency } from "@/shared/lib/format";
import fallbackImage from "@/assets/images/default.jpg";
import { Link } from "react-router-dom";

export default function ProductCard({ product }) {
  const displayImage = product?.image?.url ?? fallbackImage;

  return (
    <div className="w-1/6" style={{ padding: "5px" }}>
      <div
        className="relative h-full"
        style={{ border: "1px solid rgba(0, 0, 0, 0.09)" }}
      >
        <Link to={`/product/${product.id}`} className="block h-full">
          <div className="flex flex-col h-full w-full bg-white">
            {/* -------- IMAGE  -------- */}
            <div className="relative">
              <div className="w-full aspect-square">
                <img
                  src={displayImage}
                  className="w-full h-full object-contain"
                />
              </div>
              {/* -------- BADGE -------- */}
              <div className="absolute top-0 right-0 bg-[black] px-1 product-card__discount">
                {product.percent && (
                  <span className="text-white text-xs">
                    -{product.percent}%
                  </span>
                )}
              </div>
            </div>
            {/* -------- CONTENT -------- */}
            <div className="flex flex-col flex-1 p-2 justify-between">
              <div className="text-sm line-clamp-2">{product.title}</div>
              <div className="flex justify-between items-center mt-2">
                <div className="flex items-center">
                  <span className="text-base">
                    {formatCurrency(product.price)}
                  </span>
                  {/* <span className="pl-1 text-[10px] underline">đ</span> */}
                </div>
                {product.sold && (
                  <span className="text-xs text-gray-500">
                    Sold {product.sold}
                  </span>
                )}
              </div>
            </div>
          </div>
        </Link>
      </div>
    </div>
  );
}
