import './App.css';
import { useRoutes } from 'react-router-dom';
import { AppRoutes } from './routes';
import '@ant-design/v5-patch-for-react-19';

function App() {
  const element = useRoutes(AppRoutes);
  return element;
}

export default App;
