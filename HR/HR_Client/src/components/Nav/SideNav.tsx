import { FC } from 'react';
import { FaFire } from 'react-icons/fa';
import { IoHomeOutline } from 'react-icons/io5';
import { PiUsersThreeLight } from 'react-icons/pi';
import { CiUser } from 'react-icons/ci';
import { IoCalendarOutline } from 'react-icons/io5';
import { TiFolderOpen } from 'react-icons/ti';
import { GoGear } from 'react-icons/go';

// Define the type for the SideBarIcon props
interface SideBarIconProps {
	icon: JSX.Element;
	title?: string;
}

function SideNav() {
	return (
		<div className="side-nav fixed top-0 left-0 h-screen w-16 m-0 flex flex-col bg-gray-900 text-white shadow-lg overflow-y-auto overflow-x-hidden scrollbar-hidden">
			<i>
				<div className="cursor-pointer w-14 h-14 mt-4 rounded-full border border-white items-center justify-center mx-auto"></div>
			</i>
			<i className="mt-12">
				<SideBarIcon icon={<IoHomeOutline size="28" />} title={'Home'} />
			</i>
			<i>
				<SideBarIcon icon={<PiUsersThreeLight size="28" />} title={'Users'} />
			</i>
			<i>
				<SideBarIcon icon={<CiUser size="28" />} title={'Profile'} />
			</i>
			<i>
				<SideBarIcon
					icon={<IoCalendarOutline size="28" />}
					title={'Calendar'}
				/>
			</i>
			<i>
				<SideBarIcon icon={<TiFolderOpen size="28" />} title={'Files'} />
			</i>
			<i className="mt-auto mb-6">
				<SideBarIcon icon={<GoGear size="28" />} />
			</i>
		</div>
	);
}

const SideBarIcon: FC<SideBarIconProps> = ({ icon, title }) => {
	return (
		<div className="hover:text-green-400 cursor-pointer group">
			<div className="relative flex items-center justify-center h-12 w-12 mt-2 mb-[2px] mx-auto bg-gray-800 rounded-lg group-hover:rounded-3xl transition-all duration-200">
				{icon}
			</div>
			{title && (
				<p className="mx-auto text-white text-xs mt-1 group-hover:text-green-400 text-center">
					{title}
				</p>
			)}
		</div>
	);
};

export default SideNav;
