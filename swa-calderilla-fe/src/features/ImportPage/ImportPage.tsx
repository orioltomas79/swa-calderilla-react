import { Box, Button, styled, Toolbar } from "@mui/material";
import { useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import apiClient from "../../api/apiClient";
import { useState } from "react";

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

const ImportPage = () => {
  const { accountId, year, month } = useParams<{
    accountId: string;
    year: string;
    month: string;
  }>();

  const [loading, setLoading] = useState(false);

  const handleFileUpload = async (file: File) => {
    try {
      setLoading(true);
      if (!accountId || !year || !month) {
        return;
      }
      await apiClient.ingEndpointsClient.uploadIngExtract(accountId, Number(year), Number(month), {
        data: file,
        fileName: file.name,
      });
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <h1>Import</h1>
        <div>
          {year && month ? (
            <Box sx={{ fontWeight: "bold", fontSize: 20, my: 2 }}>
              {new Date(Number(year), Number(month) - 1).toLocaleString(undefined, { month: "long", year: "numeric" })}
            </Box>
          ) : null}
        </div>
        <div>
          <Button
            component="label"
            role={undefined}
            variant="contained"
            tabIndex={-1}
            startIcon={<CloudUploadIcon />}
            disabled={loading}
          >
            Upload Ing file
            <VisuallyHiddenInput
              type="file"
              onChange={(event) => {
                if (event.target.files && event.target.files.length > 0) {
                  const file = event.target.files[0];
                  handleFileUpload(file);
                  console.log("Selected file:", file);
                }
              }}
            />
          </Button>
        </div>
      </Box>
    </>
  );
};

export default ImportPage;
