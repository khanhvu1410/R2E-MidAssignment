import { useAuthContext } from '../../context/AuthContext';
import { Navigate } from 'react-router-dom';
import { PATH } from '../../constants/paths';
import { JSX } from 'react';
import { UserRole } from '../../models/auth';

interface PrivateRouteProps {
  children: JSX.Element;
  requiredRole: UserRole;
}

const PrivateRoute = (props: PrivateRouteProps) => {
  const { user, isAuthenticated } = useAuthContext();

  return isAuthenticated && user?.role === props.requiredRole ? (
    props.children
  ) : (
    <Navigate to={PATH.auth.login} replace />
  );
};

export default PrivateRoute;
