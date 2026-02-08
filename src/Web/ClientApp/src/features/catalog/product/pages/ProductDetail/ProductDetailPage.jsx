import s from "./ProductDetailPage.module.css";
import { useState, useEffect, useRef, useMemo } from "react";
import {
  ImagePreview,
  ImageGallery,
  ProductSummary,
  OptionSelector,
  QuantitySelector,
  ProductDetails
} from "../../components";
import { PreviewProvider } from "../../contexts/PreviewContext";
import { useParams } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  fetchProductById,
  fetchVariantByOptions,
} from "../../services/product-service";

import { formatCurrency } from "@/shared/lib/format";
import fallbackImage from "@/assets/images/default.jpg";
import {
  fetchBasket,
  updateBasket,
} from "../../../../basket/services/basket-service";
import clsx from "clsx";

export function ProductDetailPage() {
  const { id } = useParams();
  const queryClient = useQueryClient();
  const hasTrackedRef = useRef(false);

  const [selectedOptions, setSelectedOptions] = useState({});
  const [selectedImage, setSelectedImage] = useState(0);
  const [galleryIndex, setGalleryIndex] = useState(0);
  const [quantity, setQuantity] = useState(0); // current quantity in basket
  const [inputValue, setInputValue] = useState(1); // default value
  const [displayPrice, setDisplayPrice] = useState("");
  const [displayStock, setDisplayStock] = useState(0);
  const [hasError, setHasError] = useState({ isError: false, message: "" });
  const [isSuccess, setIsSuccess] = useState(false);

  // filter available quantity and options ???
  const [variantId, setVariantId] = useState(null);

  const canSetQuantity = variantId !== null;

  // useQuery[basket]
  const { data: basket } = useQuery({
    queryKey: ["basket"],
    queryFn: () => fetchBasket().then((res) => res.data),
    refetchOnWindowFocus: false,
    initialData: {
      id: "",
      customerId: "",
      items: [],
      createdAt: new Date().toDateString(),
      lastModified: null,
    },
  });

  const updateBasketItem = useMutation({
    mutationFn: (qty) => updateBasket(variantId, qty),
    onSuccess: () => {
      queryClient.refetchQueries({ queryKey: ["basket"] });
      setHasError({ isError: false, message: "" });
      setIsSuccess(true);
    },
    onError: (err) => {
      setHasError({ isError: true, message: "Invalid Input" });
      console.log(err);
    },
  });

  const handleAddToCart = () => {
    if (!variantId) {
      setHasError({
        isError: true,
        message: "Please select product variation first",
      });
      return;
    }

    const newQuantity = quantity > 0 ? quantity + inputValue : inputValue;
    updateBasketItem.mutate(newQuantity);
  };

  const handleIncrease = () => {
    setInputValue((prev) => prev + 1);
  };

  const handleDecrease = () => {
    setInputValue((prev) => Math.max(1, prev - 1));
  };

  // sync quantity from basket
  useEffect(() => {
    if (!variantId) return;
    const existingItem = basket?.items.find(
      (item) => item.productVariantId === variantId,
    );
    if (existingItem) {
      setQuantity(existingItem.quantity);
    } else {
      setQuantity(0);
    }
  }, [basket, variantId]);

  useEffect(() => {
    if (id && !hasTrackedRef.current) {
      hasTrackedRef.current = true;
      //
    }
  }, [id]);

  const optionValueIds = useMemo(
    () => Object.values(selectedOptions || {}),
    [selectedOptions],
  );

  // useQuery[variant]
  const {
    data: variant,
    isLoading: vLoading,
    isFetching,
  } = useQuery({
    queryKey: ["variant", id, optionValueIds],
    queryFn: () =>
      fetchVariantByOptions(id, optionValueIds).then((res) => res.data),
    enabled: optionValueIds.length > 0,
  });

  // useQuery[product]
  const {
    data: product,
    isLoading,
    error,
  } = useQuery({
    queryKey: ["product", id],
    queryFn: () => fetchProductById(id).then((res) => res.data),
    enabled: !!id,
  });

  const gallery__limit = 5;
  const displayImage = product?.mainImage?.url ?? fallbackImage;

  // set display price text
  const handleSetPriceText = (minPrice, maxPrice) => {
    if (minPrice == null || maxPrice == null) return "";

    if (minPrice === maxPrice) return formatCurrency(maxPrice);

    return `${formatCurrency(minPrice)} - ${formatCurrency(maxPrice)}`;
  };

  // display option selected (clone + delete object)
  const handleSelectOption = (optionId, optionValueId) => {
    setSelectedOptions((prev) => {
      const next = { ...prev };
      if (next[optionId] === optionValueId) {
        delete next[optionId];
      } else {
        next[optionId] = optionValueId;
      }
      return next;
    });
  };

  const resolveStock = () => {
    if (variant) return variant.totalStock ?? 0;
    if (product?.variantSummary) return product.variantSummary.quantity ?? 0;
    return 0;
  };

  // get price source (variant/product)
  const priceSource = useMemo(() => {
    if (variant) {
      return {
        minPrice: variant.minPrice,
        maxPrice: variant.maxPrice,
      };
    }

    if (product?.variantSummary) {
      return {
        minPrice: product.variantSummary.minPrice,
        maxPrice: product.variantSummary.maxPrice,
      };
    }
    return null;
  }, [variant, product]);

  // update display price text when product/variant change state
  useEffect(() => {
    if (!priceSource) return;

    const newPrice = handleSetPriceText(
      priceSource.minPrice,
      priceSource.maxPrice,
    );

    const timer = setTimeout(() => {
      setDisplayPrice(newPrice);
    }, 800);

    return () => clearTimeout(timer);
  }, [priceSource]);

  // update display stock when product/variant change state
  useEffect(() => {
    const stock = resolveStock();
    const timer = setTimeout(() => {
      setDisplayStock(stock);
    }, 800);

    return () => clearTimeout(timer);
  }, [product, variant]);

  // get variant id
  useEffect(() => {
    if (variant?.variants.length === 1) {
      setVariantId(variant.variants[0].id);
    } else if (product?.variantSummary?.id) {
      setVariantId(product.variantSummary.id);
    } else {
      setVariantId(null);
    }
  }, [product, variant]);

  // add to cart success
  useEffect(() => {
    const timer = setTimeout(() => {
      setIsSuccess(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, [isSuccess]);

  return (
    <>
      {product && (
        <div className={s["container"]}>
          {/* Overview */}
          <div className="flex mt-3 bg-white">
            <PreviewProvider item={displayImage}>
              {/* Left */}
              <div className={s["detail__section--left"]}>
                <div className="flex flex-col">
                  {/* Display image preview */}
                  <ImagePreview />
                  {/* Gallery image*/}
                  <ImageGallery
                    images={product.images}
                    limit={gallery__limit}
                    galleryIndex={galleryIndex}
                    onSetGalleryIndex={setGalleryIndex}
                    selectedImage={selectedImage}
                    onSelectImage={setSelectedImage}
                  />
                </div>
              </div>
              {/* Right */}
              <div className={s["detail__section--right"]}>
                {/* PriceBox(regular price, price, discount/percent) */}
                <ProductSummary price={displayPrice} name={product.title} />
                {/* Configuration behavior */}
                <div
                  className={clsx(
                    s["selector__section"],
                    hasError.isError && s["error"],
                  )}
                >
                  <div className="flex flex-col">
                    {/* Option */}
                    <OptionSelector
                      options={product.options}
                      selectedOption={selectedOptions}
                      onChange={handleSelectOption}
                    />
                    {/* Quantity */}
                    <QuantitySelector
                      stock={displayStock}
                      quantity={inputValue}
                      onIncrease={handleIncrease}
                      onDecrease={handleDecrease}
                      onChange={setQuantity}
                      onShow={canSetQuantity}
                    />
                    {hasError.isError && (
                      <div className={s["error--not-enough-option"]}>
                        {hasError.message}
                      </div>
                    )}
                  </div>
                </div>
                {/* Action controls */}
                <div className={s["purchase-action__section"]}>
                  <div style={{ paddingLeft: "20px" }}>
                    <div className="flex">
                      {/* Add to cart */}
                      <button
                        onClick={() => handleAddToCart()}
                        className="purchase__button purchase__button-add-to-cart"
                      >
                        <span style={{ marginRight: "5px" }}>
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            width="24"
                            height="24"
                            viewBox="0 0 24 24"
                          >
                            <path
                              fill="currentColor"
                              d="M16 18a2 2 0 0 1 2 2a2 2 0 0 1-2 2a2 2 0 0 1-2-2a2 2 0 0 1 2-2m0 1a1 1 0 0 0-1 1a1 1 0 0 0 1 1a1 1 0 0 0 1-1a1 1 0 0 0-1-1m-9-1a2 2 0 0 1 2 2a2 2 0 0 1-2 2a2 2 0 0 1-2-2a2 2 0 0 1 2-2m0 1a1 1 0 0 0-1 1a1 1 0 0 0 1 1a1 1 0 0 0 1-1a1 1 0 0 0-1-1M18 6H4.27l2.55 6H15c.33 0 .62-.16.8-.4l3-4c.13-.17.2-.38.2-.6a1 1 0 0 0-1-1m-3 7H6.87l-.77 1.56L6 15a1 1 0 0 0 1 1h11v1H7a2 2 0 0 1-2-2a2 2 0 0 1 .25-.97l.72-1.47L2.34 4H1V3h2l.85 2H18a2 2 0 0 1 2 2c0 .5-.17.92-.45 1.26l-2.91 3.89c-.36.51-.96.85-1.64.85"
                            />
                          </svg>
                        </span>
                        <span>Add To Cart</span>
                      </button>
                      {/* Buy now */}
                      <button className="purchase__button purchase__button-buy-now">
                        Buy Now
                      </button>
                    </div>
                  </div>
                </div>
              </div>
            </PreviewProvider>
          </div>
          {/* Description: product description, dimestions, ...*/}
          <ProductDetails
            category={product.category}
            description={product.description}
          />
        </div>
      )}
      {/* MODAL */}
      {isSuccess && (
        <div className={s["add-to-cart__modal"]}>
          <div
            className="flex flex-col items-center justify-center "
            style={{ padding: "40px 20px" }}
          >
            <div>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width={60}
                height={60}
                viewBox="0 0 24 24"
              >
                <path
                  fill="#fff"
                  d="m10.6 13.8l-2.15-2.15q-.275-.275-.7-.275t-.7.275t-.275.7t.275.7L9.9 15.9q.3.3.7.3t.7-.3l5.65-5.65q.275-.275.275-.7t-.275-.7t-.7-.275t-.7.275zM12 22q-2.075 0-3.9-.788t-3.175-2.137T2.788 15.9T2 12t.788-3.9t2.137-3.175T8.1 2.788T12 2t3.9.788t3.175 2.137T21.213 8.1T22 12t-.788 3.9t-2.137 3.175t-3.175 2.138T12 22m0-2q3.35 0 5.675-2.325T20 12t-2.325-5.675T12 4T6.325 6.325T4 12t2.325 5.675T12 20m0-8"
                ></path>
              </svg>
            </div>
            <div style={{ marginTop: "10px", color: "white" }}>
              Item has been added to your shopping cart
            </div>
          </div>
        </div>
      )}
    </>
  );
}
