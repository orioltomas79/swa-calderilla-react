import { render } from "@testing-library/react";
import HelloWorld from "../HelloWorld";

describe("HelloWorld", () => {
  it("renders Hello World", () => {
    render(<HelloWorld />);
  });
});
