import {
  createContext,
  ReactNode,
  useContext,
  useEffect,
  useState,
} from 'react';
import { User } from '../models/auth';

interface AuthContextType {
  user: User | null;
  token: string | null;
  login: (user: User, accessToken: string) => void;
  logout: () => void;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType>({
  user: null,
  token: null,
  login: () => {
    throw new Error('login function must be overridden by AuthProvider');
  },
  logout: () => {
    throw new Error('logout function must be overridden by AuthProvider');
  },
  isAuthenticated: false,
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [user, setUser] = useState<User | null>(null);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    const initializeAuth = async () => {
      const storedTokens = localStorage.getItem('token');
      const storedUser = localStorage.getItem('user');

      if (storedTokens && storedUser) {
        setToken(storedTokens);
        setUser(JSON.parse(storedUser));
      }
    };

    initializeAuth();
  }, []);

  const login = (user: User, accessToken: string) => {
    localStorage.setItem('token', accessToken);
    localStorage.setItem('user', JSON.stringify(user));
    setToken(accessToken);
    setUser(user);
  };

  const logout = () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    setToken(null);
    setUser(null);
  };

  const value = {
    user,
    token,
    login,
    logout,
    isAuthenticated: !!token,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuthContext = () => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
