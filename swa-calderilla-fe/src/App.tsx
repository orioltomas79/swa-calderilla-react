import { useState } from "react";
import "./App.css";
import WelcomeMessage from "./features/WelcomeMessage";
import Button from "@mui/material/Button";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <div>
        <a href="/.auth/login/github">Login</a>
      </div>
      <div>
        <a href="/.auth/logout">Log out</a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
        <WelcomeMessage />
      </div>
      <div>
        <Button variant="contained">Hello Material UI</Button>
      </div>
    </>
  );
}

export default App;
