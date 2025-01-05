import { useState, FC } from 'react';
import { FaFire } from 'react-icons/fa';

import './App.css';

// Define the type for the SideBarIcon props
interface SideBarIconProps {
	icon: JSX.Element; // The icon will be a JSX element
}

function App() {
	return (
		<div className="fixed top-0 left-0 h-screen w-16 m-0 flex flex-col bg-gray-900 text-white shadow-lg">
			<i>
				<SideBarIcon icon={<FaFire size="28" />} />
			</i>
			<i>
				<SideBarIcon icon={<FaFire size="28" />} />
			</i>
			<i>
				<SideBarIcon icon={<FaFire size="28" />} />
			</i>
			<i>
				<SideBarIcon icon={<FaFire size="28" />} />
			</i>
			<i>
				<SideBarIcon icon={<FaFire size="28" />} />
			</i>
		</div>
	);
}

const SideBarIcon: FC<SideBarIconProps> = ({ icon }) => {
	return (
		<div className=" hover:text-pink-500 cursor-pointer ">
			<div className="relative flex items-center justify-center h-12 w-12 mt-4 mb-1 mx-auto bg-gray-800 rounded-lg hover:rounded-3xl transition-all duration-200">
				{icon}
			</div>
			<p className="mx-auto white w-min text-xs mt-0 pt-0 transition-all duration-200">
				hello
			</p>
		</div>
	);
};

export default App;
