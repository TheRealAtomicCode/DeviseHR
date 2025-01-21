import { IServiceResponse } from '../Interfaces/IServiceResponse';

export interface ILoginData {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    profilePicture: string | null;
    isTerminated: boolean;
    isVerified: boolean;
    userRole: number;
    createdAt: string;
    jwt: string;
    refreshToken: string;
}

export async function login(email: string, password: string): Promise<IServiceResponse<ILoginData>> {
  const response = await fetch('/api/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
      email,
      password,
    }),
  });

  if (!response.ok) {
    // Throw an error if the request fails
    throw new Error('Login failed');
  }

  const data = await response.json();
  
  return data;
}
