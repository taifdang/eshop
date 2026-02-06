import { createContext, useContext } from "react";
import clsx from "clsx";
import s from "./Table.module.css";

const TableContext = createContext({});

export function Table({ children, className }) {
  return (
    <div className={clsx(s.table, className)} role="table">
      {children}
    </div>
  );
}

export function TableHeader({ children, className }) {
  return (
    <div className={clsx(s["table-header"], className)} role="rowgroup">
      <div className={clsx(s["table-row"], s["table-header-row"])} role="row">
        {children}
      </div>
    </div>
  );
}

export function TableBody({ children, className }) {
  return (
    <div className={clsx(s["table-body"], className)} role="rowgroup">
      {children}
    </div>
  );
}

export function TableRow({ children, className, ...props }) {
  return (
    <div className={clsx(s["table-row"], className)} role="row" {...props}>
      {children}
    </div>
  );
}

export function TableCell({ children, className, flex, align = "left", ...props }) {
  const style = flex ? { flex } : undefined;
  
  return (
    <div
      className={clsx(
        s["table-cell"],
        align === "center" && s["table-cell--center"],
        align === "right" && s["table-cell--right"],
        className
      )}
      role="cell"
      style={style}
      {...props}
    >
      {children}
    </div>
  );
}

export function TableHeaderCell({ children, className, flex, align = "left", ...props }) {
  const style = flex ? { flex } : undefined;
  
  return (
    <div
      className={clsx(
        s["table-cell"],
        s["table-header-cell"],
        align === "center" && s["table-cell--center"],
        align === "right" && s["table-cell--right"],
        className
      )}
      role="columnheader"
      style={style}
      {...props}
    >
      {children}
    </div>
  );
}
