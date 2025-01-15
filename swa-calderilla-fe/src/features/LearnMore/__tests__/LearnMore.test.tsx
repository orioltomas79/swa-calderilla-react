import { render, screen } from "@testing-library/react";
import "@testing-library/jest-dom";
import LearnMore from "..";


test("renders the LearnMore component correctly", () => {
  render(<LearnMore />);
  expect(screen.getByTestId("learn-more")).toBeInTheDocument();
});
