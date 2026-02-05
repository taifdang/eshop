import { z } from "zod";

// Create schema validation
export const shippingAddressSchema = z.object({
  fullname: z
    .string()
    .trim()
    .nonempty("Full name is required")
    .min(2, "Name is too short."),
  phoneNumber: z
    .string()
    .trim()
    .nonempty("Phone number is required")
    .regex(/^(0|\+84)[0-9]{9}$/, "Invalid phone number"),
  city: z.string().trim().nonempty("City is required"),
  zipCode: z
    .string()
    .trim()
    .regex(/^\d{5,6}$/, "Must be 5–6 digits"),
  street: z.string().trim().min(5, "Street address is too short"),
});
