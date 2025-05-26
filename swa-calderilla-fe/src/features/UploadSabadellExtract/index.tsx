import { Button } from "@mui/material";
import { styled } from "@mui/material/styles";
import CloudUploadIcon from "@mui/icons-material/CloudUpload";
import React from "react";

interface UploadSabadellExtractProps {
  onFileUpload: (file: File) => void;
  disabled?: boolean;
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

const UploadSabadellExtract: React.FC<UploadSabadellExtractProps> = ({
  onFileUpload,
  disabled,
}) => (
  <Button
    component="label"
    role={undefined}
    variant="contained"
    tabIndex={-1}
    startIcon={<CloudUploadIcon />}
    disabled={disabled}
  >
    Upload Sabadell file
    <VisuallyHiddenInput
      type="file"
      onChange={(event) => {
        if (event.target.files && event.target.files.length > 0) {
          const file = event.target.files[0];
          onFileUpload(file);
          console.log("Selected file:", file);
        }
      }}
    />
  </Button>
);

export default UploadSabadellExtract;
