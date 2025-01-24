import { Route, Routes } from 'react-router-dom';
import HomePage from '../pages/HomePage'; // Home page component
import LoginPage from '../pages/LoginPage'; // Login page component
import NotFoundPage from '../pages/NotFoundPage'; // Not Found page component
import AuthenticatedLayout from '../Layouts/AuthenticatedLayout';
import UsersPage from '../pages/UsersPage';
import UserPage from '../pages/UserPage';
import CalendarPage from '../pages/CalendarPage';
import RotasPage from '../pages/RotasPage';
import RotaPage from '../pages/RotaPage';
import FilesPage from '../pages/FilesPage';
import SettingsPage from '../pages/SettingsPage';

const AppRoutes = () => {
	return (
		<Routes>
			{/* Routes with Layout (Nav shown) */}
			<Route element={<AuthenticatedLayout />}>
				<Route path="/" element={<HomePage />} /> {/* Home route */}
				<Route path="/home" element={<HomePage />} />
				<Route path="/users" element={<UsersPage />} />
				<Route path="/users/:userId" element={<UserPage />} />
				<Route path="/calendar" element={<CalendarPage />} />
				<Route path="/rotas" element={<RotasPage />} />
				<Route path="/rotas/:rotaId" element={<RotaPage />} />
				<Route path="/files" element={<FilesPage />} />
				<Route path="/settings" element={<SettingsPage />} />
			</Route>

			{/* Routes without Layout (No Nav) */}
			<Route path="/login" element={<LoginPage />} />
			<Route path="*" element={<NotFoundPage />} />
		</Routes>
	);
};

export default AppRoutes;
