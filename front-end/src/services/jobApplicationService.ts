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
  }
};