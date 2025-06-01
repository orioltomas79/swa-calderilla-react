import { Box, Toolbar } from "@mui/material";
import { useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";

const ImportPage = () => {
  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <h1>Import</h1>
        <div>
          Account ID: {useParams<{ accountId: string }>().accountId}
          <br />
          Year: {useParams<{ year: string }>().year}
          <br />
          Month: {useParams<{ month: string }>().month}
        </div>
      </Box>
    </>
  );
};

export default ImportPage;
