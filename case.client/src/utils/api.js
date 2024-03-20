import axios from "axios";
import { toast } from "react-hot-toast";

const api = axios.create({
    baseURL: import.meta.env.VITE_API_BASE_URL,
});

api.interceptors.request.use(
  (config) => {
    return config;
  },
  (error) => {
    toast.error("Request failed. Please try again later.");
    return Promise.reject(error);
  }
);


api.interceptors.response.use(
  (response) => {
    return response;
  },
  (error) => {
    if (error.response) {
      const status = error.response.status;
      if (status === 401) {
        toast.error("Yetkisiz. Lütfen tekrar giriş yapın.");
      } else if (status === 404) {
        toast.error("Kaynak bulunamadı.");
      } else {
        toast.error("Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
      }
    } else if (error.request) {
      toast.error("Sunucudan yanıt yok. Lütfen daha sonra tekrar deneyiniz.");
    } else {
      toast.error("Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.");
    }
    return Promise.reject(error);
  }
);

export default api;