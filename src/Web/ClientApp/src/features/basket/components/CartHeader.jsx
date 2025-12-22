export function CartHeader() {
  return (
    <div
      className="container-wrapper mx-auto"
      style={{ padding: "16px 0 10px 0", height: "90px" }}
    >
      <div className="flex flex-col">
        <div className="cart-header">
          <a className="cart-logo" href="/">
            <div></div>
            <img
              src="src/assets/images/logo-brand-no-bg.png"
              width="162px"
              height="50px"
              alt=""
            />
            <div className="cart-logo__page-name">Shoppping Cart</div>
          </a>
        </div>
      </div>
    </div>
  );
}
