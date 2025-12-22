export default function CartEmpty() {
  return (
    <div style={{ margin: "20px 0" }}>
      <div
        style={{
          height: "336px",
          display: "flex",
          flexDirection: "column",
          alignItems: "center",
          justifyContent: "center",
        }}
      >
        <div
          style={{
            height: "100px",
            width: "100px",
            backgroundImage: "url(src/assets/images/cart-empty.png)",
            backgroundSize: "cover",
          }}
        ></div>
        <div
          style={{
            marginTop: "18px",
            fontSize: "14px",
            lineHeight: "16px",
            fontWeight: 700,
            color: "rgba(0, 0, 0, 0.4)",
          }}
        >
          Your shopping cart is empty
        </div>
        <a href="/" style={{ marginTop: "17px" }}>
          <button
            style={{
              display: "flex",
              alignItems: "center",
              justifyContent: "center",
              padding: "10px 42px",
              backgroundColor: "black",
              color: "white",
            }}
          >
            <span style={{ fontSize: "16px", lineHeight: "20px" }}>
              Go Shopping Now
            </span>
          </button>
        </a>
      </div>
    </div>
  );
}
