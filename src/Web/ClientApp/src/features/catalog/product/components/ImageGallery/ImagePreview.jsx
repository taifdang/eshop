import { usePreview } from "../../contexts/PreviewContext";

export default function ImagePreview() {
  const { image } = usePreview();
  return (
    <div style={{ height: "450px", width: "450px", aspectRatio: 1 }}>
      <img
        src={image}
        style={{ height: "100%", width: "100%", objectFit: "contain" }}
      />
    </div>
  );
}
