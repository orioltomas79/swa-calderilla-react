import {
  Box,
  Button,
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
import { useNavigate, useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";
import { useEffect, useState } from "react";
import apiClient from "../../api/apiClient";
import type { Operation } from "../../api/types";

const AccountMonthPage = () => {
  const navigate = useNavigate();
  const [operations, setApiResponse] = useState<Operation[] | null>(null);
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
        const response = await apiClient.operationsEndpointsClient.getOperations(
          accountId,
          Number(year),
          Number(month)
        );
        setApiResponse(response);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchData().catch((err) => console.error(err));
  }, [accountId, year, month]);

  const handleClick = () => {
    void navigate(`/accounts/${accountId}/import/${year}/${month}`);
  };

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
        <div>
          <Button variant="contained" onClick={handleClick}>
            Import
          </Button>
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
                  <TableCell>Day</TableCell>
                  <TableCell>Description</TableCell>
                  <TableCell>Payer</TableCell>
                  <TableCell>Amount</TableCell>
                  <TableCell>Balance</TableCell>
                  <TableCell>Ignore</TableCell>
                  <TableCell>Type</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {operations && operations.length > 0 ? (
                  operations.map((op) => (
                    <TableRow key={op.id}>
                      <TableCell>{new Date(op.operationDate).getDate()}</TableCell>
                      <TableCell>{op.description}</TableCell>
                      <TableCell>{op.payer}</TableCell>
                      <TableCell align="right" sx={{ color: op.amount >= 0 ? "green" : "red" }}>
                        {op.amount.toFixed(2)}
                      </TableCell>
                      <TableCell align="right">{op.balance}</TableCell>
                      <TableCell>{op.ignore}</TableCell>
                      <TableCell>{op.type}</TableCell>
                    </TableRow>
                  ))
                ) : (
                  <TableRow>
                    <TableCell colSpan={3} align="center">
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

export default AccountMonthPage;
