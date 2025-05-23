/* eslint-disable @typescript-eslint/no-misused-promises */
import { useState } from "react";
import apiClient from "../../api/apiClient";
import { Operation } from "../../api/types";
import {
  Button,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";

interface MonthOperationsTableProps {
  currentAccount: string;
  year: number;
  month: number;
}

const VisuallyHiddenInput = styled("input")({
  clip: "rect(0 0 0 0)",
  clipPath: "inset(50%)",
  height: 1,
  overflow: "hidden",
  position: "absolute",
  bottom: 0,
  left: 0,
  whiteSpace: "nowrap",
  width: 1,
});

const MonthOperationsTable = ({
  currentAccount,
  year,
  month,
}: MonthOperationsTableProps): JSX.Element => {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [apiResponse, setApiResponse] = useState<Operation[] | null>(null);

  const fetchApiData = async () => {
    setLoading(true);
    setError(null); // Clear previous errors
    try {
      const listOperations =
        await apiClient.operationsEndpointsClient.getOperations(
          currentAccount,
          year,
          month
        );
      setApiResponse(listOperations);
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message || "An unexpected error occurred");
      } else {
        setError("An unexpected error occurred");
      }
    } finally {
      setLoading(false);
    }
  };

  const handleFileUpload = async (file: File) => {
    setLoading(true);
    setError(null);
    try {
      await apiClient.ingEndpointsClient.uploadIngExtract(
        currentAccount,
        year,
        month,
        { data: file, fileName: file.name }
      );
    } catch (err) {
      if (err instanceof Error) {
        setError(err.message || "File upload failed");
      } else {
        setError("File upload failed");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <div>
        <Button
          component="label"
          role={undefined}
          variant="contained"
          tabIndex={-1}
          startIcon={<CloudUploadIcon />}
        >
          Upload Ing file
          <VisuallyHiddenInput
            type="file"
            onChange={(event) => {
              if (event.target.files && event.target.files.length > 0) {
                const file = event.target.files[0];
                void handleFileUpload(file);
                console.log("Selected file:", file);
              }
            }}
          />
        </Button>
      </div>
      <button onClick={fetchApiData} disabled={loading}>
        {loading ? "Loading..." : "Fetch API Data"}
      </button>
      {error && <p style={{ color: "red" }}>Error: {error}</p>}
      {apiResponse && (
        <TableContainer
          component={Paper}
          style={{
            marginTop: "1rem",
            boxShadow: "0px 3px 6px rgba(0,0,0,0.1)",
          }}
        >
          <Table>
            <TableHead>
              <TableRow style={{ backgroundColor: "#f5f5f5" }}>
                <TableCell style={{ fontWeight: "bold" }}>
                  Operation Date
                </TableCell>
                <TableCell style={{ fontWeight: "bold" }}>
                  Description
                </TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Amount</TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Balance</TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Payer</TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Ignore</TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Type</TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Notes</TableCell>
                <TableCell style={{ fontWeight: "bold" }}>Reviewed</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {apiResponse
                .sort((a, b) => a.monthOperationNumber - b.monthOperationNumber)
                .map((operation) => (
                  <TableRow
                    key={operation.id}
                    style={{
                      backgroundColor: operation.reviewed
                        ? "#e0f7fa"
                        : "#ffffff",
                    }}
                  >
                    <TableCell>{operation.operationDate}</TableCell>
                    <TableCell>{operation.description}</TableCell>
                    <TableCell>{operation.amount}</TableCell>
                    <TableCell>{operation.balance}</TableCell>
                    <TableCell>{operation.payer}</TableCell>
                    <TableCell>{operation.ignore ? "Yes" : "No"}</TableCell>
                    <TableCell>{operation.type}</TableCell>
                    <TableCell>{operation.notes}</TableCell>
                    <TableCell>{operation.reviewed ? "Yes" : "No"}</TableCell>
                  </TableRow>
                ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}
    </div>
  );
};

export default MonthOperationsTable;
