import { useContext, useEffect, useState } from 'react';
import { useNavigate, useLocation } from 'react-router-dom';
import AppRoutes from './router/appRouter';
import { useMutation } from '@tanstack/react-query';
import { TServiceResponse } from './types/TServiceResponse';
import { refresh, IRefreshData, TRefreshRequest } from './APIs/auth/refresh';
import { getCookie, setCookie } from './utils/cookies';
import { UserContext } from './context/AppContext';

const App = () => {
	const navigate = useNavigate();
	const { pathname } = useLocation();
	const [loading, setLoading] = useState(true);

	// Read dark mode state from localStorage
	const [isDarkMode, setIsDarkMode] = useState(() => {
		if (typeof window !== 'undefined') {
			return localStorage.getItem('theme') === 'dark';
		}
		return false;
	});

	// Apply the dark mode class to <html> when the state changes
	useEffect(() => {
		if (isDarkMode) {
			document.documentElement.classList.add('dark');
			localStorage.setItem('theme', 'dark');
		} else {
			document.documentElement.classList.remove('dark');
			localStorage.setItem('theme', 'light');
		}
	}, [isDarkMode]);

	// Function to toggle dark mode
	const toggleDarkMode = () => {
		setIsDarkMode((prev) => !prev);
	};

	const context = useContext(UserContext);

	const refreshMutation = useMutation<
		TServiceResponse<IRefreshData>,
		Error,
		TRefreshRequest
	>({
		mutationFn: (refreshToken) => refresh(refreshToken),
		onSuccess: (res: TServiceResponse<IRefreshData>) => {
			if (res.success) {
				setCookie('jwt', res.data.jwt, 900);
				setCookie('refreshToken', res.data.refreshToken, 900);

				context.id = res.data.id;
				context.firstName = res.data.firstName;
				context.lastName = res.data.lastName;
				context.email = res.data.email;
				context.isTerminated = res.data.isTerminated;
				context.isVerified = res.data.isVerified;
				context.profilePicture = res.data.profilePicture;
				context.userRole = res.data.userRole;

				navigate(pathname);
				setLoading(false);
			} else {
				navigate('/login');
			}
		},
		onError: (error: Error) => {
			console.log(error);
			setLoading(false);
			navigate('/login');
		},
	});

	useEffect(() => {
		const refreshToken = getCookie('refreshToken');
		const jwt = getCookie('jwt');

		const tokens = { refreshToken, jwt };

		if (!tokens.jwt || !tokens.refreshToken) {
			navigate('/login');
			setLoading(false);
			return;
		}

		refreshMutation.mutate(tokens); // Perform mutation to refresh tokens
	}, []);

	if (loading) {
		return (
			<div className="flex items-center justify-center min-h-screen">
				<div className="text-xl">Loading...</div>
			</div>
		);
	}

	return (
		<div className={`min-h-screen ${isDarkMode ? 'dark' : ''}`}>
			{/* Pass the toggle function as a prop to children */}
			<AppRoutes toggleDarkMode={toggleDarkMode} isDarkMode={isDarkMode} />
		</div>
	);
};

export default App;
