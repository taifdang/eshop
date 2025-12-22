import { useState, useEffect } from "react";
import ProductImage from "../components/ProductImage";
import ProductImageList from "../components/ProductImageList";
import ProductDescription from "../components/ProductDescription";
import ProductInfo from "../components/ProductInfo";

const ProductDetailPage = () => {
  // FAKE DATA
  const product = {
    id: 101,
    name: "Basic T-Shirt",
    price: 199000,
    category: "Clothes",
    description:
      "Lorem ipsum dolor sit amet consectetur adipisicing elit. Error cupiditate praesentium asperiores consectetur mollitia, ipsum eligendi placeat doloremque accusantium. Aliquam adipisci enim nemo eligendi? Illo, ad. Aperiam ipsa officia eveniet?",
    images: [
      { id: 1, url: "src/assets/images/den.jpg" },
      { id: 2, url: "src/assets/images/trang.jpg" },
      { id: 3, url: "src/assets/images/nau.jpg" },
      { id: 4, url: "src/assets/images/den.jpg" },
      { id: 5, url: "src/assets/images/trang.jpg" },
      { id: 6, url: "src/assets/images/nau.jpg" },
      { id: 7, url: "src/assets/images/den.jpg" },
    ],

    options: [
      {
        id: 1,
        label: "Color",
        values: [
          {
            id: 1,
            value: "Black",
            image: "src/assets/images/den.jpg",
          },
          {
            id: 2,
            value: "White",
            image: "src/assets/images/trang.jpg",
          },
          {
            id: 3,
            value: "Brown",
            image: "src/assets/images/nau.jpg",
          },
        ],
      },
      {
        id: 2,
        label: "Size",
        values: [
          { id: 1, value: "S" },
          { id: 2, value: "M" },
        ],
      },
    ],
  };

  // HOOKS
  const [selectedOptions, setSelectedOptions] = useState({});
  const [selectedImage, setSelectedImage] = useState(0);
  const [galleryIndex, setGalleryIndex] = useState(0);
  const [previewImage, setPreviewImage] = useState(null);

  // ATTRIBUTES
  const MAX_VISIBLE = 5;
  //const mainImage = selectedImage !== null ? product.images[selectedImage].url : product.images[0].url;
  const mainImage =
    previewImage !== null ? previewImage : product.images[selectedImage].url;

  // LOGIC FUNCTION

  // SIDE EFFECT
  useEffect(() => {
    console.log(
      "Selected Options updated:",
      JSON.stringify(selectedOptions, null, 2)
    );

    // FETCH API VARIANT BY OPTION VALUE
  }, [selectedOptions]);

  return (
    <div style={{ backgroundColor: "#f5f5f5" }}>
      <div className="product-detail-container">
        {/* OVERVIEW */}
        <div className="flex mt-3 p-0 bg-white">
          {/* LEFT */}
          <div className="overview-left">
            <div className="flex flex-col">
              <ProductImage mainImage={mainImage} />
              <ProductImageList
                images={product.images}
                maxSize={MAX_VISIBLE}
                galleryIndex={galleryIndex}
                onSetGalleryIndex={setGalleryIndex}
                selectedImage={selectedImage}
                onSelectImage={setSelectedImage}
              />
            </div>
          </div>
          {/* RIGHT */}
          <div className="overview-right">
            <div className="flex flex-col">
              <ProductInfo
                price={product.price}
                name={product.name}
                options={product.options}
                selectedOption={selectedOptions}
                onSelectOption={setSelectedOptions}
                onSetPreview={setPreviewImage}
              />
            </div>
          </div>
        </div>
        {/* DESCRIPTION */}
        <ProductDescription
          category={product.category}
          description={product.description}
        />
      </div>
    </div>
  );
};

export default ProductDetailPage;
