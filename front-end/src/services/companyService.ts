import api from './api';
import { ResultViewModel } from '../types';

export interface CompanyResponse {
  id: number;
  userId: number;
  companyName: string;
  cnpj: string;
  sector: string;
  email: string;
  phoneNumber: string;
  city: string;
  state: string;
}

export interface CreateCompanyRequest {
  companyName: string;
  cnpj: string;
  sector: string;
  email: string;
  phoneNumber: string;
  zipCode: string;
  street: string;
  number: string;
  neighborhood: string;
  city: string;
  state: string;
  country: string;
  complement?: string;
}

export const companyService = {
  create: async (data: CreateCompanyRequest) => {
    const response = await api.post<ResultViewModel<number>>('/Company', data);
    return response.data;
  },

  getMe: async () => {
    const response = await api.get<ResultViewModel<CompanyResponse>>('/Company/me');
    return response.data;
  }
};