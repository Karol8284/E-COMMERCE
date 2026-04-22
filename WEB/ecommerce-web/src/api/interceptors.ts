
import { type AxiosInstance,AxiosError } from "axios";

export const setupInterceptors = (apiClient: AxiosInstance) => {
    // dadanie tokenu
    apiClient.interceptors.request.use((config) => {
        const token = localStorage.getItem("token");
        if (token) {
            config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
    });

    apiClient.interceptors.response.use(
        (response) => response,
        (error: AxiosError) => {
            if (error.response?.status === 401) {
                // koniec życia tokenu
                localStorage.removeItem("authToken");
                window.location.href = "/login";
            }
            if(error.response?.status === 403) {
                // brak uprawnień
                console.error("Forbidden:", error.response.data);
            }
            return Promise.reject(error);
        }
    )
}