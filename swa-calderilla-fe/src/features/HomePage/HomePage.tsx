import { useParams, useNavigate } from "react-router-dom";
import { useCurrentAccounts } from "../../contexts/CurrentAccountsContext";
// import type { CurrentAccount } from "../../api/types";
import { Box, Grid, Toolbar, Select, MenuItem, FormControl, InputLabel, type SelectChangeEvent } from "@mui/material";
import CurrentAccountAnualSummary from "./CurrentAccountAnualSummay/CurrentAccountAnualSummary";
import TopMenu from "../TopMenu/TopMenu";

const HomePage = () => {
  const navigate = useNavigate();

  const { year } = useParams<{ year: string }>();

  const { listCurrentAccounts, loading, error } = useCurrentAccounts();

  // Generate years from 2020 to current year
  const currentYear = new Date().getFullYear();
  const years = [];
  for (let y = currentYear; y >= 2020; y--) {
    years.push(y);
  }

  // Handle dropdown change
  const handleYearChange = (event: SelectChangeEvent) => {
    const selectedYear = event.target.value;
    void navigate(`/home/${selectedYear}`); // explicitly ignore the promise
  };

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <h1 style={{ margin: 0 }}>Current accounts</h1>
        <Box sx={{ display: "flex", alignItems: "center", gap: 2, mb: 2, mt: 1 }}>
          <FormControl size="small" sx={{ minWidth: 120 }}>
            <InputLabel id="year-select-label">Year</InputLabel>
            <Select
              labelId="year-select-label"
              id="year-select"
              value={year ? year : String(currentYear)}
              label="Year"
              onChange={handleYearChange}
            >
              {years.map((y) => (
                <MenuItem key={y} value={String(y)}>
                  {y}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>
        {loading && <div>Loading accounts...</div>}
        {Boolean(error) && <div>Error loading accounts: {String(error)}</div>}
        {listCurrentAccounts && (
          <Grid container spacing={2}>
            {listCurrentAccounts.map((account) => (
              <Grid key={account.id} size={{ xs: 12, sm: 12, md: 6 }}>
                <CurrentAccountAnualSummary account={account} year={Number(year ?? currentYear)} />
              </Grid>
            ))}
          </Grid>
        )}
      </Box>
    </>
  );
};
export default HomePage;
