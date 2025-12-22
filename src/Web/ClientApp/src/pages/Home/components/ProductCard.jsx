export function ProductCard() {
  return (
    <>
      <div className="w-1/6" style={{ padding: "5px" }}>
        <div
          className="relative h-full"
          style={{ border: "1px solid rgba(0, 0, 0, 0.09)" }}
        >
          <a href="/detail" className="block h-full">
            <div className="flex flex-col h-full w-full bg-white">
              {/* -------- IMAGE  -------- */}
              <div className="relative">
                <div className="w-full aspect-square">
                  <img
                    src="src/assets/images/den.jpg"
                    alt=""
                    className="w-full h-full object-contain"
                  />
                </div>
                {/* -------- BADGE -------- */}
                <div className="absolute top-0 right-0 bg-[black] px-1 product-card__discount">
                  <span className="text-white text-xs">-33%</span>
                </div>
              </div>
              {/* -------- CONTENT -------- */}
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
    </>
  );
}
