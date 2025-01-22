import { BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./Router/AppRouter"; // Import the routes component
import { useQuery } from '@tanstack/react-query'

const App = () => {
  const refresh = useQuery({
    queryKey: ['refresh']
  })

  return (
    <Router> {/* This is the Router that wraps your routes */}
      <AppRoutes /> {/* Your routing logic */}
    </Router>
  );
};

export default App;
