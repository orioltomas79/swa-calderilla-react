import apiClient from "./api/apiClient";
import { useEffect, useState } from "react";
import type { CurrentAccount } from "./api/types";
import Button from "@mui/material/Button";
import { useNavigate } from "react-router-dom";

function App() {
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
    navigate("/hello");
  };

  return (
    <>
      <div>
        <a href="/.auth/login/github">Login</a>
      </div>
      <div>
        <a href="/.auth/logout">Log out</a>
      </div>
      <h1>Vite + React</h1>
      {apiResponse && (
        <ul>
          {apiResponse.map((account) => (
            <li key={account.id}>{account.name}</li>
          ))}
        </ul>
      )}
      <Button variant="contained" onClick={handleClick}>Hello world</Button>
    </>
  );
}

export default App;
