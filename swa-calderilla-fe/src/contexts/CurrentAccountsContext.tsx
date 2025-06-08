import { createContext, useContext, useEffect, useState } from "react";
import type { ReactNode } from "react";
import type { CurrentAccount } from "../api/types";
import apiClient from "../api/apiClient";

interface CurrentAccountsContextType {
  listCurrentAccounts: CurrentAccount[] | null;
  loading: boolean;
  error: unknown;
}

const CurrentAccountsContext = createContext<CurrentAccountsContextType | undefined>(undefined);

// Custom hook to use context safely
export const useCurrentAccounts = () => {
  const context = useContext(CurrentAccountsContext);
  if (!context) {
    throw new Error("useCurrentAccounts must be used within a CurrentAccountsProvider");
  }
  return context;
};

export const CurrentAccountsProvider = ({ children }: { children: ReactNode }) => {
  const [listCurrentAccounts, setListCurrentAccounts] = useState<CurrentAccount[] | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<unknown>(null);

  useEffect(() => {
    let isMounted = true;
    const fetchAccounts = async () => {
      try {
        const accounts = await apiClient.currentAccountEndpointsClient.getCurrentAccounts();
        if (isMounted) {
          setListCurrentAccounts(accounts);
        }
      } catch (err) {
        if (isMounted) setError(err);
      } finally {
        if (isMounted) setLoading(false);
      }
    };
    fetchAccounts();
    return () => {
      isMounted = false;
    };
  }, []);

  return (
    <CurrentAccountsContext.Provider value={{ listCurrentAccounts, loading, error }}>
      {children}
    </CurrentAccountsContext.Provider>
  );
};
