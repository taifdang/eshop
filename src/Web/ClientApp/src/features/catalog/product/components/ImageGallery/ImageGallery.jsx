import { usePreview } from "../../contexts/PreviewContext";
import s from "./ImageGallery.module.css";
import clsx from "clsx";

import arrowLeft from "@/assets/images/arrow_left.svg";
import arrowRight from "@/assets/images/arrow_right.svg";

const ImageGallery = ({
  images = [],
  limit,
  galleryIndex,
  onSetGalleryIndex,
  onSelectImage,
  selectedImage,
}) => {
  //context
  const { setImage } = usePreview();

  const visibleImages = images.slice(galleryIndex, galleryIndex + limit);
  const shouldShowButton = images && images.length > limit;

  const nextItem = () => {
    if (galleryIndex + limit < images.length) {
      onSetGalleryIndex(galleryIndex + 1);
    }
  };

  const prevItem = () => {
    if (galleryIndex > 0) {
      onSetGalleryIndex(galleryIndex - 1);
    }
  };

  const handleImage = (index, image) => {
    onSelectImage(index);
    setImage(image);
  };

  return (
    <div className={s["gallery-section"]}>
      {visibleImages.map((img, index) => {
        const __index = galleryIndex + index;
        const __active = __index === selectedImage;

        return (
          <div
            key={index}
            className={s["image-wrapper"]}
            onMouseEnter={() => handleImage(__index, img.url)}
            onMouseLeave={() => {}}
            onClick={() => handleImage(__index, img.url)}
          >
            <img
              src={img.url}
              alt={`Thumbnail ${index}`}
              className={`${s["image-box"]} ${__active ? s["active"] : ""}`}
            />
          </div>
        );
      })}
      {shouldShowButton && (
        <>
          <button
            className={clsx(s["gallery__button"], s["gallery__button--left"])}
            disabled={galleryIndex === 0}
            onClick={prevItem}
          >
            <img src={arrowLeft} />
          </button>
          <button
            className={clsx(s["gallery__button"], s["gallery__button--right"])}
            disabled={galleryIndex + limit >= images.length}
            onClick={nextItem}
          >
            <img src={arrowRight} />
          </button>
        </>
      )}
    </div>
  );
};

export default ImageGallery;
