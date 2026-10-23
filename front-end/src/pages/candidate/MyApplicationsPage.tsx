import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { candidateService } from '../../services/candidateService';
import { jobApplicationService } from '../../services/jobApplicationService';
import { JobApplication, JobApplicationStatus, Opportunity } from '../../types';
import Header from '../../components/layout/Header';
import Sidebar from '../../components/layout/Sidebar';
import { opportunityService } from '../../services/opportunityService';
import { confirmAction, notifySuccess, notifyError } from '../../components/ui/Alerts';


const statusConfig: Record<number, { label: string; bg: string; color: string }> = {
  [JobApplicationStatus.Registered]: { label: 'Candidatura enviada', bg: '#EEF2F9', color: '#1B2A4A' },
  [JobApplicationStatus.UnderReview]: { label: 'Em análise', bg: '#FEF3E2', color: '#F5A623' },
  [JobApplicationStatus.InterviewScheduled]: { label: 'Entrevista agendada', bg: '#E0F2FE', color: '#0284C7' },
  [JobApplicationStatus.FinalProcess]: { label: 'Processo final', bg: '#F3E8FF', color: '#9333EA' },
  [JobApplicationStatus.Approved]: { label: 'Aprovado', bg: '#DCFCE7', color: '#16A34A' },
  [JobApplicationStatus.NotAdvanced]: { label: 'Não avançou', bg: '#FEE2E2', color: '#DC2626' },
  [JobApplicationStatus.Withdrawn]: { label: 'Cancelada', bg: '#F3F4F6', color: '#6B7280' },
  [JobApplicationStatus.PositionClosed]: { label: 'Vaga encerrada', bg: '#F3F4F6', color: '#6B7280' },
};

export default function MyApplicationsPage() {
  const navigate = useNavigate();

  const [applications, setApplications] = useState<JobApplication[]>([]);
  const [opportunities, setOpportunities] = useState<Opportunity[]>([]);

  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadApplications();
    loadOpportunities();
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

  const loadOpportunities = async () => {
    setLoading(true);
    try {
      const result = await opportunityService.getAll();
      if (result.isSuccess) setOpportunities(result.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const cancelableStatuses = [
    JobApplicationStatus.Registered,
    JobApplicationStatus.UnderReview,
    JobApplicationStatus.InterviewScheduled,
    JobApplicationStatus.FinalProcess,
  ];

  const handleCancel = async (id: number) => {
    const confirmed = await confirmAction({
      title: 'Cancelar Candidatura',
      text: 'Tem certeza que deseja cancelar esta candidatura?',
      confirmText: 'Cancelar',
      cancelText: 'Voltar',
      danger: true
    });
    if (!confirmed) return;

    const result = await jobApplicationService.cancel(id);
    if (result.isSuccess) {
      await loadApplications();
      notifySuccess('Candidatura cancelada com sucesso.');
    } else {
      notifyError(result.message ?? 'Não foi possível cancelar a candidatura.');
    }
  };

  const handleReapply = async (opportunityId: number) => {
    const result = await jobApplicationService.create(opportunityId);
    if (result.isSuccess) {
      await loadApplications();
      notifySuccess('Candidatura reenviada com sucesso!');
    } else {
      notifyError(result.message ?? 'Não foi possível reenviar a candidatura.');
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">

      <Header
        navItems={[
          { label: 'Oportunidades', path: '/opportunities' },
          { label: 'Candidaturas', path: '/my-applications' }
        ]}
      />

      <div className="max-w-7xl mx-auto px-6 py-8 flex gap-8">

        <Sidebar
          sections={[
            {
              title: 'Navegação',
              items: [
                { label: 'Oportunidades', path: '/opportunities', badge: opportunities.length },
                { label: 'Candidaturas', path: '/my-applications', badge: applications.length },
              ],
            },
            {
              title: 'Conta',
              items: [
                { label: 'Meu Perfil', path: '/my-profile' },
                { label: 'Configurações', path: '/settings' },
              ],
            },
          ]}
        />

        <main className="flex-1">
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

                    <div className="flex items-center gap-4">
                      <span className="text-sm px-4 py-1.5 rounded-full font-medium"
                        style={{ backgroundColor: config.bg, color: config.color }}>
                        {config.label}
                      </span>

                      {cancelableStatuses.includes(app.status) && (
                        <button
                          onClick={() => handleCancel(app.id)}
                          className="text-sm px-3 py-1.5 rounded-lg font-medium border transition hover:bg-red-50"
                          style={{ color: '#DC2626', borderColor: '#FECACA' }}
                        >
                          Cancelar
                        </button>
                      )}

                      {app.status === JobApplicationStatus.Withdrawn && (
                        <button
                          onClick={() => handleReapply(app.opportunityId)}
                          className="text-sm px-3 py-1.5 rounded-lg font-medium text-white transition hover:opacity-90"
                          style={{ backgroundColor: '#1B2A4A' }}
                        >
                          Candidatar novamente
                        </button>
                      )}
                    </div>
                  </div>
                );
              })}
            </div>
          )}
        </main>
      </div>
    </div>
  );
}