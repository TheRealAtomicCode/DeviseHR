import { TServiceResponse } from '../../types/TServiceResponse';
import endpoints from '../endpoints';

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

export type TRefreshRequest = {
  jwt: string | null;
  refreshToken: string | null;
};

export async function refresh(
  tokens: TRefreshRequest
): Promise<TServiceResponse<IRefreshData>> {
  const response = await fetch(`${endpoints.hrBackend}/api/Auth/refresh`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: `Bearer ${tokens.jwt}`,
    },
    body: JSON.stringify({ refreshToken: tokens.refreshToken }),
  });

  const data = await response.json();

  if (!data && !data?.message) throw new Error('Authentication failed');

  return data;
}
