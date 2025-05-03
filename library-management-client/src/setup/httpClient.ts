import axios from 'axios';
import { ROOT_API } from './config';
import { useAuthContext } from '../context/AuthContext';

const axiosInstance = axios.create(ROOT_API);

axiosInstance.interceptors.request.use((config) => {
  const token = localStorage.getItem('token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

axiosInstance.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response.status === 401 || error.response.status === 403) {
      const { logout } = useAuthContext();
      logout();
      window.location.href = '/';
    }
    return Promise.reject(error);
  }
);

export const httpClient = axiosInstance;
