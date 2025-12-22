const ProductDescription = ({ category, description }) => {
  return (
    <div className="section-wrapper container-wrapper mx-auto bg-white">
      <div className="page-product__content">
        <div className="product-detail page-product__detail">
          {/* DETAIL */}
          <section>
            <h2 className="title-wrapper">DETAIL</h2>
            {/* ITEMS */}
            <div className="product-detail-info">
              <div className="flex">
                <h3
                  style={{
                    width: "188px",
                    paddingRight: "12px",
                    marginBottom: "18px",
                    fontSize: "14px",
                    fontWeight: 400,
                    color: "rgba(0, 0, 0, 0.8)",
                  }}
                >
                  Category
                </h3>
                <div>{category}</div>
              </div>
            </div>
          </section>
          {/* DESCRIPTION */}
          <section>
            <h2 className="title-wrapper">DESCRIPTION</h2>
            <div className="product-detail-info">
              <p>{description}</p>
            </div>
          </section>
        </div>
      </div>
    </div>
  );
};

export default ProductDescription;
