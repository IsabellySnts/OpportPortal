import api from './api';
import { ResultViewModel, Opportunity } from '../types';

export interface CreateOpportunityRequest {
  title: string;
  description: string;
  type: number;
  format: number;
  requirements: string;
  fieldOfWork: string;
  state: string;
  salary: number;
}

export const opportunityService = {
  getAll: async () => {
    const response = await api.get<ResultViewModel<Opportunity[]>>('/Opportunity');
    return response.data;
  },

  getByFilter: async (params: {
    fieldOfWork?: string;
    state?: string;
    type?: number;
    format?: number;
  }) => {
    const response = await api.get<ResultViewModel<Opportunity[]>>('/Opportunity/filter', {
      params
    });
    return response.data;
  },

  getById: async (id: number) => {
    const response = await api.get<ResultViewModel<Opportunity>>(`/Opportunity/${id}`);
    return response.data;
  },

  create: async (data: CreateOpportunityRequest) => {
    const response = await api.post<ResultViewModel<number>>('/Opportunity', data);
    return response.data;
  },

  getByCompany: async (companyId: number) => {
    const response = await api.get<ResultViewModel<Opportunity[]>>(`/Opportunity/company/${companyId}`);
    return response.data;
  },

  close: async (id: number) => {
    const response = await api.patch<ResultViewModel<number>>(`/Opportunity/${id}/close`);
    return response.data;
  }
};