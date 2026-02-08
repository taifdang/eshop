import styles from './ProductSummary.module.css';

export function ProductPrice({ price, regularPrice, discount }) {
  return (
    <div className={styles['product-info__price-container']}>
      <div className={styles['product-info__card']}>
        <section className={styles['product-info__section']}>
          <div className={styles['product-info__amount']}>{price}</div>
          <div className={styles['product-info__regular']}>{regularPrice}</div>
          {discount && (
            <div className={styles['product-info__discount']}>{discount}</div>
          )}
        </section>
      </div>
    </div>
  );
}
