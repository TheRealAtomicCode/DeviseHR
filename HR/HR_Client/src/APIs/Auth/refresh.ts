import { IServiceResponse } from '../../Interfaces/IServiceResponse';

export interface IRefreshData {
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


export async function refresh(refreshToken: string): Promise<IServiceResponse<IRefreshData>> {
  const response = await fetch('https://localhost:7017/api/Auth/refresh', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(refreshToken),
  });

  const data = await response.json();

  if(!data && !data?.message) throw new Error('Authentication failed'); 
  
  return data;
}
