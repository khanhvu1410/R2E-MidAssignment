import './App.css';
import { useRoutes } from 'react-router-dom';
import { AppRoutes } from './routes';
import '@ant-design/v5-patch-for-react-19';
import { AuthProvider } from './context/AuthContext';

function App() {
  const element = useRoutes(AppRoutes);
  return (
    <AuthProvider>
      <div>{element}</div>
    </AuthProvider>
  );
}

export default App;
