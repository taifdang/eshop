import fallbackImage from "@/assets/images/default.jpg";
import { useAuth } from "@/features/identity/contexts/AuthContext";
import { profileStorage } from "../../storage/profile-storage";
import { useQueryClient } from "@tanstack/react-query";
import { logout } from "../../../features/identity/services/user-service";

export function NavBar() {
  //const { logout } = useAuth();
  
  const user = profileStorage.get();
  const queryClient = useQueryClient();

  const handleLogout = async () => {
    try {
      await logout();
      profileStorage.clear();
    } catch (error) {
      console.error("Logout failed:", error);
    }
  };

  
  //const cart = queryClient.getQueryData("basket");
  
  return (
    <div className="bg-white">
      <section
        className="container-wrapper mx-auto"
        style={{
          fontSize: "14px",
          lineHeight: "16.8px",
        }}
      >
        <div className="flex flex-col flex-1">
          <div className="flex flex-row flex-1 justify-end">
            <div className="flex flex-end">
              <ul className="flex items-center m-0" style={{ height: "34px" }}>
                {/* <li>
                  <a href="/notification">
                    <span>
                      <i class="bi bi-bell"></i>
                    </span>
                    <span style={{ marginLeft: "5px" }}>Notifications</span>
                  </a>
                </li>
                <li>
                  <a href="/support" style={{ paddingRight: "10px" }}>
                    <span style={{ margin: "0 5px 0 8px" }}>
                      <i class="bi bi-question-circle"></i>
                    </span>
                    <span className="ms-1">Help</span>
                  </a>
                </li> */}
                <li>
                  <a
                    href="/language"
                    className="flex items-center"
                    style={{ padding: "7px 10px" }}
                  >
                    <span>
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        width="16"
                        height="16"
                        viewBox="0 0 24 24"
                      >
                        <g fill="none" stroke="currentColor" strokeWidth="1.5">
                          <path d="M22 12a10 10 0 1 1-20.001 0A10 10 0 0 1 22 12Z" />
                          <path d="M16 12c0 1.313-.104 2.614-.305 3.827c-.2 1.213-.495 2.315-.867 3.244c-.371.929-.812 1.665-1.297 2.168c-.486.502-1.006.761-1.531.761s-1.045-.259-1.53-.761c-.486-.503-.927-1.24-1.298-2.168c-.372-.929-.667-2.03-.868-3.244A23.6 23.6 0 0 1 8 12c0-1.313.103-2.614.304-3.827s.496-2.315.868-3.244c.371-.929.812-1.665 1.297-2.168C10.955 2.26 11.475 2 12 2s1.045.259 1.53.761c.486.503.927 1.24 1.298 2.168c.372.929.667 2.03.867 3.244C15.897 9.386 16 10.687 16 12Z" />
                          <path strokeLinecap="round" d="M2 12h20" />
                        </g>
                      </svg>
                    </span>
                    <span
                      style={{
                        margin: "0 5px",
                        fontSize: "13px",
                        lineHeight: "15.6px",
                        fontWeight: 300,
                      }}
                    >
                      English
                    </span>
                  </a>
                </li>
                {/* USER */}
                {user ? (
                  <li style={{ padding: "0 10px" }}>
                    <div className="flex items-center">
                      <a href="/user" className="flex relative">
                        <div
                          className="flex items-center"
                          style={{ padding: "5px 0" }}
                        >
                          <div>
                            <img
                              src={fallbackImage}
                              alt=""
                              className="user__avatar"
                            />
                          </div>
                          <span style={{ padding: "0 10px" }}>
                            {user?.userName || "guest"}
                          </span>
                          <div
                            className="block"
                            style={{
                              height: "13px",
                              borderLeft: "1px solid black",
                            }}
                          ></div>
                        </div>
                      </a>
                      {/* OLD: onClick logout with local navigation
                      <a
                        onClick={() => logout()}
                        className="flex relative"
                        style={{ padding: "0 10px", cursor: "pointer" }}
                      >
                        Logout
                      </a>
                      */}
                      {/* NEW: BFF logout endpoint */}
                      <a
                        onClick={() => { handleLogout(); }}
                        className="flex relative"
                        style={{ padding: "0 10px", cursor: "pointer" }}
                      >
                        Logout
                      </a>
                    </div>
                  </li>
                ) : (
                  <>
                    {/* GUEST */}
                    {/* OLD: Local signup and login routes
                    <a
                      href="/signup"
                      className="flex relative"
                      style={{ padding: "0 10px" }}
                    >
                      Sign up
                    </a>
                    <div
                      className="block"
                      style={{ height: "13px", borderLeft: "1px solid black" }}
                    ></div>
                    <a
                      href="/login"
                      className="flex items-center relative"
                      style={{ padding: "0 10px" }}
                    >
                      Login
                    </a>
                    */}
                    {/* NEW: BFF login endpoint - signup typically handled by identity provider */}
                    <a
                      href={`https://localhost:5002/bff/login?returnUrl=/`}
                      className="flex items-center relative"
                      style={{ padding: "0 10px" }}
                    >
                      Login
                    </a>
                  </>
                )}
                {/* <li style={{ padding: "0 10px" }}>
                  <a href="/user" className="flex relative">
                    <div
                      className="flex items-center"
                      style={{ padding: "5px 0" }}
                    >
                      <div>
                        <img
                          src={fallbackImage}
                          alt=""
                          className="user__avatar"
                        />
                      </div>
                      <span style={{ padding: "0 10px" }}>
                        {user?.userName || "guest"}
                      </span>
                      <div
                        className="block"
                        style={{
                          height: "13px",
                          borderLeft: "1px solid black",
                        }}
                      ></div>
                      <a
                        href="/signup"
                        className="flex relative"
                        style={{ padding: "0 10px" }}
                      >
                        Logout
                      </a>
                    </div>
                  </a>
                </li> */}
              </ul>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}
