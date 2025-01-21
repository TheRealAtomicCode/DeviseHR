export interface IServiceResponse<T> {
    data: T;
    success: boolean;
    isPending: boolean;
    message: string;
    errorCode: number;
    jwt: string | null;
    refreshToken: string | null;
  }