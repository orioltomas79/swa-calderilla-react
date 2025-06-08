import React from "react";

interface ImportResponseRawDataProps {
  data: string[];
}

const ImportResponseRawData: React.FC<ImportResponseRawDataProps> = ({ data }) => {
  if (!data || data.length === 0) return null;
  return (
    <div>
      <strong>Raw Data:</strong>
      <pre style={{ maxHeight: 200, overflow: "auto", background: "#f5f5f5", padding: 8 }}>{data.join("\n")}</pre>
    </div>
  );
};

export default ImportResponseRawData;
