import { ReactElement } from 'react';
import { Navigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

interface RoleRouteProps {
  children: ReactElement;
  allowedRole: string;
}

export default function RoleRoute({ children, allowedRole }: RoleRouteProps) {
  const { isAuthenticated, userType } = useAuth();

  if (!isAuthenticated) {
    return <Navigate to="/login" />;
  }

  if (userType !== allowedRole) {
    return <Navigate to={userType === 'Company' ? '/company/dashboard' : '/opportunities'} />;
  }

  return children;
}