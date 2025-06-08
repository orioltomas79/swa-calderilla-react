import { Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from "@mui/material";
import React from "react";
import type { Operation } from "../../api/types";

interface ImportResponseOperationsDataProps {
  operations: Operation[];
}

const ImportResponseOperationsData: React.FC<ImportResponseOperationsDataProps> = ({ operations }) => {
  if (!operations || operations.length === 0) return null;
  return (
    <div>
      <strong>Operations:</strong>
      <TableContainer component={Paper} sx={{ mt: 3 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>Day</TableCell>
              <TableCell>Description</TableCell>
              <TableCell>Payer</TableCell>
              <TableCell>Amount</TableCell>
              <TableCell>Balance</TableCell>
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
                  <TableCell align="right">{op.balance.toFixed(2)}</TableCell>
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
    </div>
  );
};

export default ImportResponseOperationsData;
