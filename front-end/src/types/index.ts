export interface LoginResponse {
  token: string;
}

export interface ResultViewModel<T> {
  data: T;
  isSuccess: boolean;
  message: string;
}

export enum UserType {
  Candidate = 1,
  Company = 2
}

export enum OpportunityType {
  Internship = 1,
  Freelance = 2,
  CLT = 3,
  PJ = 4
}

export enum JobFormat {
  OnSite = 1,
  Remote = 2,
  Hybrid = 3
}

export enum JobApplicationStatus {
  Registered = 1,
  UnderReview = 2,
  InterviewScheduled = 3,
  FinalProcess = 4,
  Approved = 5,
  NotAdvanced = 6,
  Withdrawn = 7,
  PositionClosed = 8
}

export interface Opportunity {
  id: number;
  companyId: number;
  companyName: string;
  title: string;
  description: string;
  type: OpportunityType;
  format: JobFormat;
  requirements: string;
  fieldOfWork: string;
  state: string;
  salary: number;
  status: string;
  createdAt: string;
}

export interface JobApplication {
  id: number;
  candidateId: number;
  candidateName: string;
  opportunityId: number;
  opportunityTitle: string;
  status: JobApplicationStatus;
  createdAt: string;
  updatedAt?: string;
}