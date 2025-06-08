import { use } from "react";
import { CurrentAccountsContext } from "./CurrentAccountsContextDef";

export const useCurrentAccounts = () => {
  const context = use(CurrentAccountsContext);
  if (!context) {
    throw new Error("useCurrentAccounts must be used within a CurrentAccountsProvider");
  }
  return context;
};
