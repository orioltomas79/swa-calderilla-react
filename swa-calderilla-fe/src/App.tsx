import { useState } from 'react'
import reactLogo from './assets/react.svg'
import viteLogo from '/vite.svg'
import './App.css'
import { OtherEndpointsClient } from './api/apiClient.g.nswag'

function App() {
  const [count, setCount] = useState(0)
  const [apiResponse, setApiResponse] = useState<string | null>(null) // State to store API response
  const [loading, setLoading] = useState(false) // State to show loading status
  const [error, setError] = useState<string | null>(null) // State to handle errors

  const fetchApiData = async () => {
    setLoading(true);
    setError(null); // Clear previous errors
    try {
      // Instantiate the OtherEndpointsClient
      const client = new OtherEndpointsClient();
  
      // Call the getMessage method
      const message = await client.getMessage();
  
      // Update the state with the result
      setApiResponse(message);
    } catch (err: any) {
      setError(err.message || 'An unexpected error occurred');
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <div>
        <a href="/.auth/login/github">Login</a>
      </div>
      <div>
        <a href="/.auth/logout">Log out</a>
      </div>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} className="logo" alt="Vite logo" />
        </a>
        <a href="https://react.dev" target="_blank">
          <img src={reactLogo} className="logo react" alt="React logo" />
        </a>
      </div>
      <h1>Vite + React</h1>
      <div className="card">
        <button onClick={() => setCount((count) => count + 1)}>
          count is {count}
        </button>
        <p>
          Edit <code>src/App.tsx</code> and save to test HMR
        </p>
        <button onClick={fetchApiData} disabled={loading}>
          {loading ? 'Loading...' : 'Fetch API Data'}
        </button>
        {error && <p style={{ color: 'red' }}>Error: {error}</p>}
        {apiResponse && (
          <pre style={{ textAlign: 'left', marginTop: '1rem' }}>{apiResponse}</pre>
        )}
      </div>
      <p className="read-the-docs">
        Click on the Vite and React logos to learn more
      </p>
    </>
  )
}

export default App
