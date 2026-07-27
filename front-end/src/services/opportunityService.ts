import api from './api';
import { ResultViewModel, Opportunity } from '../types';

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
  }
};