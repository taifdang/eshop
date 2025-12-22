import { NavBar } from "../../../shared/components/layout/NavBar";
import { CartFooter } from "../components/CartFooter";
import { CartHeader } from "../components/CartHeader";
import CartItem from "../components/CartItem";
import { useNavigate } from "react-router-dom";

export default function CartPage() {
  const navigate = useNavigate();

  return (
    <div>
      <NavBar />
      <div>
        <section className="bg-white">
          <CartHeader />
        </section>
        <div className="container-wrapper mx-auto">
          <main className="flex flex-col" style={{ padding: "20px 0 0 0" }}>
            <div className="product-list-section">
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
              <div className="col col-product">Product</div>
              <div className="col col-unit text-center">Unit Price</div>
              <div className="col col-qty text-center">Quantity</div>
              <div className="col col-total text-center">Total Price</div>
              <div className="col col-actions text-center">Actions</div>
            </div>
            <section className="product-list-item-section">
              <section className="100h">
                <div className="border" style={{ margin: "20px 22px" }}>
                  <div
                    style={{
                      fontSize: "14px",
                      lineHeight: "16px",
                      fontWeight: 500,
                      padding: "9px 12px",
                      backgroundColor: "gray",
                      color: "white",
                    }}
                  >
                    <span>Items: 3 </span>
                  </div>
                  <CartItem />
                  <div
                    style={{
                      borderBottom: "1px solid rgba(0, 0, 0, 0.09)",
                      margin: "0 20px 0 50px",
                    }}
                  ></div>
                  <CartItem />
                  <div
                    style={{
                      borderBottom: "1px solid rgba(0, 0, 0, 0.09)",
                      margin: "0 20px 0 50px",
                    }}
                  ></div>
                  <CartItem />
                  <div
                    style={{
                      borderBottom: "1px solid rgba(0, 0, 0, 0.09)",
                      margin: "0 20px 0 50px",
                    }}
                  ></div>
                  <CartItem />
                  <div
                    style={{
                      borderBottom: "1px solid rgba(0, 0, 0, 0.09)",
                      margin: "0 20px 0 50px",
                    }}
                  ></div>
                  <CartItem />
                  <div
                    style={{
                      borderBottom: "1px solid rgba(0, 0, 0, 0.09)",
                      margin: "0 20px 0 50px",
                    }}
                  ></div>
                  <CartItem />
                </div>
              </section>
            </section>
          </main>
          <section className="cart-footer">
            <div className="cart-footer__promotion">
              <img
                style={{ marginRight: "8px" }}
                src="src/assets/images/voucher_icon.svg"
                alt=""
              />
              <div>Platform voucher</div>
              <div className="flex-1"></div>
              <button
                style={{
                  marginRight: "30px",
                  fontSize: "14px",
                  lineHeight: "16px",
                  color: "rgb(0, 85, 170)",
                }}
              >
                Select or enter code
              </button>
            </div>
            <div className="cart-footer__separate"></div>
            <div className="cart-footer__total">
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
              <button style={{ padding: "1px 6px", display: "block" }}>
                Select All (3)
              </button>
              <button
                style={{
                  padding: "1px 6px",
                  margin: "0 8px",
                  textWrap: "nowrap",
                }}
              >
                Delete
              </button>
              <div className="flex-1"></div>
              <div className="flex flex-col">
                <div className="flex items-center flex-end">
                  <div
                    className="flex items-center"
                    style={{
                      marginLeft: "20px",
                      fontSize: "16px",
                      lineHeight: "20px",
                    }}
                  >
                    Total (0 item):
                  </div>
                  <div
                    style={{
                      marginLeft: "5px",
                      fontSize: "24px",
                      lineHeight: "28px",
                    }}
                  >
                    1.000.000₫
                  </div>
                </div>
              </div>

              <div>
                <button
                  onClick={() => navigate("/checkout")}
                  className="relative flex items-center justify-center"
                  style={{
                    height: "40px",
                    width: "210px",
                    padding: "13px 36px",
                    backgroundColor: "black",
                    color: "white",
                    margin: "0 22px 0 15px",
                  }}
                >
                  <span style={{ fontSize: "14px", lineHeight: "14px" }}>
                    Checkout
                  </span>
                </button>
              </div>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
}
