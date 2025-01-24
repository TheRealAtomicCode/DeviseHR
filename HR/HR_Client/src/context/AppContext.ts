import { createContext } from 'react';

// Define the TUserContext type
type TUserContext = {
	email: string;
	firstName: string;
	id: number;
	isTerminated: boolean;
	isVerified: boolean;
	lastName: string;
	profilePicture: string | null;
	userRole: number;
};

// Define the default value for the context
export const defaultUserContext: TUserContext = {
	email: '',
	firstName: '',
	id: 0,
	isTerminated: false,
	isVerified: false,
	lastName: '',
	profilePicture: null,
	userRole: 0,
};

export const UserContext = createContext<TUserContext>(defaultUserContext);
