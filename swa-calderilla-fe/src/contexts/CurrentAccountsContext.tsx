import { useEffect, useState, useMemo } from "react";
import type { ReactNode } from "react";
import apiClient from "../api/apiClient";
import { CurrentAccountsContext } from "./CurrentAccountsContextDef";
import type { CurrentAccountsContextType } from "./CurrentAccountsContextDef";

/**
 * Provides the current accounts context to its child components.
 *
 * This provider is responsible for fetching the list of current accounts from the API
 * when it mounts, and exposes the list, loading state, and any error encountered
 * during the fetch via context.
 *
 * @param children - The child components that will have access to the current accounts context.
 *
 * @remarks
 * The `useMemo` hook is used to memoize the `contextValue` object. This is important because
 * React context only triggers re-renders in consumers when the context value changes by reference.
 * Without `useMemo`, a new object would be created on every render, causing unnecessary re-renders
 * of all consumers even if the values inside the context haven't changed.
 */
export const CurrentAccountsProvider = ({ children }: { children: ReactNode }) => {
  const [listCurrentAccounts, setListCurrentAccounts] =
    useState<CurrentAccountsContextType["listCurrentAccounts"]>(null);
  const [loadingAccounts, setLoading] = useState<CurrentAccountsContextType["loadingAccounts"]>(true);
  const [error, setError] = useState<CurrentAccountsContextType["error"]>(null);

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
    void fetchAccounts();
    return () => {
      isMounted = false;
    };
  }, []);

  const contextValue = useMemo(
    () => ({ listCurrentAccounts, loadingAccounts, error }),
    [listCurrentAccounts, loadingAccounts, error]
  );

  return <CurrentAccountsContext value={contextValue}>{children}</CurrentAccountsContext>;
};
