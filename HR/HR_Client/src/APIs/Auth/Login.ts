import { TServiceResponse } from '../../types/TServiceResponse';

export type TLoginData = {
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
};

export interface ILoginRequestDto {
    email: string;
    password: string
}

export async function login(loginRequest: ILoginRequestDto): Promise<TServiceResponse<TLoginData>> {
  const response = await fetch('https://localhost:7017/api/Auth/login', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({
        email: loginRequest.email,
        password: loginRequest.password,
    }),
  });

  const data = await response.json();

  if(!data && !data?.message) throw new Error('Login failed'); 
  
  return data;
}
