import { Routes, Route, Navigate } from 'react-router-dom';
import PrivateRoute from './PrivateRoute';

// Auth
import LoginPage from '../pages/auth/LoginPage';
import RegisterPage from '../pages/auth/RegisterPage';

// Candidate
import CandidateSetupPage from '../pages/candidate/CandidateSetupPage';
import MyApplicationsPage from '../pages/candidate/MyApplicationsPage';
import ProfilePage from '../pages/candidate/ProfilePage';

// Opportunity
import OpportunitiesPage from '../pages/opportunity/OpportunitiesPage';

// Company
import CompanySetupPage from '../pages/company/CompanySetupPage';
import CompanyDashboardPage from '../pages/company/CompanyDashboardPage';
import NewOpportunityPage from '../pages/company/NewOpportunityPage';
import OpportunityCandidatesPage from '../pages/company/OpportunityCandidatesPage';

// Settings
import SettingsPage from '../pages/settings/SettingsPage';

import RoleRoute from './RoleRoute';


export default function AppRoutes() {
  return (
    <Routes>
      {/* Públicas */}
      <Route path="/login" element={<LoginPage />} />
      <Route path="/register" element={<RegisterPage />} />

      {/* Candidato */}
      <Route path="/setup/candidate" element={<PrivateRoute><CandidateSetupPage /></PrivateRoute>} />
      <Route path="/opportunities" element={<PrivateRoute><OpportunitiesPage /></PrivateRoute>} />
      <Route path="/my-applications" element={<PrivateRoute><MyApplicationsPage /></PrivateRoute>} />
      <Route path="/profile" element={<PrivateRoute><ProfilePage /></PrivateRoute>} />

      {/* Empresa */}
      <Route path="/company/dashboard" element={
        <RoleRoute allowedRole="Company"><CompanyDashboardPage /></RoleRoute>
      } />
      <Route path="/company/new-opportunity" element={
        <RoleRoute allowedRole="Company"><NewOpportunityPage /></RoleRoute>
      } />
      <Route path="/company/opportunity/:id/candidates" element={
        <RoleRoute allowedRole="Company"><OpportunityCandidatesPage /></RoleRoute>
      } />
      <Route path="/company/setup" element={
        <RoleRoute allowedRole="Company"><CompanySetupPage /></RoleRoute>
      } />

      {/* Configurações */}
      <Route path="/settings" element={<PrivateRoute><SettingsPage /></PrivateRoute>} />

      {/* Perfil */}
      <Route path="/my-profile" element={<PrivateRoute><ProfilePage /></PrivateRoute>} />

      {/* Redirect padrão */}
      <Route path="/" element={<Navigate to="/opportunities" />} />
    </Routes>
  );
}