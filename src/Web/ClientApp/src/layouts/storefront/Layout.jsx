import { Outlet } from "react-router-dom";
import NavBar from "./components/Navbar/Navbar";
import Header from "./components/Header/Header";

export default function StoreFrontLayout() {
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
