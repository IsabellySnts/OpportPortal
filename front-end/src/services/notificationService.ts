import api from './api';
import { ResultViewModel } from '../types';

export interface NotificationItem {
  id: number;
  title: string;
  message: string;
  type: number;
  isRead: boolean;
  createdAt: string;
}

export const notificationService = {
  getMy: async () => {
    const response = await api.get<ResultViewModel<NotificationItem[]>>('/Notification');
    return response.data;
  },
  getUnreadCount: async () => {
    const response = await api.get<ResultViewModel<number>>('/Notification/unread-count');
    return response.data;
  },
  markAsRead: async (id: number) => {
    const response = await api.patch<ResultViewModel<boolean>>(`/Notification/${id}/read`, {});
    return response.data;
  },
  markAllAsRead: async () => {
    const response = await api.patch<ResultViewModel<boolean>>('/Notification/read-all', {});
    return response.data;
  },
};