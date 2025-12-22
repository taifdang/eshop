import { useState } from "react";
import { AuthLayout } from "../components/AuthLayout/AuthLayout";
import { RegisterForm } from "../components/RegisterForm";
export default function RegisterPage() {
  // HOOKS
  const [showPassword, setShowPassword] = useState(false);

  return (
    <div>
      <AuthLayout
        title={"Sign Up"}
        redirect="/login"
        redirectName="Log In"
        redirectTitle="Have an account"
      >
        <RegisterForm
          showPassword={showPassword}
          setShowPassword={setShowPassword}
        />
        <div style={{ height: "16px" }}></div>
      </AuthLayout>
    </div>
  );
}