import s from "../AuthLayout/auth.module.css";
export function AuthLayout({
  title,
  children,
  redirect,
  redirectName,
  redirectTitle,
}) {
  return (
    <>
      {/* -------- Navigation  -------- */}
      <nav className={s["navigation-container"]}>
        <div className={s["navigation"]}>
          <div className="flex items-center relative">
            <a href="/" className="mx-2 mb-2">
              <img
                src="src/assets/images/logo-brand-no-bg.png"
                style={{ height: "32px", width: "auto" }}
              />
            </a>
            <div style={{ fontSize: "22px" }}>{title}</div>
          </div>
          <div className="underline">Need Help?</div>
        </div>
      </nav>
      <div>
        <div className={s["background-image"]}>
          <div className={s["auth-container"]}>
            <div className={s["auth-with-form"]}>
              <div className={s["auth-with-form__header"]}>{title}</div>

              <div style={{ padding: "0 30px 30px" }}>
                {/* ================= MAIN ================= */}
                <main>{children}</main>
                {/* --------  SOCIAL LOGIN  -------- */}
                <div>
                  <div
                    className="flex items-center justify-center"
                    style={{ paddingBottom: "14px" }}
                  >
                    <div className={s["separate__spacing"]}></div>
                    <span className={s["separate__title"]}>OR</span>
                    <div className={s["separate__spacing"]}></div>
                  </div>
                  <div className={s["social-login"]}>
                    <button className={s["social-login__button"]}>
                      <div className={s["social-login__icon"]}>
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          width="22"
                          height="22"
                          viewBox="0 0 32 32"
                        >
                          <path
                            fill="currentColor"
                            d="M32 16c0-8.839-7.167-16-16-16C7.161 0 0 7.161 0 16c0 7.984 5.849 14.604 13.5 15.803V20.626H9.437v-4.625H13.5v-3.527c0-4.009 2.385-6.223 6.041-6.223c1.751 0 3.584.312 3.584.312V10.5h-2.021c-1.984 0-2.604 1.235-2.604 2.5v3h4.437l-.713 4.625H18.5v11.177C26.145 30.603 32 23.983 32 15.999z"
                          />
                        </svg>
                      </div>
                      <div>Facebook</div>
                    </button>
                    <button className={s["social-login__button"]}>
                      <div className={s["social-login__icon"]}>
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          width="22"
                          height="22"
                          viewBox="0 0 32 32"
                        >
                          <path
                            fill="currentColor"
                            d="M16.318 13.714v5.484h9.078c-.37 2.354-2.745 6.901-9.078 6.901c-5.458 0-9.917-4.521-9.917-10.099s4.458-10.099 9.917-10.099c3.109 0 5.193 1.318 6.38 2.464l4.339-4.182C24.251 1.584 20.641.001 16.318.001c-8.844 0-16 7.151-16 16s7.156 16 16 16c9.234 0 15.365-6.49 15.365-15.635c0-1.052-.115-1.854-.255-2.651z"
                          />
                        </svg>
                      </div>
                      <div>Google</div>
                    </button>
                  </div>
                </div>
              </div>
              {/* --------  REDIRECT  -------- */}
              <div className={s["auth-with-form__footer"]}>
                <div className="flex items-center justify-center">
                  {redirectTitle} ?&nbsp;
                  <a
                    href={redirect ? redirect : "/"}
                    className={s["auth-with-form__footer-link"]}
                  >
                    {redirectName}
                  </a>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
