import { Route, Routes } from 'react-router-dom';
import HomePage from '../pages/HomePage';
import LoginPage from '../pages/LoginPage';
import NotFoundPage from '../pages/NotFoundPage';
import AuthenticatedLayout from '../layouts/AuthenticatedLayout';
import UsersPage from '../pages/UsersPage';
import UserPage from '../pages/UserPage';
import CalendarPage from '../pages/CalendarPage';
import RotasPage from '../pages/RotasPage';
import RotaPage from '../pages/RotaPage';
import FilesPage from '../pages/FilesPage';
import SettingsPage from '../pages/SettingsPage';

// Define the props
interface AppRoutesProps {
	toggleDarkMode: () => void;
	isDarkMode: boolean;
}

const AppRoutes = ({ toggleDarkMode, isDarkMode }: AppRoutesProps) => {
	return (
		<Routes>
			{/* Routes with Layout (Nav shown) */}
			<Route element={<AuthenticatedLayout toggleDarkMode={toggleDarkMode} isDarkMode={isDarkMode} />}>
				<Route path="/" element={<HomePage />} />
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
