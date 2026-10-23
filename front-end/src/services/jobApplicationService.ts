import api from './api';
import { ResultViewModel, JobApplication } from '../types';

export const jobApplicationService = {

  create: async (opportunityId: number) => {
    const response = await api.post<ResultViewModel<number>>('/JobApplication', {
      opportunityId
    });
    return response.data;
  },

  getByCandidate: async (candidateId: number) => {
    const response = await api.get<ResultViewModel<JobApplication[]>>(
      `/JobApplication/candidate/${candidateId}`
    );
    return response.data;
  },

  getByOpportunity: async (opportunityId: number) => {
    const response = await api.get<ResultViewModel<JobApplication[]>>(
      `/JobApplication/opportunity/${opportunityId}`
    );
    return response.data;
  },

  updateStatus: async (id: number, newStatus: number) => {
    const response = await api.patch<ResultViewModel<number>>(
      `/JobApplication/${id}/status`,
      { id, newStatus }
    );
    return response.data;
  },

  cancel: async (id: number) => {
    const response = await api.delete<ResultViewModel<number>>(`/JobApplication/${id}`);
    return response.data;
  },

};