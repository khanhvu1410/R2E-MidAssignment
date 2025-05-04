import { LoginCredentials, RegisterData, User, UserRole } from '../models/auth';
import { ENDPOINT_API } from '../setup/config';
import { httpClient } from '../setup/httpClient';

export const loginService = (credentials: LoginCredentials) => {
  return httpClient.post(ENDPOINT_API.auth.login, credentials);
};

export const registerService = (data: RegisterData) => {
  // Default to NormalUser if role not specified
  const registrationData = {
    ...data,
    role: data.role || UserRole.NormalUser,
  };
  return httpClient.post(ENDPOINT_API.auth.register, registrationData);
};
