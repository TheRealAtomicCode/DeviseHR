import {
	NavigationMenu,
	NavigationMenuContent,
	NavigationMenuItem,
	NavigationMenuLink,
	NavigationMenuList,
	NavigationMenuTrigger,
  } from '../../components/ui/navigation-menu';
  import { HiSearch } from 'react-icons/hi';
  import { IoIosNotifications } from 'react-icons/io';
  import { FaSun, FaMoon } from 'react-icons/fa';
  
  interface TopNavProps {
	toggleDarkMode: () => void;
	isDarkMode: boolean;
  }
  
  const TopNav = ({ toggleDarkMode, isDarkMode }: TopNavProps) => {
	return (
	  <div
		className={`fixed w-full md:w-[calc(100%-4.1rem)] transition-all duration-300 bg-gray-100 dark:bg-slate-900`}
	  >
		<div className="flex items-center justify-between px-4">
		  {/* Logo */}
		  <h1 className="text-2xl font-bold">D</h1>
  
		  {/* Search Bar */}
		  <div className="relative flex items-center my-2 rounded-2xl w-1/3 bg-secondary">
			<HiSearch className="absolute left-2" size={20} />
			<input
			  type="text"
			  placeholder="Search..."
			  className="h-10 bg-white dark:bg-gray-800 placeholder-text pl-8 pr-4 rounded-3xl w-full focus:outline-none"
			/>
		  </div>
  
		  <div className="flex items-center space-x-4">
			{/* Dark Mode Toggle */}
			<button
			  onClick={toggleDarkMode}
			  className="p-2 rounded-full focus:outline-none text-primary "
			>
			  {isDarkMode ? <FaSun /> : <FaMoon />}
			</button>
  
			{/* Navigation Menu */}
			<NavigationMenu>
			  <NavigationMenuList className="flex items-center space-x-4 ">
				{/* Notifications Button */}
				<NavigationMenuItem>
				  <div className="relative ">
					<NavigationMenuTrigger className="flex items-center space-x-2 px-3 bg-transparent py-2 rounded-lg ">
					  <IoIosNotifications size={20} />
					</NavigationMenuTrigger>
					<NavigationMenuContent className="rounded-lg shadow-lg bg-secondary">
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
					<NavigationMenuTrigger className="flex items-center space-x-2 bg-transparent px-3 py-2">
					  <span>Features</span>
					</NavigationMenuTrigger>
					<NavigationMenuContent className="rounded-lg shadow-lg bg-secondary">
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
  