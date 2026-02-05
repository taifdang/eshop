import { useLocation, Outlet } from "react-router-dom";
import { NavBar } from "./components/Navbar";
import { Header } from "./components/Header";

export default function UserLayout() {
  const __location = useLocation();

  const renderHeader = () => {
    if (__location.pathname === "/cart") return <BasketHeader />;
    if (__location.pathname === "/checkout") return <CheckoutHeader />;
    return <Header />;
  };

  return (
    <div id="main">
      <div className="flex flex-col">
        <header>
          <NavBar />
          <Header />
        </header>
        <main style={{ marginTop: "120px" }}>
          <Outlet />
        </main>
      </div>
    </div>
  );
}
