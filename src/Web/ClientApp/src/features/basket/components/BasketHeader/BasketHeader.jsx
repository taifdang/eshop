import React from 'react';
import styles from './BasketHeader.module.css';

export default function BasketHeader() {
  return (
     <div className={styles.container}>
      <div className={`${styles['container-wrapper']} mx-auto`}>
        <div className="flex flex-col">
          <div className={styles['basket__header']}>
            <a className={styles['basket__logo']} href="/">
              <div></div>
              <img
                src="src/assets/images/logo-brand-no-bg.png"
                width="162px"
                height="50px"
                alt=""
              />
              <div className={styles['basket__title']}>Shoppping Cart</div>
            </a>
          </div>
        </div>
      </div>
    </div>
  );
}
