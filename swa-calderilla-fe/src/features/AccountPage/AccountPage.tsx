import {
  Box,
  Toolbar,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Paper,
  CircularProgress,
} from "@mui/material";
import { useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";

import { useEffect, useState } from "react";
import apiClient from "../../api/apiClient";
import type { OperationTypeSummary } from "../../api/apiClient.g.nswag";

const AccountMonthPage = () => {
  const { accountId, year } = useParams<{
    accountId: string;
    year: string;
  }>();

  const [data, setData] = useState<OperationTypeSummary[] | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    if (!accountId || !year) return;
    setLoading(true);
    setError(null);
    apiClient.currentAccountEndpointsClient
      .getCurrentAccountYearlyDetails(accountId, Number(year))
      .then((res) => setData(res?.types ?? null))
      .catch(() => setError("Failed to load data"))
      .finally(() => setLoading(false));
  }, [accountId, year]);

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <div>
          <div>
            <strong>Account ID:</strong> {accountId}
          </div>
          <div>
            <strong>Year:</strong> {year}
          </div>
        </div>
        {loading ? (
          <Box sx={{ display: "flex", justifyContent: "center", mt: 2 }}>
            <CircularProgress />
          </Box>
        ) : error ? (
          <Box color="error.main" sx={{ mt: 2 }}>
            {error}
          </Box>
        ) : data && data.length > 0 ? (
          <TableContainer component={Paper} sx={{ mt: 3 }}>
            <Table size="small">
              <TableHead>
                <TableRow>
                  <TableCell>Type</TableCell>
                  <TableCell align="right">Jan</TableCell>
                  <TableCell align="right">Feb</TableCell>
                  <TableCell align="right">Mar</TableCell>
                  <TableCell align="right">Apr</TableCell>
                  <TableCell align="right">May</TableCell>
                  <TableCell align="right">Jun</TableCell>
                  <TableCell align="right">Jul</TableCell>
                  <TableCell align="right">Aug</TableCell>
                  <TableCell align="right">Sep</TableCell>
                  <TableCell align="right">Oct</TableCell>
                  <TableCell align="right">Nov</TableCell>
                  <TableCell align="right">Dec</TableCell>
                  <TableCell align="right">Total</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {data.map((row, idx) => (
                  <TableRow key={row.type ?? idx}>
                    <TableCell>{row.type ?? "-"}</TableCell>
                    <TableCell align="right">{row.jan ?? "-"}</TableCell>
                    <TableCell align="right">{row.feb ?? "-"}</TableCell>
                    <TableCell align="right">{row.mar ?? "-"}</TableCell>
                    <TableCell align="right">{row.apr ?? "-"}</TableCell>
                    <TableCell align="right">{row.may ?? "-"}</TableCell>
                    <TableCell align="right">{row.jun ?? "-"}</TableCell>
                    <TableCell align="right">{row.jul ?? "-"}</TableCell>
                    <TableCell align="right">{row.aug ?? "-"}</TableCell>
                    <TableCell align="right">{row.sep ?? "-"}</TableCell>
                    <TableCell align="right">{row.oct ?? "-"}</TableCell>
                    <TableCell align="right">{row.nov ?? "-"}</TableCell>
                    <TableCell align="right">{row.dec ?? "-"}</TableCell>
                    <TableCell align="right">{row.total ?? "-"}</TableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        ) : (
          <Box sx={{ mt: 2 }}>No data available.</Box>
        )}
      </Box>
    </>
  );
};

export default AccountMonthPage;
