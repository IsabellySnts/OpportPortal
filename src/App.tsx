import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider, useAuth } from './contexts/AuthContext';

// Pages (vamos criar em seguida)
import LoginPage from './pages/auth/LoginPage';
import RegisterPage from './pages/auth/RegisterPage';
import OpportunitiesPage from './pages/opportunity/OpportunitiesPage';
import CandidateSetupPage from './pages/candidate/CandidateSetupPage';
import { ReactElement } from 'react';


// Rota protegida
function PrivateRoute({ children }: { children: ReactElement }) {
  const { isAuthenticated } = useAuth();
  return isAuthenticated ? children : <Navigate to="/login" />;
}

function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <Routes>
          {/* Públicas */}
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />

          {/* Protegidas */}
          <Route path="/opportunities" element={
            <PrivateRoute>
              <OpportunitiesPage />
            </PrivateRoute>
          } />
          <Route path="/setup/candidate" element={
            <PrivateRoute>
              <CandidateSetupPage />
            </PrivateRoute>
          } />

          {/* Redirect padrão */}
          <Route path="/" element={<Navigate to="/opportunities" />} />
        </Routes>
      </BrowserRouter>
    </AuthProvider>
  );
}

export default App;