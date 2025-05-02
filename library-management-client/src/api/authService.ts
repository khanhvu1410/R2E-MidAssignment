import { LoginCredentials, RegisterData, User, UserRole } from '../models/auth';
import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const loginService = async (
  credentials: LoginCredentials
): Promise<{ user: User; accessToken: string }> => {
  const response = await httpClient.post(ENDPOINT_API.auth.login, credentials);
  return response.data;
};

export const registerService = async (data: RegisterData): Promise<void> => {
  // Default to NormalUser if role not specified
  const registrationData = {
    ...data,
    role: data.role || UserRole.NormalUser,
  };
  const response = await httpClient.post(
    ENDPOINT_API.auth.register,
    registrationData
  );
  return response.data;
};
