import styles from './ProductInfo.module.css';

export function DisplayName({ name }) {
  return (
    <div className="line-clamp-2">
      <h1 className={styles['product-info__title']}>{name}</h1>
    </div>
  );
}
