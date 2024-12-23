import pk from "../../package.json";

export const BASE_URL =
  process.env.NODE_ENV === "development"
    ? "http://localhost:3000/statenet"
    : pk.homepage;

export const PROD_BASE_URL = pk.homepage;
