import { Box, Button, Toolbar } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import TopMenu from "../TopMenu/TopMenu";

const AccountMonthPage = () => {
  const navigate = useNavigate();

  const handleClick = () => {
    void navigate("/import");
  };

  return (
    <>
      <TopMenu />
      <Toolbar />
      <Box sx={{ width: "95%", mx: "auto" }}>
        <h1>Monthly details</h1>
        <div>
          <Button variant="contained" onClick={handleClick}>
            Import
          </Button>
        </div>
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

export default AccountMonthPage;
