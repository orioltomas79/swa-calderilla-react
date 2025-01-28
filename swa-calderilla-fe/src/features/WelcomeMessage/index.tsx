/* eslint-disable @typescript-eslint/no-misused-promises */
import { useState } from "react";
import apiClient from "../../api/apiClient";

const WelcomeMessage = () => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [apiResponse, setApiResponse] = useState<string | null>(null);

  const fetchApiData = async () => {
    setLoading(true);
    setError(null); // Clear previous errors
    try {
      const message = await apiClient.devEndpointsClient.getHelloWorldMessage();
      setApiResponse(message);
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
        <pre style={{ textAlign: "left", marginTop: "1rem" }}>
          {apiResponse}
        </pre>
      )}
    </div>
  );
};

export default WelcomeMessage;
