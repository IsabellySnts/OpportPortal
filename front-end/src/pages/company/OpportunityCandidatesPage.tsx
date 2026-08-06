import { useEffect, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
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

// Ações disponíveis por status atual
const availableActions: Record<number, { status: number; label: string }[]> = {
  [JobApplicationStatus.Registered]: [
    { status: JobApplicationStatus.UnderReview, label: 'Iniciar análise' },
    { status: JobApplicationStatus.NotAdvanced, label: 'Recusar' },
  ],
  [JobApplicationStatus.UnderReview]: [
    { status: JobApplicationStatus.InterviewScheduled, label: 'Agendar entrevista' },
    { status: JobApplicationStatus.NotAdvanced, label: 'Recusar' },
  ],
  [JobApplicationStatus.InterviewScheduled]: [
    { status: JobApplicationStatus.FinalProcess, label: 'Avançar' },
    { status: JobApplicationStatus.NotAdvanced, label: 'Recusar' },
  ],
  [JobApplicationStatus.FinalProcess]: [
    { status: JobApplicationStatus.Approved, label: 'Aprovar' },
    { status: JobApplicationStatus.NotAdvanced, label: 'Recusar' },
  ],
};

export default function OpportunityCandidatesPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const [applications, setApplications] = useState<JobApplication[]>([]);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState<number | null>(null);

  useEffect(() => {
    loadCandidates();
  }, [id]);

  const loadCandidates = async () => {
    setLoading(true);
    try {
      const result = await jobApplicationService.getByOpportunity(Number(id));
      if (result.isSuccess) {
        setApplications(result.data);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateStatus = async (applicationId: number, newStatus: number) => {
    setUpdating(applicationId);
    try {
      const result = await jobApplicationService.updateStatus(applicationId, newStatus);
      if (result.isSuccess) {
        loadCandidates();
      } else {
        alert(result.message);
      }
    } catch {
      alert('Erro ao atualizar status.');
    } finally {
      setUpdating(null);
    }
  };

  return (
    <div className="min-h-screen bg-gray-50">

      {/* Header */}
      <header className="text-white" style={{ backgroundColor: '#1B2A4A' }}>
        <div className="max-w-5xl mx-auto px-6 py-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-lg flex items-center justify-center"
              style={{ backgroundColor: '#F5A623' }}>
              <span className="font-bold">P</span>
            </div>
            <span className="font-bold text-lg">Portal de Oportunidades</span>
          </div>
          <button className="hover:opacity-80" onClick={() => navigate('/company/dashboard')}>
            Voltar
          </button>
        </div>
      </header>

      {/* Conteúdo */}
      <main className="max-w-5xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
          Candidatos
        </h1>
        <p className="text-gray-500 mb-6">
          {applications.length} candidato(s) para esta vaga
        </p>

        {loading ? (
          <p className="text-gray-500">Carregando...</p>
        ) : applications.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-xl border border-gray-100">
            <p className="text-gray-400">Nenhum candidato ainda.</p>
          </div>
        ) : (
          <div className="space-y-4">
            {applications.map((app) => {
              const config = statusConfig[app.status];
              const actions = availableActions[app.status] || [];

              return (
                <div key={app.id}
                  className="bg-white rounded-xl border border-gray-100 p-5">
                  <div className="flex justify-between items-start mb-3">
                    <div className="flex items-center gap-3">
                      <div className="w-10 h-10 rounded-full flex items-center justify-center text-white font-bold"
                        style={{ backgroundColor: '#1B2A4A' }}>
                        {app.candidateName.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase()}
                      </div>
                      <div>
                        <h3 className="font-semibold" style={{ color: '#1B2A4A' }}>
                          {app.candidateName}
                        </h3>
                        <p className="text-sm text-gray-500">
                          Candidatura em {new Date(app.createdAt).toLocaleDateString('pt-BR')}
                        </p>
                      </div>
                    </div>
                    <span className="text-sm px-4 py-1.5 rounded-full font-medium"
                      style={{ backgroundColor: config.bg, color: config.color }}>
                      {config.label}
                    </span>
                  </div>

                  {actions.length > 0 && (
                    <div className="flex gap-2 pt-3 border-t border-gray-100">
                      {actions.map((action) => (
                        <button
                          key={action.status}
                          onClick={() => handleUpdateStatus(app.id, action.status)}
                          disabled={updating === app.id}
                          className="text-sm px-4 py-2 rounded-lg font-medium disabled:opacity-50"
                          style={action.status === JobApplicationStatus.NotAdvanced
                            ? { border: '1px solid #DC2626', color: '#DC2626' }
                            : { backgroundColor: '#1B2A4A', color: 'white' }}
                        >
                          {action.label}
                        </button>
                      ))}
                    </div>
                  )}
                </div>
              );
            })}
          </div>
        )}
      </main>
    </div>
  );
}