import { useState } from "react";
import { AuthLayout } from "../components/AuthLayout/AuthLayout";
import { LoginForm } from "../components/LoginForm";

// OLD: This page is deprecated in BFF architecture
// Login is now handled by redirecting to /bff/login?returnUrl=<url>
// This route has been commented out in routes.jsx
// The BFF manages authentication via cookies and redirects

export default function LoginPage() {
  // HOOKS
  const [showPassword, setShowPassword] = useState(false);
  const [showError, setShowError] = useState(null);

  return (
    <div>
      <AuthLayout
        title={"Login"}
        redirect="/signup"
        redirectName="Sign Up"
        redirectTitle="New Guest"
        inputError={showError}
      >
        <LoginForm
          showPassword={showPassword}
          setShowPassword={setShowPassword}
          setShowError={setShowError}
        />
        <div style={{ margin: "10px 0" }}>
          <a
            href="/reset"
            style={{
              fontSize: "12px",
              lineHeight: "16.8px",
              textDecoration: "underline",
            }}
          >
            Forgot Password
          </a>
        </div>
      </AuthLayout>
    </div>
  );
}
