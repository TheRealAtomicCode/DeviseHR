import { Outlet } from 'react-router-dom';
import SideNav from '../components/Nav/SideNav';
import TopNav from '../components/Nav/TopNav';

function AuthenticatedLayout() {
	return (
		<div className="sm:ml-16">
			<SideNav /> {/* This is the navigation bar */}
			<TopNav />
			<main>
				<Outlet /> {/* Nested route content will be rendered here */}
			</main>
		</div>
	);
}

export default AuthenticatedLayout;
