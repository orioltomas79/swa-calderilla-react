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
  Checkbox,
  Select,
  MenuItem,
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";
import { useEffect, useState } from "react";
import apiClient from "../../api/apiClient";
import type { Operation, OperationType } from "../../api/types";

const AccountMonthPage = () => {
  const navigate = useNavigate();
  const [operations, setOperations] = useState<Operation[] | null>(null);
  const [operationTypes, setOperationTypes] = useState<OperationType[]>([]);
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
        const [operationsResponse, operationTypesResponse] = await Promise.all([
          apiClient.operationsEndpointsClient.getOperations(accountId, Number(year), Number(month)),
          apiClient.operationsEndpointsClient.getOperationTypes(),
        ]);
        setOperations(operationsResponse);
        setOperationTypes(operationTypesResponse ?? []);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };
    fetchData().catch((err) => console.error(err));
  }, [accountId, year, month]);

  const handleImportClick = () => {
    void navigate(`/accounts/${accountId}/import/${year}/${month}`);
  };

  const handleGroupClick = () => {
    void navigate(`/accounts/${accountId}/month-group/${year}/${month}`);
  };

  // Toggles the 'ignore' property for the specified operation
  const handleIgnoreChange = (operationId: string) => {
    setOperations((operations) => {
      if (!operations) return operations;
      const updated = operations.map((operation) =>
        operation.id === operationId ? { ...operation, ignore: !operation.ignore } : operation
      );

      // Find the toggled operation
      const toggled = operations.find((op) => op.id === operationId);
      if (toggled && accountId && year && month) {
        console.log(toggled);
        // Call API to patch the ignore field
        apiClient.operationsEndpointsClient
          .patchOperation(accountId, Number(year), Number(month), operationId, { ignore: !toggled.ignore })
          .catch((err) => {
            // Optionally handle error (e.g., revert UI change)
            console.error("Failed to patch ignore field", err);
          });
      }
      return updated;
    });
  };

  // Handler for changing the type
  const handleTypeChange = (operationId: string, newType: string | null) => {
    setOperations((operations) => {
      if (!operations) return operations;
      const updated = operations.map((operation) =>
        operation.id === operationId ? { ...operation, type: newType } : operation
      );

      // Find the changed operation
      const changed = operations.find((op) => op.id === operationId);
      if (changed && accountId && year && month) {
        apiClient.operationsEndpointsClient
          .patchOperation(accountId, Number(year), Number(month), operationId, { type: newType })
          .catch((err) => {
            // Optionally handle error (e.g., revert UI change)
            console.error("Failed to patch type field", err);
          });
      }
      return updated;
    });
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
          <Button variant="contained" onClick={handleImportClick}>
            Import
          </Button>
          <Button variant="contained" onClick={handleGroupClick}>
            Group
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
                  <TableCell>Ignore</TableCell>
                  <TableCell>Type</TableCell>
                  <TableCell>Description</TableCell>
                  <TableCell>Amount</TableCell>
                  <TableCell>Payer</TableCell>
                  <TableCell>Balance</TableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {operations && operations.length > 0 ? (
                  operations.map((op) => (
                    <TableRow key={op.id}>
                      <TableCell>{new Date(op.operationDate).getDate()}</TableCell>
                      <TableCell>
                        <Checkbox checked={op.ignore} onChange={() => handleIgnoreChange(op.id)} />
                      </TableCell>
                      <TableCell>
                        <Select
                          value={op.type ?? ""}
                          displayEmpty
                          onChange={(e) => {
                            const value = e.target.value === "" ? null : e.target.value;
                            handleTypeChange(op.id, value);
                          }}
                          size="small"
                          sx={{ minWidth: 120 }}
                        >
                          <MenuItem value="">
                            <em style={{ color: "red" }}>None</em>
                          </MenuItem>
                          {operationTypes.map((type) => (
                            <MenuItem key={type.id} value={type.name}>
                              {type.name}
                            </MenuItem>
                          ))}
                        </Select>
                      </TableCell>
                      <TableCell>{op.description}</TableCell>
                      <TableCell align="right" sx={{ color: op.amount >= 0 ? "green" : "red" }}>
                        {op.amount.toFixed(2)}
                      </TableCell>
                      <TableCell>{op.payer}</TableCell>
                      <TableCell align="right">{op.balance}</TableCell>
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
