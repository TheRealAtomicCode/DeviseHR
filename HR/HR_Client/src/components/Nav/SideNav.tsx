import { FC, useContext } from 'react';
import { FaRegClock } from 'react-icons/fa';
import { IoHomeOutline } from 'react-icons/io5';
import { PiUsersThreeLight } from 'react-icons/pi';
import { IoCalendarOutline } from 'react-icons/io5';
import { TiFolderOpen } from 'react-icons/ti';
import { GoGear } from 'react-icons/go';
import { BiJoystickAlt } from 'react-icons/bi'; // For the plus icon
import { GiHamburgerMenu } from 'react-icons/gi'; // For the burger menu
import { Link } from 'react-router-dom';
import { UserContext } from '../../context/AppContext';

type TSideBarIconProps = {
	icon: JSX.Element;
	title?: string;
};

function SideNav() {
	const { id } = useContext(UserContext);

	return (
		<div>
			{/* Sidebar for larger screens */}
			<div className="side-nav hidden sm:flex fixed top-0 left-0 h-screen w-16 m-0 flex-col bg-gray-900 text-white shadow-lg overflow-y-auto overflow-x-hidden scrollbar-hidden">
				<i>
					<Link to={'/users/' + id}>
						<div className="cursor-pointer w-14 h-14 mt-4 rounded-full border border-white items-center justify-center mx-auto"></div>
					</Link>
				</i>
				<i className="mt-16">
					<Link to="/home">
						<MdSideBarIcon icon={<IoHomeOutline size="28" />} title={'Home'} />
					</Link>
				</i>

				<i>
					<Link to="/users">
						<MdSideBarIcon
							icon={<PiUsersThreeLight size="28" />}
							title={'Users'}
						/>
					</Link>
				</i>
				<i>
					<Link to="/calendar">
						<MdSideBarIcon
							icon={<IoCalendarOutline size="28" />}
							title={'Calendar'}
						/>
					</Link>
				</i>
				<i>
					<Link to="/rotas">
						<MdSideBarIcon icon={<FaRegClock size="28" />} title={'Rotas'} />
					</Link>
				</i>

				<i>
					<Link to="/files">
						<MdSideBarIcon icon={<TiFolderOpen size="28" />} title={'Files'} />
					</Link>
				</i>
				<i className="mt-auto mb-6">
					<Link to="/settings">
						<MdSideBarIcon icon={<GoGear size="28" />} />
					</Link>
				</i>
			</div>

			{/* Bottom bar for mobile screens */}
			<div className="sm:hidden fixed bottom-0 left-0 w-full bg-gray-900 text-white flex justify-between items-center p-2">
				<Link to="/home">
					<SmSideBarIcon icon={<IoHomeOutline size="28" />} />
				</Link>
				<Link to="/rotas">
					<SmSideBarIcon icon={<FaRegClock size="28" />} />
				</Link>
				<Link to="/home">
					<SmSideBarIcon icon={<BiJoystickAlt size="28" />} />
				</Link>
				<Link to="/calendar">
					<SmSideBarIcon icon={<IoCalendarOutline size="28" />} />
				</Link>
				<Link to="/home">
					<SmSideBarIcon icon={<GiHamburgerMenu size="28" />} />
				</Link>
			</div>
		</div>
	);
}

const MdSideBarIcon: FC<TSideBarIconProps> = ({ icon, title }) => {
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

const SmSideBarIcon: FC<TSideBarIconProps> = ({ icon, title }) => {
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
