import { useState } from "react";
import { TextField } from "@mui/material";
import "./App.css";
import MonthOperationsTable from "./features/MonthOperationsTable";

function App() {
  const [year, setYear] = useState(2025);
  const [month, setMonth] = useState(1);

  return (
    <>
      <div>
        <a href="/.auth/login/github">Login</a>
      </div>
      <div>
        <a href="/.auth/logout">Log out</a>
      </div>
      <h1>Calderilla</h1>
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
      <div className="card">
        <MonthOperationsTable year={year} month={month} />
      </div>
    </>
  );
}

export default App;
