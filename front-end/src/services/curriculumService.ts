import api from './api';
import { ResultViewModel } from '../types';

export interface CreateCurriculumRequest {
  educationLevel: number;
  hardSkills: string[];
  softSkills: string[];
}

export interface CurriculumResponse {
  id: number;
  candidateId: number;
  educationLevel: number;
  hardSkills: string[];
  softSkills: string[];
}

export interface UpdateCurriculumRequest {
  id: number;
  educationLevel: number;
  hardSkills: string[];
  softSkills: string[];
}


export const curriculumService = {
  create: async (data: CreateCurriculumRequest) => {
    const response = await api.post<ResultViewModel<number>>('/Curriculum', data);
    return response.data;
  },

  getByCandidate: async (candidateId: number) => {
    const response = await api.get<ResultViewModel<CurriculumResponse>>(
      `/Curriculum/candidate/${candidateId}`
    );
    return response.data;
  },

  update: async (id: number, data: UpdateCurriculumRequest) => {
    const response = await api.put<ResultViewModel<number>>(`/Curriculum/${id}`, data);
    return response.data;
  }
}; 