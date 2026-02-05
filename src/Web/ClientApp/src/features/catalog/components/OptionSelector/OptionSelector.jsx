import { usePreview } from "../../contexts/PreviewContext";
import s from "./OptionSelector.module.css";
import clsx from "clsx";
import fallbackImage from "@/assets/images/default.jpg";
import { Checkbox } from "@/shared/components";

export function OptionSelector({ options, selectedOption, onChange }) {
  // context
  const { setImage } = usePreview();

  return (
    <div>
      {options.map((o,index) => {
        //
        const __isMax = o.values.length > 20;
        //
        return (
          <section key={o.id} className={s["option-section"]}>
            <h2 className={s["option__title"]}>{o.title}</h2>
            <div
              className={clsx(
                s["option-area"],
                __isMax && s["option-area-view"]
              )}
            >
              {o.values.map((ov) => {
                //
                const __selected = selectedOption[o.id] === ov.id;
                const displayImage = ov?.image?.url ?? fallbackImage;
                //
                return (
                  <div key={ov.id}>
                    <Checkbox
                      checked={__selected}
                      onChange={() => onChange(o.id, ov.id)}
                      onMouseEnter={() => {
                        if (ov.image) setImage(displayImage);
                      }}
                      onMouseLeave={() => {}}
                      className={clsx(
                        !ov.image
                          ? s["option-value-no-image"]
                          : s["option-value-with-image"],
                        s["option-value__button"]
                      )}
                    >
                      {ov.image && (
                        <div className={s["option-value__image"]}>
                          <img src={displayImage} loading="lazy" />
                        </div>
                      )}
                      <span className={s["option-value__title"]}>
                        {ov.value}
                      </span>
                    </Checkbox>
                  </div>
                );
              })}
            </div>
          </section>
        );
      })}
    </div>
  );
}
