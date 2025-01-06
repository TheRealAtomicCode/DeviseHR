import { BrowserRouter as Router, Routes, Route, useLocation } from 'react-router-dom';
import SideNav from './components/Nav/SideNav';
import Home from './Views/HomeView';
import Login from './Views/LoginView';
import './App.css';

function App() {
	const location = useLocation();
	const showSideNav = location.pathname !== '/login'; // Adjust the path as necessary

	return (
		<div className="">
			{showSideNav && <SideNav />}
			<main className={showSideNav ? 'ml-16' : ''}>
				<Routes>
					<Route path="/" element={<Home />} />
					<Route path="/login" element={<Login />} />
					{/* Add other routes as needed */}
				</Routes>
			</main>
		</div>
	);
}

function AppWrapper() {
	return (
		<Router>
			<App />
		</Router>
	);
}

export default AppWrapper;