import s from "./index.module.css";
import { useState, useEffect, useRef, useMemo } from "react";
import ImagePreview from "../../components/ImagePreview";
import Gallery from "../../components/Gallery";
import { Info } from "../../components/Info";
import { OptionSelector } from "../../components/OptionSelector/OptionSelector";
import { QuantitySelector } from "../../components/QuantiySelector/QuantitySelector";
import { Description } from "../../components/Description";
import { PreviewProvider } from "../../contexts/PreviewContext";
import { useParams } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import {
  fetchProductById,
  fetchVariantByOptions,
} from "../../services/product-service";

import { formatCurrency } from "@/shared/lib/currency";
import fallbackImage from "@/assets/images/default.jpg";
import { useDebounce } from "@/shared/hooks/useDebounce";
import { updateBasket } from "../../../basket/services/basket-service";

export function ProductDetailPage() {
  const { id } = useParams();
  const queryClient = useQueryClient();
  const hasTrackedRef = useRef(false);

  const [selectedOptions, setSelectedOptions] = useState({});
  const [selectedImage, setSelectedImage] = useState(0);
  const [galleryIndex, setGalleryIndex] = useState(0);
  const [quantity, setQuantity] = useState(1);

  // filter available quantity and options ???
  const [variantId, setVariantId] = useState(null);
  const canSetQuantity = variantId !== null;
  const canAddToCart = variantId !== null;

  const addToCart = useMutation({
    mutationFn: ({ variantId, quantity }) => updateBasket(variantId, quantity),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["basket"] });
    },
  });

  const handleAddToCart = () => {
    if (variantId === null) return;
    addToCart.mutate({ variantId: variantId, quantity: quantity });
  };

  useEffect(() => {
    if (id && !hasTrackedRef.current) {
      hasTrackedRef.current = true;
      //
    }
  }, [id]);

  const optionValueIds = useMemo(
    () => Object.values(selectedOptions || {}),
    [selectedOptions]
  );

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

  // useQuery hook
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

  const isAvailable = variant?.totalStock ?? null;

  const handleSetPriceText = (minPrice, maxPrice) => {
    if (minPrice == null || maxPrice == null) return "";

    if (minPrice === maxPrice) return formatCurrency(maxPrice);

    return `${formatCurrency(minPrice)} - ${formatCurrency(maxPrice)}`;
  };

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

  const handleIncrease = () => {};

  const handleDecrease = () => {};

  const updateQuatity = () => {};

  const priceSource = useMemo(() => {
    if (variant) {
      return {
        minPrice: variant.minPrice,
        maxPrice: variant.maxPrice,
      };
    }

    if (product?.variantBrief) {
      return {
        minPrice: product.variantBrief.minPrice,
        maxPrice: product.variantBrief.maxPrice,
      };
    }
    return null;
  }, [variant, product]);

  const priceText = useMemo(() => {
    if (!priceSource) return "";
    return handleSetPriceText(priceSource.minPrice, priceSource.maxPrice);
  }, [priceSource]);

  // delay show UI
  const priceText__delay = useDebounce(priceText, 500);
  const quantity__delay = useDebounce(isAvailable, 500);

  useEffect(() => {
    if (!product) return;
    setVariantId(product.variantBrief?.id ?? null);
  }, [product]);

  useEffect(() => {
    if (!variant) return;

    if (variant.variants.length === 1) {
      setVariantId(variant.variants[0].id);
    } else {
      setVariantId(null);
    }
  }, [variant]);

  if (isLoading) {
    return <></>;
  }

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
                  <Gallery
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
                <Info price={priceText__delay} name={product.title} />
                {/* Configuration behavior */}
                <div className={s["selector__section"]}>
                  <div className="flex flex-col">
                    {/* Option */}
                    <OptionSelector
                      options={product.options}
                      selectedOption={selectedOptions}
                      onChange={handleSelectOption}
                    />
                    {/* Quantity */}
                    <QuantitySelector
                      available={quantity__delay}
                      quantity={quantity}
                      onChange={setQuantity}
                      onShow={canSetQuantity}
                    />
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
          <Description
            category={product.category}
            description={product.description}
          />
        </div>
      )}
    </>
  );
}
