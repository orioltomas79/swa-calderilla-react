import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import type { CurrentAccount } from "../../api/types";
import apiClient from "../../api/apiClient";
import { Button } from "@mui/material";

const HomePage = () => {
  const navigate = useNavigate();

  const [apiResponse, setApiResponse] = useState<CurrentAccount[] | null>(null);

  const fetchApiData = async () => {
    try {
      const listCurrentAccounts =
        await apiClient.currentAccountEndpointsClient.getCurrentAccounts();
      setApiResponse(listCurrentAccounts);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    fetchApiData().catch((err) => console.error(err));
  }, []);

  const handleClick = () => {
    void navigate("/import");
  };

  return (
    <>
      <h1>Current accounts</h1>
      {apiResponse && (
        <ul>
          {apiResponse.map((account) => (
            <li key={account.id}>{account.name}</li>
          ))}
        </ul>
      )}
      <Button variant="contained" onClick={handleClick}>
        Import
      </Button>
    </>
  );
};
export default HomePage;
