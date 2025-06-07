import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableCell from "@mui/material/TableCell";
import TableContainer from "@mui/material/TableContainer";
import TableHead from "@mui/material/TableHead";
import TableRow from "@mui/material/TableRow";
import Paper from "@mui/material/Paper";
import type { CurrentAccount } from "../../../api/types";
import { useEffect, useState } from "react";
import apiClient from "../../../api/apiClient";
import type { GetCurrentAccountYearlySummaryResponse } from "../../../api/apiClient.g.nswag";
import { Box, CircularProgress } from "@mui/material";

type CurrentAccountAnualSummaryProps = {
  account: CurrentAccount;
  year: number;
};

const CurrentAccountAnualSummary = ({
  account,
  year,
}: CurrentAccountAnualSummaryProps) => {
  const [yearlySummary, setApiResponse] =
    useState<GetCurrentAccountYearlySummaryResponse | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchApiData = async () => {
    setLoading(true);
    try {
      const response =
        await apiClient.currentAccountEndpointsClient.getCurrentAccountYearlySummary(
          account.id,
          year
        );
      setApiResponse(response);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchApiData().catch((err) => console.error(err));
  }, [year, account.id]);

  return (
    <>
      <div>
        <p>{account.name}</p>
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
                <TableCell align="right">Result</TableCell>
                <TableCell align="right">Month end balance</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {yearlySummary?.months?.map((month, idx) => (
                <TableRow
                  key={month?.month ?? idx}
                  sx={{ "&:last-child td, &:last-child th": { border: 0 } }}
                >
                  <TableCell component="th" scope="row">
                    {month?.month ?? "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.incomes === "number"
                      ? month.incomes.toFixed(2)
                      : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.expenses === "number"
                      ? month.expenses.toFixed(2)
                      : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.result === "number"
                      ? month.result.toFixed(2)
                      : "-"}
                  </TableCell>
                  <TableCell align="right">
                    {typeof month?.monthEndBalance === "number"
                      ? month.monthEndBalance.toFixed(2)
                      : "-"}
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
