import axios from "axios";

const baseURL = import.meta.env.VITE_API_BASE_URL;

const api = axios.create({
  baseURL: "http://localhost:5123",
});

// 요청 인터셉터 > 요청 나가기 전
api.interceptors.request.use(
  (config) => {
    // 요청 전 수행
    return config;
  },
  (error) => {
    return Promise.reject(error);
  },
);

// // 응답 인터셉터 > 응답이 온 후 (직후)
// api.interceptors.response.use(
//   (response) => {
//     // 응답 가공 혹은 반환
//     return response;
//   },
//   (error) => {
//     if (error.response) {
//       const status = error.response.status;
//       console.error(`error occurred in 인터셉터, status: ${status}`);
//     }
//     return Promise.reject(error);
//   },
// );

export default api;
