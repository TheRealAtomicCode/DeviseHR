import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import { QueryClientProvider, QueryClient } from '@tanstack/react-query';
import { ReactQueryDevtools } from '@tanstack/react-query-devtools';
import './index.css';
import App from './App.tsx';
import { UserContext, defaultUserContext } from './context/AppContext.ts';
import { BrowserRouter as Router } from 'react-router-dom'; // Ensure the Router is imported

const queryClient = new QueryClient();

createRoot(document.getElementById('root')!).render(
	<StrictMode>
		<UserContext.Provider value={defaultUserContext}>
			<QueryClientProvider client={queryClient}>
				{/* Wrap App with Router */}
				<Router>
					<App />
				</Router>
				<ReactQueryDevtools initialIsOpen={false} />
			</QueryClientProvider>
		</UserContext.Provider>
	</StrictMode>
);
