import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import type { CurrentAccount, GetCurrentAccountYearlySummaryResponse } from "../../../api/types";
import { useEffect, useState } from "react";
import apiClient from "../../../api/apiClient";
import { Box, CircularProgress } from "@mui/material";
import { Link } from "react-router-dom";

type CurrentAccountAnualSummaryProps = {
  account: CurrentAccount;
  year: number;
};

const CurrentAccountAnualSummary = ({ account, year }: CurrentAccountAnualSummaryProps) => {
  const [yearlySummary, setApiResponse] = useState<GetCurrentAccountYearlySummaryResponse | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const response = await apiClient.currentAccountEndpointsClient.getCurrentAccountYearlySummary(account.id, year);
        setApiResponse(response);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchData().catch((err) => console.error(err));
  }, [account.id, year]);

  return (
    <>
      <div>
        <Link
          to={`/accounts/${account.id}/${year}`}
          style={{
            textDecoration: "underline",
            color: "#1976d2",
            fontWeight: 500,
            cursor: "pointer",
          }}
        >
          {account.name}
        </Link>
      </div>
      {loading ? (
        <Box sx={{ display: "flex" }}>
          <CircularProgress />
        </Box>
      ) : (
        <TableContainer component={Paper}>
          <Table sx={{ minWidth: 650 }} size="small" aria-label="a dense table">
            <TableHead>
              <TableRow>
                <TableCell>Month</TableCell>
                <TableCell align="right">Incomes</TableCell>
                <TableCell align="right">Expenses</TableCell>
                <TableCell align="right">Investments</TableCell>
                <TableCell align="right">Result</TableCell>
                <TableCell align="right">Month end balance</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {yearlySummary?.months?.map((month, idx) => (
                <TableRow key={month?.month ?? idx} sx={{ "&:last-child td, &:last-child th": { border: 0 } }}>
                  <TableCell component="th" scope="row">
                    {typeof month?.month === "number" ? (
                      <Link
                        to={`/accounts/${account.id}/month-details/${year}/${month.month}`}
                        style={{
                          textDecoration: "underline",
                          color: "#1976d2",
                          fontWeight: 500,
                          cursor: "pointer",
                        }}
                      >
                        {new Date(0, month.month - 1).toLocaleString("default", { month: "long" })}
                      </Link>
                    ) : (
                      "-"
                    )}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.incomes === "number" ? month.incomes.toFixed(2) : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.expenses === "number" ? month.expenses.toFixed(2) : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.investments === "number" ? month.investments.toFixed(2) : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.result === "number" ? month.result.toFixed(2) : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.monthEndBalance === "number" ? month.monthEndBalance.toFixed(2) : "-"}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </>
  );
};

export default CurrentAccountAnualSummary;
