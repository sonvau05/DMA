import axios from "axios";

const apiClient = axios.create({
  baseURL: "http://localhost:5100/api",
  timeout: 5000,
});

apiClient.interceptors.response.use(
  res => res,
  err => {
    console.error("API error:", err);
    return Promise.reject(err);
  }
);

export default apiClient;
