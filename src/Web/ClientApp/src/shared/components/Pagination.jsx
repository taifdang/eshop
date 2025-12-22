import clsx from "clsx";
import { getPageList } from "../utils/pagination";

export function Pagination({ currentPage, totalPage, onChange }) {
  const pages = getPageList(totalPage, currentPage);
  const isFirst = currentPage === 1;
  const isLast = currentPage === totalPage;

  return (
    <>
      <nav role="navigation" className="shop-page-controller">
        <button
          className="shop-icon-button shop-icon-button--left"
          disabled={isFirst}
          onClick={() => onChange(currentPage - 1)}
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
          >
            <path
              fill="none"
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="1.5"
              d="m15 5l-6 7l6 7"
            />
          </svg>
        </button>
        {/* -------- MAIN -------- */}
        {pages.map((item) => {
          const __selected = currentPage === item;
          return (
            <button
              key={item}
              className={clsx(
                "shop-button-no-outline",
                __selected ? "shop-button-solid--darken" : ""
              )}
              onClick={() => onChange(item)}
            >
              {item}
            </button>
          );
        })}
        {/*
          <button className="shop-button-no-outline shop-button-no-outline--no-click">
            ...
          </button>
        */}

        <button
          className="shop-icon-button shop-icon-button--right"
          disabled={isLast}
          onClick={() => onChange(currentPage + 1)}
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            width="24"
            height="24"
            viewBox="0 0 24 24"
          >
            <path
              fill="none"
              stroke="currentColor"
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="1.5"
              d="m9 5l6 7l-6 7"
            />
          </svg>
        </button>
      </nav>
    </>
  );
}
