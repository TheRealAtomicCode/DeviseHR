import { createContext } from 'react';

interface AppContextType {
	jwt: string;
	refreshToken: string;
}

export const appContext: AppContextType = {
	jwt: '',
	refreshToken: '',
};

export const AppContext = createContext<AppContextType>(appContext);
