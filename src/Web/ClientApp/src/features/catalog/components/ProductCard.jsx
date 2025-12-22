export default function ProductCard() {
  return (
    <div className="w-1/6" style={{ padding: "5px" }}>
      <div
        className="relative h-full"
        style={{ border: "1px solid rgba(0, 0, 0, 0.09)" }}
      >
        <a href="/detail" className="block h-full">
          <div className="flex flex-col h-full w-full bg-white">
            {/* Image */}
            <div className="relative">
              <div className="w-full aspect-square">
                <img
                  src="src/assets/images/den.jpg"
                  alt=""
                  className="w-full h-full object-contain"
                />
              </div>

              {/* Discount badge */}
              <div className="absolute top-0 right-0 bg-[black] px-1 product-card__discount">
                <span className="text-white text-xs">-33%</span>
              </div>
            </div>

            {/* Content */}
            <div className="flex flex-col flex-1 p-2 justify-between">
              <div className="text-sm line-clamp-2">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                Consequuntur velit veritatis nulla labore eaque nesciunt ex a
                illum culpa!
              </div>

              <div className="flex justify-between items-center mt-2">
                <div className="flex items-center">
                  <span className="text-base">10.000</span>
                  <span className="pl-1 text-[10px] underline">đ</span>
                </div>

                <span className="text-xs text-gray-500">Sold 123</span>
              </div>
            </div>
          </div>
        </a>
      </div>
    </div>
  );
}
{
  /* ================= IF USE BOOTSTRAP ================= */
}
{
  <div className="col-xs-2">
    <div className="product-card">
      <div className="border h-100 position-relative">
        <a href="/detail">
          <div className="d-flex flex-column h-100 w-100 bg-white">
            <div>
              <div className="w-100 product-card__image">
                <img
                  src="src\assets\images\den.jpg"
                  alt=""
                  className="w-100 h-100 object-fit-contain"
                />
              </div>
              <div className="position-absolute p-0 top-0 end-0 bg-dark product-card__discount">
                <span className="text-white product-card__discount-content">
                  -33%
                </span>
              </div>
            </div>
            <div className="d-flex flex-column h-100 p-2 justify-content-between">
              <div className="card-content_title">
                <span className="line-clamp-2">
                  Lorem ipsum dolor sit amet, consectetur adipisicing elit.
                  Consequuntur velit veritatis nulla labore eaque nesciunt ex a
                  illum culpa! Optio quibusdam impedit esse necessitatibus
                  distinctio incidunt ullam molestiae consequuntur qui.
                </span>
              </div>
              <div className="d-flex flex-row justify-content-between">
                <div className="d-flex align-items-center">
                  <span style={{ fontSize: "16px" }}>10.000</span>
                  <span className="ps-1" style={{ fontSize: "10px" }}>
                    <u>đ</u>
                  </span>
                </div>
                <div>
                  <span style={{ fontSize: "12px" }}>Sold 123</span>
                </div>
              </div>
            </div>
          </div>
        </a>
      </div>
    </div>
  </div>;
}
