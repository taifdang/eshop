export default function ProductImage({ mainImage }) {
  return (
    <div
      className="h-[450px] aspect-square"
      //style={{ height: "450px", width: "450px" }}
    >
      <img
        src={mainImage}
        alt="product"
        className="object-cover h-full w-full "
      />
    </div>
  );
}
