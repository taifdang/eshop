import { useState } from "react";
import { AuthLayout } from "../components/AuthLayout/AuthLayout";
import { LoginForm } from "../components/LoginForm";
export default function LoginPage() {
  // HOOKS
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div>
      <AuthLayout
        title={"Login"}
        redirect="/signup"
        redirectName="Sign Up"
        redirectTitle="New Guest"
      >
        <LoginForm
          showPassword={showPassword}
          setShowPassword={setShowPassword}
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
