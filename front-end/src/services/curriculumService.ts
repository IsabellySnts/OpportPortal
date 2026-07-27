import api from './api';
import { ResultViewModel } from '../types';

export interface CreateCurriculumRequest {
  educationLevel: number;
  hardSkills: string[];
  softSkills: string[];
}

export const curriculumService = {
  create: async (data: CreateCurriculumRequest) => {
    const response = await api.post<ResultViewModel<number>>('/Curriculum', data);
    return response.data;
  }
};