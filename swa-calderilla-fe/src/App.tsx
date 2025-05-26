import { useEffect, useState } from "react";
import { MenuItem, TextField } from "@mui/material";
import "./App.css";
import MonthOperationsTable from "./features/MonthOperationsTable";
import { CurrentAccount } from "./api/types";
import apiClient from "./api/apiClient";
import UploadIngExtract from "./features/UploadIngExtract";
import UploadSabadellExtract from "./features/UploadSabadellExtract";

function App() {
  const [year, setYear] = useState(2025);
  const [month, setMonth] = useState(1);
  const [currentAccount, setCurrentAccount] = useState("");
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

  const handleIngFileUpload = async (file: File) => {
    try {
      await apiClient.ingEndpointsClient.uploadIngExtract(
        currentAccount,
        year,
        month,
        { data: file, fileName: file.name }
      );
    } catch (err) {
      console.error(err);
    }
  };

  const handleSabadellFileUpload = async (file: File) => {
    try {
      await apiClient.sabadellEndpointsClient.uploadSabadellExtract(
        currentAccount,
        year,
        month,
        { data: file, fileName: file.name }
      );
    } catch (err) {
      console.error(err);
    }
  };

  return (
    <>
      <div>
        <a href="/.auth/login/github">Login</a>
      </div>
      <div>
        <a href="/.auth/logout">Log out</a>
      </div>
      <h1>Calderilla</h1>
      {apiResponse && (
        <TextField
          id="currentAccountSelect"
          select
          label="Current Account"
          value={currentAccount}
          onChange={(e) => setCurrentAccount(e.target.value)}
          variant="outlined"
        >
          {apiResponse.map((account) => (
            <MenuItem key={account.id} value={account.id}>
              {account.name}
            </MenuItem>
          ))}
        </TextField>
      )}
      <TextField
        id="yearTextField"
        label="Year"
        variant="outlined"
        value={year}
        onChange={(e) => setYear(Number(e.target.value) || 0)}
      />
      <TextField
        id="monthTextField"
        label="Month"
        variant="outlined"
        value={month}
        onChange={(e) => setMonth(Number(e.target.value) || 0)}
      />
      <div>
        <UploadIngExtract
          onFileUpload={(file) => void handleIngFileUpload(file)}
        />
      </div>
      <div>
        <UploadSabadellExtract
          onFileUpload={(file) => void handleSabadellFileUpload(file)}
        />
      </div>
      <div className="card">
        <MonthOperationsTable
          currentAccount={currentAccount}
          year={year}
          month={month}
        />
      </div>
    </>
  );
}

export default App;
