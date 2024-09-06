import axios from 'axios';

const instance = axios.create({
  baseURL: 'https://localhost:44339',
});

instance.interceptors.request.use(
  config => {
    const token = localStorage.getItem('token');
    if (token) {
      config.headers['Authorization'] = `Bearer ${token}`;
    }
    return config;
  },
  error => {
    return Promise.reject(error);
  }
);

instance.interceptors.response.use(
  response => response,
  error => {
    if (error.response.status === 401) {
      // Se não autorizado, redireciona para a página de login
      localStorage.removeItem('token');
      router.push('/login');
    }
    return Promise.reject(error);
  }
);

instance.interceptors.response.use(
  response => response,
  error => {
      if (error.response) {
          console.error('Erro: ', error.response.data);
      } else if (error.request) {
          console.error('Erro de rede: ', error.request);
      } else {
          console.error('Erro: ', error.message);
      }
      return Promise.reject(error);
  }
);

export default instance;