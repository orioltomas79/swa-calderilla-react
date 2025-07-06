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
import type { Operation } from "../../api/types";

const AccountMonthGroupPage = () => {
  const [groupedOperations, setGroupedOperations] = useState<{ [type: string]: number }>({});
  const [loading, setLoading] = useState(false);

  const { accountId, year, month } = useParams<{
    accountId: string;
    year: string;
    month: string;
  }>();

  useEffect(() => {
    const fetchData = async () => {
      if (!accountId || !year || !month) {
        return;
      }
      setLoading(true);
      try {
        const operationsResponse = await apiClient.operationsEndpointsClient.getOperations(
          accountId,
          Number(year),
          Number(month)
        );
        // Group by type and sum amounts, excluding ignored operations
        const grouped: { [type: string]: number } = {};
        operationsResponse
          .filter((op: Operation) => !op.ignore)
          .forEach((op: Operation) => {
            const type = op.type ?? "Sin tipo";
            if (!grouped[type]) grouped[type] = 0;
            grouped[type] += op.amount;
          });
        setGroupedOperations(grouped);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchData().catch((err) => console.error(err));
  }, [accountId, year, month]);

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <div>
          {year && month ? (
            <Box sx={{ fontWeight: "bold", fontSize: 20, my: 2 }}>
              {new Date(Number(year), Number(month) - 1).toLocaleString(undefined, { month: "long", year: "numeric" })}
            </Box>
          ) : null}
        </div>
        {loading ? (
          <Box sx={{ display: "flex", justifyContent: "center", mt: 2 }}>
            <CircularProgress />
          </Box>
        ) : (
          <TableContainer component={Paper} sx={{ mt: 3 }}>
            <Table>
              <TableHead>
                <TableRow>
                  <TableCell>Type</TableCell>
                  <TableCell>Amount</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {groupedOperations && Object.keys(groupedOperations).length > 0 ? (
                  Object.entries(groupedOperations)
                    .sort((a, b) => b[1] - a[1]) // Sort by amount descending
                    .map(([type, amount]) => (
                      <TableRow key={type}>
                        <TableCell>{type}</TableCell>
                        <TableCell align="right" sx={{ color: amount >= 0 ? "green" : "red" }}>
                          {amount.toFixed(2)}
                        </TableCell>
                      </TableRow>
                    ))
                ) : (
                  <TableRow>
                    <TableCell colSpan={2} align="center">
                      No operations found.
                    </TableCell>
                  </TableRow>
                )}
              </TableBody>
            </Table>
          </TableContainer>
        )}
      </Box>
    </>
  );
};

export default AccountMonthGroupPage;
