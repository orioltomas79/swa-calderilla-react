import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import "./index.css";
import "@fontsource/roboto/300.css";
import "@fontsource/roboto/400.css";
import "@fontsource/roboto/500.css";
import "@fontsource/roboto/700.css";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import LoginPage from "./features/LoginPage/LoginPage.tsx";
import { CssBaseline } from "@mui/material";
import HomePage from "./features/HomePage/HomePage.tsx";
import ImportPage from "./features/ImportPage/ImportPage.tsx";

createRoot(document.getElementById("root")!).render(
  <StrictMode>
    <CssBaseline>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<LoginPage />} />
          <Route path="home" element={<HomePage />} />
          <Route path="import" element={<ImportPage />} />
        </Routes>
      </BrowserRouter>
    </CssBaseline>
  </StrictMode>
);
