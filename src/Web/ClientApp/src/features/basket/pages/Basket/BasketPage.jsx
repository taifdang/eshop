import { NavBar } from "@/shared/components/layout/NavBar";
import { CartHeader } from "../../components/CartHeader";
import s from "./index.module.css";
import clsx from "clsx";
import BasketItem from "../../components/BasketItem";
import { useNavigate } from "react-router-dom";
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";
import { fetchBasket, updateBasket } from "../../services/basket-service";
import { useEffect, useState } from "react";
import { formatCurrency } from "@/shared/lib/currency";
import CartEmpty from "../../components/CartEmpty";
import { profileStorage } from "@/shared/storage/profile-storage";

export function BasketPage() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  //re-load page after isFetching
  const [showLoading, setShowLoading] = useState(false);
  const [showItemLoading, setShowItemLoading] = useState(false);
  const [hasError, setHasError] = useState({
    isError: false,
    id: null,
    message: "",
  });

  const {
    data: basket,
    isFetching,
    isFetched,
  } = useQuery({
    queryKey: ["basket"],
    queryFn: () => fetchBasket().then((res) => res.data),
    retry: false,
    refetchOnWindowFocus: false,
    initialData: {
      id: "",
      customerId: "",
      items: [],
      createdAt: new Date().toDateString(),
      lastModified: null,
    },
  });

  // mutation basket state
  const updateMutation = useMutation({
    mutationFn: ({ variantId, quantity }) => updateBasket(variantId, quantity),

    onMutate: async ({ variantId, quantity }) => {
      await queryClient.cancelQueries({ queryKey: ["basket"] });

      const previousBasket = queryClient.getQueryData(["basket"]);

      // optimistic update
      queryClient.setQueryData(["basket"], (old) => {
        if (!old) return old;

        return {
          ...old,
          items: old.items
            .map((item) =>
              item.productVariantId === variantId ? { ...item, quantity } : item
            )
            .filter((item) => item.quantity > 0),
        };
      });

      return { previousBasket, variantId };
    },
    onSuccess: () => {
      setHasError({
        isError: false,
        id: null,
        message: "",
      });
    },
    onError: (err, variables, context) => {
      if (context?.previousBasket) {
        queryClient.setQueryData(["basket"], context.previousBasket);
      }
      setHasError({
        isError: true,
        id: variables.variantId,
        message: "Invalid input",
      });
    },

    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: ["basket"] });
    },
  });

  // mutation price: reduce(func, initValue)
  const totalResult = basket.items?.reduce(
    (sum, item) => (sum = sum + item.regularPrice * item.quantity),
    0
  );

  // authentication
  useEffect(() => {
    if (profileStorage.get() === null || undefined) {
      navigate("/login");
    }
  }, [basket]);

  // checkout func
  const handleCheckout = () => {
    navigate("/checkout");
  };

  const isFirstLoad = isFetching && !isFetched;

  // item load
  useEffect(() => {
    setShowItemLoading(true);
    const timer = setTimeout(() => {
      setShowItemLoading(false);
    }, 350);

    return () => clearTimeout(timer);
  }, [isFetching]);

  // page load
  useEffect(() => {
    setShowLoading(true);
    const timer = setTimeout(() => {
      setShowLoading(false);
    }, 1000);

    return () => clearTimeout(timer);
  }, [isFetched]);

  if (isFirstLoad || showLoading) {
    return (
      <div className="flex items-center justify-center min-h-screen text-gray-600 text-lg">
        <span className="ml-2 flex items-end gap-1">
          <span className="dot-wave step-1" />
          <span className="dot-wave step-2" />
          <span className="dot-wave step-3" />
        </span>
      </div>
    );
  }

  return (
    <div>
      <NavBar />
      <div>
        <CartHeader />
        <div className="mx-auto w-[1200px]">
          {basket && basket.items.length === 0 ? (
            <>
              <CartEmpty />
            </>
          ) : (
            <>
              {/* table section */}
              <div className="flex flex-col pt-[20px]">
                {/* Table header */}
                <div className={s["basket__table-header"]}>
                  <div
                    className={clsx(
                      s["div-checkbox"],
                      s["table-col--checkbox"]
                    )}
                  >
                    <label htmlFor="">
                      <input type="text" hidden />
                      <div className={s["div-checkbox-wrap-input"]}></div>
                    </label>
                  </div>
                  <div
                    className={clsx(s["table-col"], s["table-col--product"])}
                  >
                    Product
                  </div>
                  <div
                    className={clsx(
                      s["table-col"],
                      s["table-col--unit"],
                      "text-center"
                    )}
                  >
                    Unit Price
                  </div>
                  <div
                    className={clsx(
                      s["table-col"],
                      s["table-col--quantity"],
                      "text-center"
                    )}
                  >
                    Quantity
                  </div>
                  <div
                    className={clsx(
                      s["table-col"],
                      s["table-col--total"],
                      "text-center"
                    )}
                  >
                    Total Price
                  </div>
                  <div
                    className={clsx(
                      s["table-col"],
                      s["table-col--actions"],
                      "text-center"
                    )}
                  >
                    Actions
                  </div>
                </div>
                {/* Table content */}
                <div className={s["basket__table-content"]}>
                  <section className={s["table-content__section"]}>
                    {/* Title */}
                    <div className={s["table-content__title"]}>
                      <span>Items: {showItemLoading ? "0" : basket.items.length}</span>
                    </div>
                    {/* CartItem */}
                    <div>
                      {basket && basket.items.length === 0 ? (
                        <>
                          <span></span>
                        </>
                      ) : (
                        <>
                          {basket.items.map((item, index) => (
                            <>
                              <BasketItem
                                key={item.id}
                                item={item}
                                error={
                                  hasError.isError &&
                                  hasError.id === item.productVariantId
                                }
                                errorMessage={hasError.message}
                                isUpdating={
                                  showItemLoading &&
                                  updateMutation.variables?.variantId ===
                                    item.productVariantId
                                }
                                onUpdate={(quantity) =>
                                  updateMutation.mutate({
                                    variantId: item.productVariantId,
                                    quantity,
                                  })
                                }
                              />
                              {index < basket.items.length - 1 && (
                                <div
                                  className={s["basket__item-divider"]}
                                ></div>
                              )}
                            </>
                          ))}
                        </>
                      )}
                    </div>
                  </section>
                </div>
              </div>
              {/* basket footer */}
              <section className={s["basket__footer"]}>
                {/* promotion */}
                <div className={s["basket__footer-promotion"]}>
                  <img
                    style={{ marginRight: "8px" }}
                    src="src/assets/images/voucher_icon.svg"
                  />
                  <div>Platform voucher</div>
                  <div className="flex-1"></div>
                  <button className={s["basket__footer-promotion-button"]}>
                    Select or enter code
                  </button>
                </div>
                {/*  */}
                <div className={s["basket__footer-divider"]}></div>
                {/* total */}
                <div className={s["basket__footer-total"]}>
                  {/* selection */}
                  <div
                    className={clsx(
                      s["div-checkbox"],
                      s["table-col--checkbox"]
                    )}
                  >
                    <label htmlFor="">
                      <input type="text" hidden />
                      <div className={s["div-checkbox-wrap-input"]}></div>
                    </label>
                  </div>
                  <button className={s["basket__footer--selected"]}>
                    Select All (0)
                  </button>
                  <button className={s["basket__footer--unselected"]}>
                    Delete
                  </button>
                  {/*  */}
                  <div></div>
                  {/* text */}
                  <div className="flex-1"></div>
                  <div className="flex flex-col">
                    <div className="flex items-center flex-end">
                      <div
                        className={clsx(
                          "flex items-center",
                          s["basket__footer-total-title"]
                        )}
                      >
                        Total ({showItemLoading ? "0" : basket.items.length}{" "}
                        item):
                      </div>
                      <div className={clsx(s["basket__footer-total-subtitle"])}>
                        {showItemLoading ? "0₫" : formatCurrency(totalResult)}
                      </div>
                    </div>
                  </div>
                  {/* button */}
                  <div>
                    <button
                      onClick={() => handleCheckout()}
                      className={s["basket__footer-button"]}
                    >
                      <span className={s["basket__footer-button-title"]}>
                        Checkout
                      </span>
                    </button>
                  </div>
                </div>
              </section>
            </>
          )}
        </div>
      </div>
    </div>
  );
}
