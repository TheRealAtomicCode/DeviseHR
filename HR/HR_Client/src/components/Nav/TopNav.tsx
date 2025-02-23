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
import { FaSun, FaMoon } from 'react-icons/fa';

interface TopNavProps {
	toggleDarkMode: () => void;
	isDarkMode: boolean;
}

const TopNav = ({ toggleDarkMode, isDarkMode }: TopNavProps) => {
	return (
		<div
			className={`fixed w-full md:w-[calc(100%-3rem)] transition-all duration-300 ${
				window.scrollY > 0 ? 'shadow' : ''
			} bg-white/30 dark:bg-gray-900/30 backdrop-blur-3xl`}
		>
			<div className="flex items-center justify-between px-4 dark">
				{/* Logo */}
				<h1 className="text-2xl font-bold text-gray-900 dark:text-white">D</h1>

				{/* Search Bar */}
				<div className="relative flex items-center p-1 rounded-lg w-1/3 my-1">
					<HiSearch className="absolute left-2 text-gray-400 dark:text-gray-300" size={20} />
					<input
						type="text"
						placeholder="Search..."
						className="h-10 bg-transparent placeholder-gray-400 dark:placeholder-gray-500 pl-8 pr-4 rounded-3xl w-full focus:outline-none text-gray-700 dark:text-white"
					/>
				</div>

				<div className="flex items-center space-x-4">
					{/* Dark Mode Toggle */}
					<button onClick={toggleDarkMode} className="p-2 rounded-full focus:outline-none">
						{isDarkMode ? <FaMoon className="text-white" /> : <FaSun className="text-gray-900" />}
					</button>

					{/* Navigation Menu */}
					<NavigationMenu>
						<NavigationMenuList className="flex items-center space-x-4">
							{/* Notifications Button */}
							<NavigationMenuItem>
								<div className="relative">
									<NavigationMenuTrigger className="flex items-center space-x-2 px-3 py-2 rounded-lg">
										<IoIosNotifications size={20} className="text-gray-900 dark:text-gray-300" />
									</NavigationMenuTrigger>
									<NavigationMenuContent className="rounded-lg shadow-lg bg-white dark:bg-gray-800 text-gray-900 dark:text-white">
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
										<span className="text-gray-900 dark:text-white">Features</span>
									</NavigationMenuTrigger>
									<NavigationMenuContent className="rounded-lg shadow-lg bg-white dark:bg-gray-800 text-gray-900 dark:text-white">
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
};

export default TopNav;
