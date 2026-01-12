import s from "./ProductDescription.module.css";

export function Description({ category, description }) {
  return (
    <div className="container-wrapper">
      <div className="section-wrapper">
        <div style={{ margin: "0 auto", backgroundColor: "rgb(255,255,255)" }}>
          <div className={s["description__section"]}>
            {/* Detail section*/}
            <section>
              <h2 className={s["description__title"]}>DETAIL</h2>
              <div className={s["description__text"]}>
                <div className={s["description__column"]}>
                  <h3 className={s["description__column-title"]}>Category</h3>
                  <div>{category}</div>
                </div>
              </div>
            </section>
            {/* Description section*/}
            <section>
              <h2 className={s["description__title"]}>DESCRIPTION</h2>
              <div className={s["description__text"]}>
                <p>{description}</p>
              </div>
            </section>
          </div>
        </div>
      </div>
    </div>
  );
}
