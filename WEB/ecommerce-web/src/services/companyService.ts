import apiClient from '../api/client';
import type { Company } from '../types';

/**
 * Company Service - handles all company-related API calls
 * Uses the Company type from @/types for consistency
 */
export const CompanyService = {
  /**
   * Get a company by ID
   * @param companyId - The company ID to fetch
   * @returns Promise containing the company data
   */
  async getCompanyById(companyId: string): Promise<Company> {
    const response = await apiClient.get<Company>(`/companies/${companyId}`);
    return response.data;
  },

  /**
   * Get all companies
   * @returns Promise containing array of all companies
   */
  async getAllCompanies(): Promise<Company[]> {
    const response = await apiClient.get<Company[]>('/companies');
    return response.data;
  },

  /**
   * Create a new company
   * @param data - Company data (name and email required)
   * @returns Promise containing the created company
   */
  async createCompany(data: {
    name: string;
    email: string;
  }): Promise<Company> {
    const response = await apiClient.post<Company>('/companies', data);
    return response.data;
  },

  /**
   * Update an existing company
   * @param companyId - The company ID to update
   * @param data - Partial company data to update (name and/or email)
   * @returns Promise containing the updated company
   */
  async updateCompany(
    companyId: string,
    data: { name?: string; email?: string }
  ): Promise<Company> {
    const response = await apiClient.put<Company>(
      `/companies/${companyId}`,
      data
    );
    return response.data;
  },

  /**
   * Delete a company
   * @param companyId - The company ID to delete
   * @returns Promise that resolves when deletion is complete
   */
  async deleteCompany(companyId: string): Promise<void> {
    await apiClient.delete(`/companies/${companyId}`);
  },
}; 