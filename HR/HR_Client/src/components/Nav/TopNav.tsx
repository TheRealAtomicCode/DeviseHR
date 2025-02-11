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

function TopNav() {

	return (
		<div className="bg-gray-800 text-white shadow-md">
			<div className="flex items-center justify-between px-4 py-1">
				{/* Logo */}
				<h1 className="text-2xl font-bold text-white">DeviseHR</h1>

				{/* Search Bar */}
				<div className="relative flex items-center bg-gray-700 p-2 rounded-lg w-1/3 my-1">
					<HiSearch className="text-gray-400 absolute left-2" size={20} />
					<input
						type="text"
						placeholder="Search..."
						className="bg-transparent text-white placeholder-gray-400 pl-8 pr-4  rounded-lg w-full focus:outline-none"
					/>
				</div>

				{/* Navigation Menu */}
				<NavigationMenu>
					<NavigationMenuList className="flex items-center space-x-4">
						{/* Notifications Button */}
						<NavigationMenuItem>
							<div className="relative">
								<NavigationMenuTrigger className="flex items-center space-x-2 px-3 py-2 bg-gray-700 rounded-lg hover:bg-gray-600">
									<IoIosNotifications size={20} />
								</NavigationMenuTrigger>
								<NavigationMenuContent className="bg-white text-black rounded-lg shadow-lg">
									<div>
										<NavigationMenuLink href="#">
											Notification
										</NavigationMenuLink>
									</div>
									<div>
										<NavigationMenuLink href="#">
											Notification
										</NavigationMenuLink>
									</div>
									<div>
										<NavigationMenuLink href="#">
											Notification
										</NavigationMenuLink>
									</div>
									<div>
										<NavigationMenuLink href="#">
											Notification
										</NavigationMenuLink>
									</div>
									<div>
										<NavigationMenuLink href="#">
											Notification
										</NavigationMenuLink>
									</div>
								</NavigationMenuContent>
							</div>
						</NavigationMenuItem>

						<NavigationMenuItem>
							<div className="relative">
								<NavigationMenuTrigger className="flex items-center space-x-2 px-3 py-2 bg-gray-700 rounded-lg hover:bg-gray-600">
									<span>Features</span>
								</NavigationMenuTrigger>
								<NavigationMenuContent className="bg-white text-black rounded-lg shadow-lg">
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
	);
}

export default TopNav;
