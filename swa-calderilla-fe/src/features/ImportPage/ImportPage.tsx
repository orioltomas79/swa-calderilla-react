import { Box, Toolbar } from "@mui/material";
import { useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";
import ImportIngButton from "./ImportIngButton";
import apiClient from "../../api/apiClient";
import { useState } from "react";
import ImportResponseRawData from "./ImportResponseRawData";
import { useCurrentAccounts } from "../../contexts/useCurrentAccounts";
import ImportSabadellButton from "./ImportSabadellButton";
import ImportResponseOperationsData from "./ImportResponseOperationsData";
import type { UploadIngExtractResponse, UploadSabadellExtractResponse } from "../../api/types";

const ImportPage = () => {
  const { accountId, year, month } = useParams<{
    accountId: string;
    year: string;
    month: string;
  }>();

  const { listCurrentAccounts, loadingAccounts, error } = useCurrentAccounts();

  const [loading, setLoading] = useState(false);
  const [ingResponse, setIngResponse] = useState<UploadIngExtractResponse | null>(null);
  const [sabadellResponse, setSabadellResponse] = useState<UploadSabadellExtractResponse | null>(null);

  const handleIngFileUpload = async (file: File) => {
    try {
      setLoading(true);
      setIngResponse(null);
      if (!accountId || !year || !month) {
        return;
      }
      const response = await apiClient.ingEndpointsClient.uploadIngExtract(accountId, Number(year), Number(month), {
        data: file,
        fileName: file.name,
      });
      setIngResponse(response);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleSabadellFileUpload = async (file: File) => {
    try {
      setLoading(true);
      setSabadellResponse(null);
      if (!accountId || !year || !month) {
        return;
      }
      const response = await apiClient.sabadellEndpointsClient.uploadSabadellExtract(
        accountId,
        Number(year),
        Number(month),
        {
          data: file,
          fileName: file.name,
        }
      );
      setSabadellResponse(response);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  let account;
  if (loadingAccounts || error) {
    account = undefined;
  } else {
    account = listCurrentAccounts?.find((acc) => acc.id === accountId);
  }

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <h1>Import bank extract for account: {account ? account.name : ""}</h1>
        <div>
          {year && month ? (
            <Box sx={{ fontWeight: "bold", fontSize: 20, my: 2 }}>
              {new Date(Number(year), Number(month) - 1).toLocaleString(undefined, { month: "long", year: "numeric" })}
            </Box>
          ) : null}
        </div>
        <div>
          {account?.type === "Ing" && (
            <ImportIngButton
              loading={loading}
              handleFileUpload={(file) => {
                void handleIngFileUpload(file);
              }}
            />
          )}
          {account?.type === "Sabadell" && (
            <ImportSabadellButton
              loading={loading}
              handleFileUpload={(file) => {
                void handleSabadellFileUpload(file);
              }}
            />
          )}
        </div>
        {account?.type === "Ing" && ingResponse && (
          <Box sx={{ mt: 3 }}>
            <h2>ING Extract Upload Result</h2>
            {ingResponse.ingExtractRaw && <ImportResponseRawData data={ingResponse.ingExtractRaw} />}
            {ingResponse.operations && <ImportResponseOperationsData operations={ingResponse.operations} />}
          </Box>
        )}
        {account?.type === "Sabadell" && sabadellResponse && (
          <Box sx={{ mt: 3 }}>
            <h2>ING Extract Upload Result</h2>
            {sabadellResponse.sabadellExtractPipe && (
              <ImportResponseRawData data={sabadellResponse.sabadellExtractPipe} />
            )}
            {sabadellResponse.operations && <ImportResponseOperationsData operations={sabadellResponse.operations} />}
          </Box>
        )}
      </Box>
    </>
  );
};

export default ImportPage;
