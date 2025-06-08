import { render } from "@testing-library/react";
import HomePage from "../HomePage";
import { BrowserRouter } from "react-router-dom";
import { CurrentAccountsProvider } from "../../../contexts/CurrentAccountsContext";

describe("HomePage", () => {
  it("renders HomePage", () => {
    render(
      <CurrentAccountsProvider>
        <BrowserRouter>
          <HomePage />
        </BrowserRouter>
      </CurrentAccountsProvider>
    );
  });
});
