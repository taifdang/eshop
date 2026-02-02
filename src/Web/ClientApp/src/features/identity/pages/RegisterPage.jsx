import { useState } from "react";
import { AuthLayout } from "../components/AuthLayout/AuthLayout";
import { RegisterForm } from "../components/RegisterForm";

// OLD: This page is deprecated in BFF architecture
// Registration is typically handled by the identity provider
// This route has been commented out in routes.jsx
// Users should register through the identity provider's registration flow

export default function RegisterPage() {
  // HOOKS
  const [showPassword, setShowPassword] = useState(false);
  const [showError, setShowError] = useState(null);

  return (
    <div>
      <AuthLayout
        title={"Sign Up"}
        redirect="/login"
        redirectName="Log In"
        redirectTitle="Have an account"
        inputError={showError}
      >
        <RegisterForm
          showPassword={showPassword}
          setShowPassword={setShowPassword}
          setShowError={setShowError}
        />
        <div style={{ height: "16px" }}></div>
      </AuthLayout>
    </div>
  );
}
