import { Outlet } from 'react-router-dom';
import SideNav from '../components/Nav/SideNav';
import TopNav from '../components/Nav/TopNav';

interface AuthenticatedLayoutProps {
	toggleDarkMode: () => void;
	isDarkMode: boolean;
}

function AuthenticatedLayout({ toggleDarkMode, isDarkMode }: AuthenticatedLayoutProps) {
	return (
		<div className={`sm:ml-16 ${isDarkMode ? 'dark' : ''}`}>
			<SideNav />
			<TopNav toggleDarkMode={toggleDarkMode} isDarkMode={isDarkMode} />
			<main className="pt-14 bg-slate-100 dark:bg-gray-900">
				<Outlet />
			</main>
		</div>
	);
}

export default AuthenticatedLayout;
