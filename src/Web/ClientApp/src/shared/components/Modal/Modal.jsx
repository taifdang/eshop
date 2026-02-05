// ref: https://www.w3schools.com/react/react_portals.asp
// ref: https://react.dev/reference/react-dom/createPortal
import { createPortal } from "react-dom";
import s from "./Modal.module.css";

export default function Modal({ open, onClose, children }) {
  const modalRoot = document.getElementById("modal");

  if (!modalRoot) return null;
  if (!open) return null;
  return createPortal(
    <div tabIndex="0" className={s["modal-backdrop"]}>
      <div
        role="dialog"
        className={s["modal-content"]}
        onClick={(e) => e.stopPropagation()}
      >
        {children}
      </div>
    </div>,
    modalRoot
  );
}

