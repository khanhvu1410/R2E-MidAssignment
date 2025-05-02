export enum UserRole {
  NormalUser = 0,
  SuperUser = 1,
}

export interface User {
  username: string;
  email: string;
  role: UserRole;
}

export interface LoginCredentials {
  username: string;
  password: string;
}

export interface RegisterData extends LoginCredentials {
  email: string;
  role: UserRole;
}
