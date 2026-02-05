import logo from "@/assets/images/logo-brand-no-bg.png";

export function Header() {
  return (
    <div
      className="container-wrapper mx-auto"
      style={{
        padding: "16px 0 10px 0",
        height: "90px",
        backgroundColor: "rgb(255,255,255)",
      }}
    >
      <div className="flex flex-col flex-1">
        <div className="flex flex-row flex-1 ">
          <div className="flex">
            <a
              href="/"
              className="relative"
              style={{ top: "-3px", paddingRight: "40px" }}
            >
              <div className="flex items-center">
                <div>
                  <img src={logo} width="162px" height="50px" alt="" />
                </div>
              </div>
            </a>
          </div>
          <div className="header-with-search">
            <form role="search" autoComplete="off" className="search-bar">
              <div className="search-bar__main">
                <div className="search-bar-input">
                  <input
                    type="combobox"
                    autoComplete="off"
                    maxLength="128"
                    className="grow p-0"
                  />
                </div>
                {/* <div className="search-popover"></div> */}
              </div>
              <button type="button" className="search-bar__search-button">
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  width="19"
                  height="19"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill="currentColor"
                    d="m18.031 16.617l4.283 4.282l-1.415 1.415l-4.282-4.283A8.96 8.96 0 0 1 11 20c-4.968 0-9-4.032-9-9s4.032-9 9-9s9 4.032 9 9a8.96 8.96 0 0 1-1.969 5.617m-2.006-.742A6.98 6.98 0 0 0 18 11c0-3.867-3.133-7-7-7s-7 3.133-7 7s3.133 7 7 7a6.98 6.98 0 0 0 4.875-1.975z"
                  />
                </svg>
              </button>
            </form>
            <div
              style={{ height: "24px", lineHeight: "24px", fontSize: "16px" }}
            >
              <div className="flex flex-wrap">
                <a href="#1" style={{ margin: "5px 13px 5px 0" }}>
                  #hashtag
                </a>
                <a href="#2" style={{ margin: "5px 13px 5px 0" }}>
                  #category
                </a>
                <a href="#3" style={{ margin: "5px 13px 5px 0" }}>
                  #hotsale
                </a>
              </div>
            </div>
          </div>
          <div
            className=" cart-wrapper flex items-center justify-center grow "
            style={{ margin: "0 10px 5px 10px" }}
          >
            <div className="star-popover relative" id="cart_target_id">
              <div role="button" className="star-popover__target">
                <div className="cart-btn-group ">
                  <a
                    href="/cart"
                    className="flex items-center relative"
                    style={{ marginLeft: "5px" }}
                  >
                    <svg
                      xmlns="http://www.w3.org/2000/svg"
                      width="32"
                      height="32"
                      style={{
                        fontSize: "17px",
                        lineHeight: "20.4px",
                        marginRight: "10px",
                        display: "block",
                      }}
                      viewBox="0 0 24 24"
                    >
                      <path
                        fill="currentColor"
                        d="M11.25 18.75c0 .83-.67 1.5-1.5 1.5s-1.5-.67-1.5-1.5s.67-1.5 1.5-1.5s1.5.67 1.5 1.5m5-1.5c-.83 0-1.5.67-1.5 1.5s.67 1.5 1.5 1.5s1.5-.67 1.5-1.5s-.67-1.5-1.5-1.5m4.48-9.57l-2 8a.75.75 0 0 1-.73.57H8c-.36 0-.67-.26-.74-.62L5.37 5.25H4c-.41 0-.75-.34-.75-.75s.34-.75.75-.75h2c.36 0 .67.26.74.62l.43 2.38H20a.754.754 0 0 1 .73.93m-1.69.57H7.44l1.18 6.5h8.79z"
                      />
                    </svg>
                    <div
                      className="relative text-center block rounded-full"
                      style={{
                        padding: "0 5px",
                        border: "2px solid white",
                        left: "-20px",
                        top: "-11px",
                        fontSize: "14px",
                        lineHeight: "16px",
                        minWidth: "25px",
                        height: "20px",
                        marginRight: "-14px",
                        backgroundColor: "black",
                        color: "white",
                      }}
                    >
                      0
                    </div>
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
