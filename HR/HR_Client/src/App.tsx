import { BrowserRouter as Router } from "react-router-dom";
import AppRoutes from "./Router/AppRouter"; // Import the routes component
import { useMutation } from '@tanstack/react-query'
import { IServiceResponse } from "./Interfaces/IServiceResponse";
import { IRefreshData, refresh } from "./APIs/Auth/refresh";

const App = () => {
  const loginMutation = useMutation<IServiceResponse<IRefreshData>, Error, { email: string, password: string }>({
    mutationFn: (token) => refresh(""),
    onSuccess: (data: IServiceResponse<IRefreshData>) => {
      if (data.success) {

        

      } else {

      }
    },
    onError: (error: Error) => {

    },
  });

  return (
    <Router> {/* This is the Router that wraps your routes */}
      <AppRoutes /> {/* Your routing logic */}
    </Router>
  );
};

export default App;
