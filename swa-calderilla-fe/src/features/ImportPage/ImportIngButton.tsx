import { Button, styled } from "@mui/material";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import React from "react";

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

interface ImportIngButtonProps {
  loading: boolean;
  handleFileUpload: (file: File) => void;
}

const ImportIngButton: React.FC<ImportIngButtonProps> = ({ loading, handleFileUpload }) => (
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
        }
      }}
    />
  </Button>
);

export default ImportIngButton;
