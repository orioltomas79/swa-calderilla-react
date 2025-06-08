import { createContext } from "react";
import type { CurrentAccount } from "../api/types";

export interface CurrentAccountsContextType {
  listCurrentAccounts: CurrentAccount[] | null;
  loadingAccounts: boolean;
  error: unknown;
}

export const CurrentAccountsContext = createContext<CurrentAccountsContextType | undefined>(undefined);
