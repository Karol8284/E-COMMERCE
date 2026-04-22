import apiClient from '../api/client';

export interface UserProfile {
  id: string;
  email: string;
  firstName?: string;
  lastName?: string;
  phone?: string;
  address?: string;
  city?: string;
  zipCode?: string;
  role: string;
  createdAt: string;
  updatedAt: string;
}

export interface UpdateProfileRequest {
  firstName?: string;
  lastName?: string;
  phone?: string;
  address?: string;
  city?: string;
  zipCode?: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
  confirmPassword: string;
}

/**
 * User Service - handles user profile and account-related API calls
 */
export const userService = {
  /**
   * Pobierz profil użytkownika
   */
  getProfile: async (): Promise<UserProfile> => {
    const response = await apiClient.get<UserProfile>('/user/profile');
    return response.data;
  },

  /**
   * Zaktualizuj profil użytkownika
   */
  updateProfile: async (data: UpdateProfileRequest): Promise<UserProfile> => {
    const response = await apiClient.put<UserProfile>('/user/profile', data);
    return response.data;
  },

  /**
   * Zmień hasło
   */
  changePassword: async (data: ChangePasswordRequest): Promise<{ message: string }> => {
    const response = await apiClient.post<{ message: string }>(
      '/user/change-password',
      data
    );
    return response.data;
  },

  /**
   * Usuń konto użytkownika
   */
  deleteAccount: async (): Promise<void> => {
    await apiClient.delete('/user/account');
  },

  /**
   * Pobierz wszystkie adresy użytkownika
   */
  getAddresses: async (): Promise<UserProfile['address'][]> => {
    const response = await apiClient.get<UserProfile['address'][]>('/user/addresses');
    return response.data;
  },

  /**
   * Dodaj nowy adres
   */
  addAddress: async (address: string): Promise<UserProfile['address']> => {
    const response = await apiClient.post<UserProfile['address']>('/user/addresses', {
      address,
    });
    return response.data;
  },
};
