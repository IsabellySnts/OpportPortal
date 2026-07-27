import api from './api';
import { ResultViewModel, LoginResponse } from '../types';

export const authService = {
  login: async (email: string, password: string) => {
    const response = await api.post<ResultViewModel<LoginResponse>>('/User/login', {
      email,
      password
    });
    return response.data;
  },

  register: async (email: string, password: string, type: number) => {
    const response = await api.post<ResultViewModel<number>>('/User/register', {
      userEmail: email,
      password,
      type
    });
    return response.data;
  }
};