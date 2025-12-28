import { z } from "zod";

export const registerSchema = z.object({
  username: z
    .string()
    .trim()
    .nonempty("Username is required")
    .max(128, "Username is too long"),

  email: z
    .string()
    .trim()
    .nonempty("Email is required")
    .max(128, "Email is too long"),

  password: z
    .string()
    .trim()
    .nonempty("Password is required")
    .min(6, "Invalid password, min length 6 digits"),
});
