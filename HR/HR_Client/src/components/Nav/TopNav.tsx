import {
	NavigationMenu,
	NavigationMenuContent,
	NavigationMenuItem,
	NavigationMenuLink,
	NavigationMenuList,
	NavigationMenuTrigger,
} from '../../components/ui/navigation-menu';
import { HiSearch } from 'react-icons/hi'; // Search icon
import { IoIosNotifications } from 'react-icons/io';
import { useState, useEffect } from 'react';
import { FaSun, FaMoon } from 'react-icons/fa';

function TopNav() {
	const [isScrolled, setIsScrolled] = useState(false);
	const [isDarkMode, setIsDarkMode] = useState(() => {
		const stored = localStorage.getItem('isDarkMode');
		return stored ? JSON.parse(stored) : false;
	});

	useEffect(() => {
		const handleScroll = () => {
			setIsScrolled(window.scrollY > 0);
		};

		window.addEventListener('scroll', handleScroll);
		return () => window.removeEventListener('scroll', handleScroll);
	}, []);

	const toggleDarkMode = () => {
		setIsDarkMode((prev: any) => {
			const newValue = !prev;
			localStorage.setItem('isDarkMode', JSON.stringify(newValue));
			// Optionally, update your app context here as well.
			return newValue;
		});
	};

	return (
		<div
			className={`fixed w-full md:w-[calc(100%-3rem)] transition-all duration-300 ${
				isScrolled ? 'shadow' : ''
			} ${isDarkMode ? 'bg-gray-900/30' : 'bg-white/30'} backdrop-blur-3xl`}
		>
			<div className="flex items-center justify-between px-4">
				{/* Logo */}
				<h1 className={`text-2xl font-bold ${isDarkMode ? 'text-white' : 'text-gray-900'}`}>
					D
				</h1>

				{/* Search Bar */}
				<div className="relative flex items-center p-1 rounded-lg w-1/3 my-1">
					<HiSearch
						className={`absolute left-2 ${isDarkMode ? 'text-gray-300' : 'text-gray-400'}`}
						size={20}
					/>
					<input
						type="text"
						placeholder="Search..."
						className={`h-10 bg-transparent placeholder-gray-400 pl-8 pr-4 rounded-3xl w-full focus:outline-none ${
							isDarkMode ? 'text-white' : 'text-gray-700'
						}`}
					/>
				</div>

				<div className="flex items-center space-x-4">
					{/* Navigation Menu */}
					<button onClick={toggleDarkMode} className="p-2 rounded-full focus:outline-none">
						{isDarkMode ? <FaMoon /> : <FaSun />}
					</button>
					<NavigationMenu>
						<NavigationMenuList className="flex items-center space-x-4">
							{/* Notifications Button */}
							<NavigationMenuItem>
								<div className="relative">
									
									<NavigationMenuTrigger className="flex items-center space-x-2 px-3 py-2 rounded-lg">
										<IoIosNotifications
											size={20}
											className={isDarkMode ? 'text-gray-300' : 'text-gray-900'}
										/>
									</NavigationMenuTrigger>
									<NavigationMenuContent
										className={`rounded-lg shadow-lg ${
											isDarkMode ? 'bg-gray-800 text-white' : 'bg-white text-gray-900'
										}`}
									>
										<div>
											<NavigationMenuLink href="#">Notification</NavigationMenuLink>
										</div>
										<div>
											<NavigationMenuLink href="#">Notification</NavigationMenuLink>
										</div>
										<div>
											<NavigationMenuLink href="#">Notification</NavigationMenuLink>
										</div>
										<div>
											<NavigationMenuLink href="#">Notification</NavigationMenuLink>
										</div>
										<div>
											<NavigationMenuLink href="#">Notification</NavigationMenuLink>
										</div>
									</NavigationMenuContent>
								</div>
							</NavigationMenuItem>

							{/* Features Menu */}
							<NavigationMenuItem>
								<div className="relative">
									<NavigationMenuTrigger className="flex items-center space-x-2 px-3 py-2">
										<span className={isDarkMode ? 'text-white' : 'text-gray-900'}>Features</span>
									</NavigationMenuTrigger>
									<NavigationMenuContent
										className={`rounded-lg shadow-lg ${
											isDarkMode ? 'bg-gray-800 text-white' : 'bg-white text-gray-900'
										}`}
									>
										<div>
											<NavigationMenuLink href="#">What's new</NavigationMenuLink>
										</div>
										<div>
											<NavigationMenuLink href="#">Help</NavigationMenuLink>
										</div>
										<div>
											<NavigationMenuLink href="#">Feedback</NavigationMenuLink>
										</div>
									</NavigationMenuContent>
								</div>
							</NavigationMenuItem>
						</NavigationMenuList>
					</NavigationMenu>
				</div>
			</div>
		</div>
	);
}

export default TopNav;
