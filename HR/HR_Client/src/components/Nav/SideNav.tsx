import { FC } from 'react';
import { FaRegClock } from 'react-icons/fa';
import { IoHomeOutline } from 'react-icons/io5';
import { PiUsersThreeLight } from 'react-icons/pi';
import { IoCalendarOutline } from 'react-icons/io5';
import { TiFolderOpen } from 'react-icons/ti';
import { GoGear } from 'react-icons/go';
import { BiJoystickAlt } from 'react-icons/bi'; // For the plus icon
import { GiHamburgerMenu } from 'react-icons/gi'; // For the burger menu

// Define the type for the SideBarIcon props
interface SideBarIconProps {
	icon: JSX.Element;
	title?: string;
}

function SideNav() {
	return (
		<div>
			{/* Sidebar for larger screens */}
			<div className="side-nav hidden sm:flex fixed top-0 left-0 h-screen w-16 m-0 flex-col bg-gray-900 text-white shadow-lg overflow-y-auto overflow-x-hidden scrollbar-hidden">
				<i>
					<div className="cursor-pointer w-14 h-14 mt-4 rounded-full border border-white items-center justify-center mx-auto"></div>
				</i>
				<i className="mt-8">
					<MdSideBarIcon icon={<IoHomeOutline size="28" />} title={'Home'} />
				</i>
				<i>
					<MdSideBarIcon
						icon={<BiJoystickAlt size="28" />}
						title={'Activity'}
					/>
				</i>
				<i>
					<MdSideBarIcon
						icon={<PiUsersThreeLight size="28" />}
						title={'Users'}
					/>
				</i>
				<i>
					<MdSideBarIcon
						icon={<IoCalendarOutline size="28" />}
						title={'Calendar'}
					/>
				</i>
				<i>
					<MdSideBarIcon icon={<FaRegClock size="28" />} title={'Rotas'} />
				</i>
				<i>
					<MdSideBarIcon icon={<TiFolderOpen size="28" />} title={'Files'} />
				</i>
				<i className="mt-auto mb-6">
					<MdSideBarIcon icon={<GoGear size="28" />} />
				</i>
			</div>

			{/* Bottom bar for mobile screens */}
			<div className="sm:hidden fixed bottom-0 left-0 w-full bg-gray-900 text-white flex justify-between items-center p-2">
				<SmSideBarIcon icon={<IoHomeOutline size="28" />} />
				<SmSideBarIcon icon={<FaRegClock size="28" />} />
				<SmSideBarIcon icon={<BiJoystickAlt size="28" />} />
				<SmSideBarIcon icon={<IoCalendarOutline size="28" />} />
				<SmSideBarIcon icon={<GiHamburgerMenu size="28" />} />
			</div>
		</div>
	);
}

const MdSideBarIcon: FC<SideBarIconProps> = ({ icon, title }) => {
	return (
		<div className="hover:text-green-400 cursor-pointer group">
			<div className="relative flex items-center justify-center h-12 w-12 mt-2 mb-[2px] mx-auto bg-gray-800 rounded-lg group-hover:rounded-3xl transition-all duration-200">
				{icon}
			</div>
			{/* Hide text on small screens */}
			{title && (
				<p className="mx-auto text-white text-[10px] mt-1 group-hover:text-green-400 text-center hidden sm:block">
					{title}
				</p>
			)}
		</div>
	);
};

const SmSideBarIcon: FC<SideBarIconProps> = ({ icon, title }) => {
	return (
		<div className="hover:text-green-400 cursor-pointer group">
			<div className="relative flex items-center justify-center h-8 w-12  mb-[2px] mx-auto  rounded-lg group-hover:rounded-3xl transition-all duration-200">
				{icon}
			</div>
			{/* Hide text on small screens */}
			{title && (
				<p className="mx-auto text-white text-xs mt-1 group-hover:text-green-400 text-center hidden sm:block">
					{title}
				</p>
			)}
		</div>
	);
};

export default SideNav;
