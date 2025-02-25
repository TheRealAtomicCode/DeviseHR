import { TLoginData, TLoginRequestDto } from '../../types/TLogin';
import { TServiceResponse } from '../../types/TServiceResponse';
import endpoints from '../endpoints';

export async function login(
  loginRequest: TLoginRequestDto
): Promise<TServiceResponse<TLoginData>> {
  const response = await fetch(`${endpoints.hrBackend}/api/Auth/login`, {
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

  if (!data && !data?.message) throw new Error('Login failed');

  return data;
}
