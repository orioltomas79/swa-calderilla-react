import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import type { CurrentAccount } from "../../api/types";
import apiClient from "../../api/apiClient";
import {
  Box,
  Grid,
  Toolbar,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
} from "@mui/material";
import CurrentAccountAnualSummary from "./CurrentAccountAnualSummay/CurrentAccountAnualSummary";
import TopMenu from "../TopMenu/TopMenu";

const HomePage = () => {
  const navigate = useNavigate();

  const { year } = useParams<{ year: string }>();

  const [listCurrentAccounts, setListCurrentAccounts] = useState<
    CurrentAccount[] | null
  >(null);

  const fetchApiData = async () => {
    try {
      const listCurrentAccounts =
        await apiClient.currentAccountEndpointsClient.getCurrentAccounts();
      setListCurrentAccounts(listCurrentAccounts);
    } catch (err) {
      console.error(err);
    }
  };

  useEffect(() => {
    fetchApiData().catch((err) => console.error(err));
  }, []);

  // Generate years from 2020 to current year
  const currentYear = new Date().getFullYear();
  const years = [];
  for (let y = currentYear; y >= 2020; y--) {
    years.push(y);
  }

  // Handle dropdown change
  const handleYearChange = (event: any) => {
    const selectedYear = event.target.value;
    navigate(`/home/${selectedYear}`);
  };

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <h1 style={{ margin: 0 }}>Current accounts</h1>
        <Box
          sx={{ display: "flex", alignItems: "center", gap: 2, mb: 2, mt: 1 }}
        >
          <FormControl size="small" sx={{ minWidth: 120 }}>
            <InputLabel id="year-select-label">Year</InputLabel>
            <Select
              labelId="year-select-label"
              id="year-select"
              value={year ? Number(year) : currentYear}
              label="Year"
              onChange={handleYearChange}
            >
              {years.map((y) => (
                <MenuItem key={y} value={y}>
                  {y}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>
        {listCurrentAccounts && (
          <Grid container spacing={2}>
            {listCurrentAccounts.map((account) => (
              <Grid key={account.id} size={{ xs: 12, sm: 12, md: 6 }}>
                <CurrentAccountAnualSummary
                  account={account}
                  year={Number(year)}
                />
              </Grid>
            ))}
          </Grid>
        )}
      </Box>
    </>
  );
};
export default HomePage;
