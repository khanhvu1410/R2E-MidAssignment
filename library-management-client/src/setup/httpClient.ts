import axios from 'axios';
import { ROOT_API } from './config';

const axiosInstance = axios.create(ROOT_API);

export const httpClient = axiosInstance;
