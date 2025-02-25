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

export type TLoginRequestDto = {
  email: string;
  password: string;
};
