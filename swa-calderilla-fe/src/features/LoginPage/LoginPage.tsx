import { Card, Box, Typography, Button } from "@mui/material";

/**
 * Login component renders a centered login card for the Calderilla application.
 *
 * The component uses Material-UI's Box and Card components to create a visually appealing login screen.
 * It displays a title, a welcome message, instructions, and a button to log in with GitHub.
 *
 * The `sx` prop is used for styling with the following attributes:
 * - `height: "100vh"`: Makes the container take the full viewport height.
 * - `display: "flex"`: Uses flexbox layout for centering.
 * - `justifyContent: "center"`: Horizontally centers the content.
 * - `alignItems: "center"`: Vertically centers the content.
 * - `backgroundColor: "grey.100"`: Sets a light grey background color.
 *
 * The Card's `sx` prop:
 * - `p: 4`: Adds padding inside the card.
 * - `maxWidth: 400`: Limits the card's maximum width.
 * - `width: "100%"`: Makes the card responsive to its container.
 *
 * The inner Box's `sx` prop:
 * - `display: "flex"`: Uses flexbox for layout.
 * - `flexDirection: "column"`: Arranges children vertically.
 * - `gap: 2`: Adds spacing between child elements.
 *
 */
const Login = () => {
  return (
    <Box
      sx={{
        height: "100vh",
        display: "flex",
        justifyContent: "center",
        alignItems: "center",
        backgroundColor: "grey.100",
      }}
    >
      <Card variant="outlined" sx={{ p: 4, maxWidth: 400, width: "100%" }}>
        <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
          <Typography variant="h4">Login</Typography>
          <Typography variant="h5">Welcome to Calderilla.</Typography>
          <Typography variant="body1">
            To access the application, please click the "Log in with Github" button. A GitHub account is required.
          </Typography>
          <Button
            fullWidth
            variant="outlined"
            onClick={() => {
              const year = new Date().getFullYear();
              window.location.href = `/.auth/login/github?post_login_redirect_uri=/home/${year}`;
            }}
          >
            Log in with Github
          </Button>
        </Box>
      </Card>
    </Box>
  );
};

export default Login;
