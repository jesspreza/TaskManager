import { useRouter } from 'vue-router';

export function useAuth() {
  const router = useRouter();

  const logout = () => {
    localStorage.removeItem('token');
    router.push('/login');
  };

  return {
    logout,
  };
}