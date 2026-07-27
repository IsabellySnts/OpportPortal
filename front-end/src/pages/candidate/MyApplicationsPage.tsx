import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { candidateService } from '../../services/candidateService';
import { jobApplicationService } from '../../services/jobApplicationService';
import { JobApplication, JobApplicationStatus } from '../../types';

const statusConfig: Record<number, { label: string; bg: string; color: string }> = {
  [JobApplicationStatus.Registered]:         { label: 'Candidatura enviada', bg: '#EEF2F9', color: '#1B2A4A' },
  [JobApplicationStatus.UnderReview]:        { label: 'Em análise', bg: '#FEF3E2', color: '#F5A623' },
  [JobApplicationStatus.InterviewScheduled]: { label: 'Entrevista agendada', bg: '#E0F2FE', color: '#0284C7' },
  [JobApplicationStatus.FinalProcess]:       { label: 'Processo final', bg: '#F3E8FF', color: '#9333EA' },
  [JobApplicationStatus.Approved]:           { label: 'Aprovado', bg: '#DCFCE7', color: '#16A34A' },
  [JobApplicationStatus.NotAdvanced]:        { label: 'Não avançou', bg: '#FEE2E2', color: '#DC2626' },
  [JobApplicationStatus.Withdrawn]:          { label: 'Cancelada', bg: '#F3F4F6', color: '#6B7280' },
  [JobApplicationStatus.PositionClosed]:     { label: 'Vaga encerrada', bg: '#F3F4F6', color: '#6B7280' },
};

export default function MyApplicationsPage() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const [applications, setApplications] = useState<JobApplication[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadApplications();
  }, []);

  const loadApplications = async () => {
    setLoading(true);
    try {
      const candidateResult = await candidateService.getMe();
      if (!candidateResult.isSuccess) {
        setLoading(false);
        return;
      }

      const appsResult = await jobApplicationService.getByCandidate(candidateResult.data.id);
      if (appsResult.isSuccess) {
        setApplications(appsResult.data);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-gray-50">

      {/* Header */}
      <header className="text-white" style={{ backgroundColor: '#1B2A4A' }}>
        <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-lg flex items-center justify-center"
              style={{ backgroundColor: '#F5A623' }}>
              <span className="font-bold">P</span>
            </div>
            <span className="font-bold text-lg">Portal de Oportunidades</span>
          </div>

          <nav className="flex items-center gap-8">
            <button className="hover:opacity-80" onClick={() => navigate('/opportunities')}>
              Oportunidades
            </button>
            <button className="font-semibold" style={{ color: '#F5A623' }}>
              Candidaturas
            </button>
            <button className="hover:opacity-80">Meu Perfil</button>
            <div className="w-9 h-9 rounded-full flex items-center justify-center cursor-pointer"
              style={{ backgroundColor: '#F5A623' }}
              onClick={handleLogout} title="Sair">
              <span className="font-bold text-sm">IS</span>
            </div>
          </nav>
        </div>
      </header>

      {/* Conteúdo */}
      <main className="max-w-4xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
          Minhas Candidaturas
        </h1>
        <p className="text-gray-500 mb-6">
          Acompanhe o status das suas candidaturas
        </p>

        {loading ? (
          <p className="text-gray-500">Carregando...</p>
        ) : applications.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-xl border border-gray-100">
            <p className="text-gray-400 mb-4">Você ainda não se candidatou a nenhuma vaga.</p>
            <button
              onClick={() => navigate('/opportunities')}
              className="px-6 py-2 rounded-lg text-white font-medium"
              style={{ backgroundColor: '#1B2A4A' }}>
              Ver oportunidades
            </button>
          </div>
        ) : (
          <div className="space-y-4">
            {applications.map((app) => {
              const config = statusConfig[app.status];
              return (
                <div key={app.id}
                  className="bg-white rounded-xl border border-gray-100 p-5 flex justify-between items-center">
                  <div>
                    <h3 className="font-semibold" style={{ color: '#1B2A4A' }}>
                      {app.opportunityTitle}
                    </h3>
                    <p className="text-sm text-gray-500">
                      Candidatura em {new Date(app.createdAt).toLocaleDateString('pt-BR')}
                    </p>
                  </div>
                  <span className="text-sm px-4 py-1.5 rounded-full font-medium"
                    style={{ backgroundColor: config.bg, color: config.color }}>
                    {config.label}
                  </span>
                </div>
              );
            })}
          </div>
        )}
      </main>
    </div>
  );
}