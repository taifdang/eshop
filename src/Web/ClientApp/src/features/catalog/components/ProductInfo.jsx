const ProductInfo = ({
  name,
  price,
  options,
  selectedOption,
  onSelectOption,
  onSetPreview,
}) => {
  const handleSelect = (optionId, optionValueId) => {
    // NOTE: CLONE + DELETE OBJECT
    onSelectOption((prev) => {
      const next = { ...prev };
      if (next[optionId] === optionValueId) {
        delete next[optionId];
      } else {
        next[optionId] = optionValueId;
      }
      return next;
    });
  };

  return (
    <div className="product-info">
      {/* NAME */}
      <div className="line-clamp-2">
        <h1 className="product-detail__name">{name}</h1>
      </div>
      {/* PRICE */}
      <div style={{ marginTop: "10px" }}>
        <div
          className="flex flex-col"
          style={{ padding: "15px 20px", backgroundColor:"rgb(248, 249, 250)" }}
        >
          <section>
            <div className="flex items-center" style={{ height: "36px" }}>
              <div className="product-detail-current-price">{price} ₫</div>
              {/* <div className="product-detail-current-price">
                4.000₫ - 22.500₫
              </div> */}
              <div className="product-detail-old-price">200.000₫</div>
              <div className="product-detail-percent">-33%</div>
            </div>
          </section>
        </div>
      </div>
      {/* OPTION AND GROUP BUTTON ???*/}
      <div className="product-detail-option-section">
        <div className="flex flex-col">
          {/* OPTION */}
          {options.map((opt) => {
            const canBeScroll = opt.values.length > 20;
            return (
              <section
                key={opt.id}
                className="flex items-baseline"
                style={{ marginBottom: "24px" }}
              >
                <h2 className="product-detail-option">{opt.label}</h2>

                <div
                  className={`flex items-center flex-wrap ${
                    canBeScroll ? "product-option-section-overflow" : ""
                  }`}
                >
                  {opt.values.map((item) => {
                    // require:
                    // 1./ change css and tick
                    // 2./ change main image if option value have image
                    const __isSelected = selectedOption[opt.id] === item.id;

                    return (
                      <div key={item.id} className="overflow-y-hidden">
                        {item.image ? (
                          <button
                            key={item.value}
                            className={`option-box-container ${
                              __isSelected
                                ? "selection-box-selected"
                                : "selection-box-unselected"
                            }`}
                            style={{
                              padding: "8px 8px 8px 40px",
                              margin: "8px 8px 0px 0px",
                              height: "40px",
                            }}
                            onMouseEnter={() => {
                              onSetPreview(item.image);
                            }}
                            onMouseLeave={() => {
                              onSetPreview(null);
                            }}
                            onClick={() => handleSelect(opt.id, item.id)}
                          >
                            <img
                              src={item.image}
                              alt=""
                              width="24px"
                              height="24px"
                              loading="lazy"
                              className="product-detail-option-image aspect-square object-contain"
                            />
                            <span
                              className="d-flex"
                              style={{ color: "rgba(0, 0, 0, 0.8)" }}
                            >
                              {item.value}
                            </span>
                            {__isSelected && (
                              <div className="selection-box-tick"></div>
                            )}
                          </button>
                        ) : (
                          <button
                            key={item.value}
                            className={`flex items-center justify-center bg-white relative option-box-container ${
                              __isSelected
                                ? "selection-box-selected"
                                : "selection-box-unselected"
                            }`}
                            style={{
                              padding: "8px 8px 8px 8px",
                              margin: "8px 8px 0px 0px",
                              width: "80px",
                              height: "40px",
                            }}
                            onClick={() => handleSelect(opt.id, item.id)}
                          >
                            <span
                              className="d-flex"
                              style={{ color: "rgba(0, 0, 0, 0.8)" }}
                            >
                              {item.value}
                            </span>
                            {__isSelected && (
                              <div className="selection-box-tick"></div>
                            )}
                          </button>
                        )}
                      </div>
                    );
                  })}
                </div>
              </section>
            );
          })}

          {/* QUANTITY  */}
          <section
            className="flex items-center"
            style={{
              height: "32px",
              marginTop: "16px",
              fontSize: "14px",
              lineHeight: "16.8px",
              color: "rgb(117, 117, 117)",
            }}
          >
            <h2 className="product-detail-option">Quantity</h2>
            <div className="flex items-center">
              <div style={{ marginRight: "15px" }}>
                <div className="flex items-center border">
                  <button
                    aria-label="Decrease"
                    className="shop-button-quantity flex justify-center items-center"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M18 12.998H6a1 1 0 0 1 0-2h12a1 1 0 0 1 0 2"
                      />
                    </svg>
                  </button>
                  <input
                    aria-label="search-input"
                    type="text"
                    name=""
                    id=""
                    value="1"
                    className="shop-button-quantity-input"
                  />
                  <button
                    aria-label="Increase"
                    className="shop-button-quantity justify-center items-center"
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="24"
                      height="24"
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M19 12.998h-6v6h-2v-6H5v-2h6v-6h2v6h6z"
                      />
                    </svg>
                  </button>
                </div>
              </div>
              <div className="">INSTOCK</div>
            </div>
          </section>
        </div>
      </div>
      {/* BUTTON GROUP */}
      <div className="section-wrapper flex-end">
        <div style={{ paddingLeft: "20px" }}>
          <div className="flex">
            <button className="shop-button-add-to-cart">
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
            <button className="shop-button-buy-now">Buy Now</button>
          </div>
        </div>
      </div>
    </div>
  );
};
export default ProductInfo;
