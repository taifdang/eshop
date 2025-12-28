import { useState } from "react";

export default function CartItem() {

  const [quantity, SetQuantity] = useState(1);

  return (
    <div className="product-item__container" role="listitem">
      <div className="flex items-center">
        <div
          className="flex col-checkbox"
          style={{
            flexDirection: "row-reverse",
            paddingLeft: "20px",
            paddingRight: "12px",
            minWidth: "58px",
          }}
        >
          <label htmlFor="">
            {/* <input type="checkbox" name="" id="" /> */}
            <div
              style={{
                width: "18px",
                height: "18px",
                border: "1px solid black",
                marginRight: "8px",
                borderRadius: "2px",
              }}
            ></div>
          </label>
        </div>
        <div className="col col-product2 flex">
          <div
            style={{ display: "flex", fontSize: "14px", lineHeight: "16.8px" }}
          >
            <a href="/">
              <img
                src="src/assets/images/default.jpg"
                alt=""
                width="80px"
                height="80px"
                className="object-fit-contain"
              />
            </a>
          </div>
          <div
            className="flex flex-col flex-1"
            style={{
              padding: "5px 20px 0 10px",
            }}
          >
            <a
              href="/"
              className="line-clamp-2"
              style={{
                fontSize: "14px",
                lineHeight: "16px",
                marginBottom: "5px",
              }}
            >
              Lorem ipsum dolor sit amet consectetur adipisicing elit.
              Voluptatibus molestias rerum eligendi dolorem, magni veniam quasi
              non sapiente eos cupiditate perspiciatis nobis vero hic in,
              quisquam incidunt, nostrum odio optio.
            </a>
          </div>
        </div>
        {/* if have variant */}
        <div className="col col-variant flex items-center">
          <button
            type="button"
            className="flex flex-col text-start flex-wrap my-auto"
            style={{
              color: "rgba(0, 0, 0, 0.54)",
              fontSize: "14px",
              lineHeight: "16px",
              marginRight: "10px",
              padding: 0,
            }}
          >
            <div>Variants:</div>
            <div style={{ marginTop: "5px" }}>Lorem ipsum dolor sit</div>
          </button>
        </div>
        <div className="col col-unit flex flex-col justify-center items-center">
          <div>
            {/* -------- IF HAVE SALE PRICE -------- */}
            {/* 
            <span
              style={{
                marginLeft: "10px",
                textDecoration: "line-through",
                marginRight: "10px",
                color: "rgba(0, 0, 0, 0.54)",
              }}
            >
              105.000₫
            </span> */}
            <span>105.000₫</span>
          </div>
        </div>
        <div className="col col-qty flex flex-col justify-center items-center">
          <div className="flex items-center border">
            <button
              aria-label="Decrease"
              className="shop-button-quantity flex items-center justify-center"
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
              value={quantity}  
              readOnly
              className="shop-button-quantity-input"
            />
            <button
              aria-label="Increase"
              className="shop-button-quantity flex items-center justify-center"
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
        <div className="col col-total flex flex-col justify-center items-center">
          <span>105.000₫</span>
        </div>
        <div className="col col-actions text-center flex flex-col justify-center items-center">
          <button style={{ padding: "1px 6px" }}>Delete</button>
        </div>
      </div>
    </div>
  );
}
