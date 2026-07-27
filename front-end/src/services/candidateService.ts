import api from './api';
import { ResultViewModel } from '../types';

export interface CreateCandidateRequest {
  fullName: string;
  birthDate: string;
  zipCode: string;
  street: string;
  number: string;
  neighborhood: string;
  city: string;
  state: string;
  country: string;
  complement?: string;
  phoneNumber: string;
  urlLinkedin?: string;
}

export interface CandidateResponse {
  id: number;
  userId: number;
  fullName: string;
  birthDate: string;
  phoneNumber: string;
  city: string;
  state: string;
  urlLinkedin?: string;
}

export const candidateService = {
  create: async (data: CreateCandidateRequest) => {
    const response = await api.post<ResultViewModel<number>>('/Candidate', data);
    return response.data;
  },

  getMe: async () => {
    const response = await api.get<ResultViewModel<CandidateResponse>>('/Candidate/me');
    return response.data;
  }
};