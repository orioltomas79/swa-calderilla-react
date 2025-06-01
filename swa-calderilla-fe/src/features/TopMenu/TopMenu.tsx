import { AppBar, Toolbar, Typography, Button } from "@mui/material";

const TopMenu = () => {
  const year = new Date().getFullYear();

  return (
    <AppBar position="fixed">
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          Calderilla
        </Typography>
        <Button
          color="inherit"
          onClick={() => {
            window.location.href = `/home/${year}`;
          }}
        >
          Home
        </Button>
        <Button
          color="inherit"
          onClick={() => {
            window.location.href = "/.auth/logout?post_logout_redirect_uri=/";
          }}
        >
          Logout
        </Button>
      </Toolbar>
    </AppBar>
  );
};

export default TopMenu;
