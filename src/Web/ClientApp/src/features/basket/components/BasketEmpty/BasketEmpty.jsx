import React from 'react';
import styles from './BasketEmpty.module.css';

export default function BasketEmpty() {
  return (
    <div className={styles['basket-empty']}>
      <div className={styles['basket-empty__inner']}>
        <div className={styles['basket-empty__illustration']} />
        <div className={styles['basket-empty__title']}>Your shopping cart is empty</div>
        <a href="/" className={styles['basket-empty__link']}>
          <button type="button" className={styles['basket-empty__button']}>
            <span className={styles['basket-empty__buttonText']}>Go Shopping Now</span>
          </button>
        </a>
      </div>
    </div>
  );
}
