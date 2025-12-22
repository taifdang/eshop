const ProductImageList = ({
  images = [],
  maxSize,
  galleryIndex,
  onSetGalleryIndex,
  onSelectImage,
  selectedImage,
}) => {
  const visibleImages = images.slice(galleryIndex, galleryIndex + maxSize);

  // LOGIC FUNCTION
  const nextItem = () => {
    if (galleryIndex + maxSize < images.length) {
      onSetGalleryIndex(galleryIndex + 1);
      console.log(galleryIndex);
    }
  };

  const prevItem = () => {
    if (galleryIndex > 0) {
      onSetGalleryIndex(galleryIndex - 1);
      console.log(galleryIndex);
    }
  };

  return (
    <div className="position-relative image-collection mt-2 ">
      {visibleImages.map((img, index) => {
        //Logic
        const __index = galleryIndex + index;
        const isActive = __index === selectedImage;

        return (
          <div
            key={index}
            className="w-[92px] h-[92px] p-[5px] cursor-pointer aspect-square "
            // style={{ width: "92px", height: "92px", padding: "5px" }}
            onMouseEnter={() => onSelectImage(__index)}
            onMouseLeave={() => {}}
            onClick={() => onSelectImage(__index)}
          >
            <img
              src={img.url}
              alt={`Thumbnail ${index}`}
              // className="object-fit-cover h-100 w-100 image-item"
              className={`w-full h-full object-contain image-item ${
                isActive ? "active" : ""
              }`}
            />
          </div>
        );
      })}
      {/* LEFT BUTTON */}
      <button
        className="image-collection-button image-collection-button--left"
        disabled={galleryIndex === 0}
        onClick={prevItem}
      >
        <img src="src/assets/images/arrow_left.svg" />
      </button>
      {/* RIGHT BUTTON */}
      <button
        className="image-collection-button image-collection-button--right"
        disabled={galleryIndex + maxSize >= images.length}
        onClick={nextItem}
      >
        <img src="src/assets/images/arrow_right.svg" />
      </button>
    </div>
  );
};

export default ProductImageList;
