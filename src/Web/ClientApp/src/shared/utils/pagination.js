//ref: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/from
//ref: https://taglineinfotech.com/blog/javascript-pagination/
export function getPageList(totalPage, currentPage, maxVisible = 5) {
  if (totalPage <= maxVisible) {
    return Array.from({ length: totalPage }, (_, i) => i + 1);
  }
  // Half range before and after current pag
  const half = Math.floor(maxVisible / 2);

  let start = currentPage - half;
  let end = currentPage + half;

  if (start < 1) {
    start = 1;
    end = maxVisible;
  }

  if (end > totalPage) {
    end = totalPage;
    start = totalPage - maxVisible + 1;
  }
  // Build page list
  return Array.from({ length: end - start + 1 }, (_, i) => start + i);
}
