/* eslint-disable @typescript-eslint/no-misused-promises */
import { useState } from "react";
import apiClient from "../../api/apiClient";
import { Operation } from "../../api/types";

const WelcomeMessage = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [apiResponse, setApiResponse] = useState<Operation[] | null>(null);

  const fetchApiData = async () => {
    setLoading(true);
    setError(null); // Clear previous errors
    try {
      await apiClient.operationsEndpointsClient.addOperation();
      const listOperations =
        await apiClient.operationsEndpointsClient.getOperations(2025, 2);
      setApiResponse(listOperations);
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message || "An unexpected error occurred");
      } else {
        setError("An unexpected error occurred");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <button onClick={fetchApiData} disabled={loading}>
        {loading ? "Loading..." : "Fetch API Data"}
      </button>
      {error && <p style={{ color: "red" }}>Error: {error}</p>}
      {apiResponse && (
        <div style={{ textAlign: "left", marginTop: "1rem" }}>
          {apiResponse.map((operation) => (
            <div key={operation.id} style={{ marginBottom: "1rem" }}>
              <p>
                <strong>ID:</strong> {operation.id}
              </p>
              <p>
                <strong>Amount:</strong> {operation.amount}
              </p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default WelcomeMessage;
