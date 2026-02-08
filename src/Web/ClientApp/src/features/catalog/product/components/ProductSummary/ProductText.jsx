import styles from './ProductSummary.module.css';

export function ProductText({ name }) {
  return (
    <div className="line-clamp-2">
      <h1 className={styles['product-info__title']}>{name}</h1>
    </div>
  );
}
