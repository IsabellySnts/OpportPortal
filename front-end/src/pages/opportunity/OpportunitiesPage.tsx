import { Fragment, useEffect, useMemo, useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { jobApplicationService } from '../../services/jobApplicationService';
import { curriculumService, CurriculumResponse } from '../../services/curriculumService';
import { JobApplication, JobApplicationStatus } from '../../types';
import { confirmAction, notifySuccess, notifyError } from '../../components/ui/Alerts';
import Modal from '../../components/ui/Modal';

const statusConfig: Record<number, { label: string; bg: string; color: string }> = {
  [JobApplicationStatus.Registered]:         { label: 'Candidatura enviada', bg: '#EEF2F9', color: '#1B2A4A' },
  [JobApplicationStatus.UnderReview]:        { label: 'Em análise', bg: '#FEF3E2', color: '#B07400' },
  [JobApplicationStatus.InterviewScheduled]: { label: 'Entrevista agendada', bg: '#E0F2FE', color: '#0284C7' },
  [JobApplicationStatus.FinalProcess]:       { label: 'Processo final', bg: '#F3E8FF', color: '#9333EA' },
  [JobApplicationStatus.Approved]:           { label: 'Aprovado', bg: '#DCFCE7', color: '#16A34A' },
  [JobApplicationStatus.NotAdvanced]:        { label: 'Não avançou', bg: '#FEE2E2', color: '#DC2626' },
  [JobApplicationStatus.Withdrawn]:          { label: 'Cancelada', bg: '#F3F4F6', color: '#6B7280' },
  [JobApplicationStatus.PositionClosed]:     { label: 'Vaga encerrada', bg: '#F3F4F6', color: '#6B7280' },
};

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
    { status: JobApplicationStatus.FinalProcess, label: 'Avançar p/ fase final' },
    { status: JobApplicationStatus.NotAdvanced, label: 'Recusar' },
  ],
  [JobApplicationStatus.FinalProcess]: [
    { status: JobApplicationStatus.Approved, label: 'Aprovar' },
    { status: JobApplicationStatus.NotAdvanced, label: 'Recusar' },
  ],
};

// Etapas do funil, em ordem
const STAGES = [
  { s: JobApplicationStatus.Registered, label: 'Enviada' },
  { s: JobApplicationStatus.UnderReview, label: 'Análise' },
  { s: JobApplicationStatus.InterviewScheduled, label: 'Entrevista' },
  { s: JobApplicationStatus.FinalProcess, label: 'Final' },
  { s: JobApplicationStatus.Approved, label: 'Aprovado' },
];

const REJECTED = [
  JobApplicationStatus.NotAdvanced,
  JobApplicationStatus.Withdrawn,
  JobApplicationStatus.PositionClosed,
];
const OPEN = [
  JobApplicationStatus.Registered,
  JobApplicationStatus.UnderReview,
  JobApplicationStatus.InterviewScheduled,
  JobApplicationStatus.FinalProcess,
];

// Ajuste conforme o enum EducationLevel que você usa no perfil/cadastro
const EDUCATION_LABELS: Record<number, string> = {
  1: 'Ensino Fundamental',
  2: 'Ensino Médio',
  3: 'Ensino Técnico',
  4: 'Superior',
  5: 'Pós-graduação',
  6: 'Mestrado',
  7: 'Doutorado',
};

type Filter = 'all' | 'open' | 'approved' | 'rejected';

export default function OpportunityCandidatesPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const [applications, setApplications] = useState<JobApplication[]>([]);
  const [loading, setLoading] = useState(true);
  const [updating, setUpdating] = useState<number | null>(null);
  const [filter, setFilter] = useState<Filter>('all');

  // Modal de currículo
  const [curriOpen, setCurriOpen] = useState(false);
  const [curriLoading, setCurriLoading] = useState(false);
  const [curri, setCurri] = useState<CurriculumResponse | null>(null);
  const [selectedName, setSelectedName] = useState('');

  useEffect(() => { loadCandidates(); }, [id]);

  const loadCandidates = async () => {
    setLoading(true);
    try {
      const result = await jobApplicationService.getByOpportunity(Number(id));
      if (result.isSuccess) setApplications(result.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleUpdateStatus = async (applicationId: number, newStatus: number, candidateName: string) => {
    if (newStatus === JobApplicationStatus.NotAdvanced) {
      const ok = await confirmAction({
        title: 'Recusar candidato',
        text: `Deseja recusar ${candidateName}? O candidato será notificado.`,
        confirmText: 'Sim, recusar',
        cancelText: 'Voltar',
        danger: true,
      });
      if (!ok) return;
    }

    setUpdating(applicationId);
    try {
      const result = await jobApplicationService.updateStatus(applicationId, newStatus);
      if (result.isSuccess) {
        notifySuccess('Status atualizado.');
        loadCandidates();
      } else {
        notifyError(result.message ?? 'Não foi possível atualizar o status.');
      }
    } catch {
      notifyError('Erro ao atualizar status.');
    } finally {
      setUpdating(null);
    }
  };

  const openCurriculum = async (app: JobApplication) => {
    setSelectedName(app.candidateName);
    setCurri(null);
    setCurriOpen(true);
    setCurriLoading(true);
    try {
      const r = await curriculumService.getByCandidate(app.candidateId);
      if (r.isSuccess) setCurri(r.data);
    } catch {
      // candidato ainda sem currículo
    } finally {
      setCurriLoading(false);
    }
  };

  // ---- resumo do funil ----
  const stats = useMemo(() => ({
    sent: applications.filter(a => a.status === JobApplicationStatus.Registered).length,
    review: applications.filter(a => a.status === JobApplicationStatus.UnderReview).length,
    interview: applications.filter(a =>
      a.status === JobApplicationStatus.InterviewScheduled || a.status === JobApplicationStatus.FinalProcess).length,
    approved: applications.filter(a => a.status === JobApplicationStatus.Approved).length,
    rejected: applications.filter(a => REJECTED.includes(a.status)).length,
  }), [applications]);

  const filtered = applications.filter(a =>
    filter === 'all' ? true
    : filter === 'open' ? OPEN.includes(a.status)
    : filter === 'approved' ? a.status === JobApplicationStatus.Approved
    : REJECTED.includes(a.status)
  );

  const tabBtn = (key: Filter, label: string) => (
    <button onClick={() => setFilter(key)}
      className="text-sm font-semibold px-4 py-1.5 rounded-full border transition"
      style={filter === key
        ? { backgroundColor: '#1B2A4A', color: '#fff', borderColor: '#1B2A4A' }
        : { backgroundColor: '#fff', color: '#6B7280', borderColor: '#E5E7EB' }}>
      {label}
    </button>
  );

  const getInitials = (name: string) =>
    name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase();

  const renderPipeline = (status: number) => {
    const rejected = REJECTED.includes(status);
    const curr = STAGES.findIndex(st => st.s === status);
    return (
      <div className="flex items-start mt-4" style={{ opacity: rejected ? 0.45 : 1 }}>
        {STAGES.map((st, i) => {
          const done = !rejected && curr > i;
          const isCurrent = !rejected && curr === i;
          const dotStyle = done
            ? { background: '#16A34A', borderColor: '#16A34A', color: '#fff' }
            : isCurrent
              ? { background: '#F5A623', borderColor: '#F5A623', color: '#2a1c00', boxShadow: '0 0 0 4px rgba(245,166,35,.35)' }
              : { background: '#fff', borderColor: '#E5E7EB', color: '#9CA3AF' };
          return (
            <Fragment key={st.s}>
              <div className="flex flex-col items-center gap-1.5" style={{ width: 58, textAlign: 'center' }}>
                <div className="rounded-full flex items-center justify-center text-xs font-bold"
                  style={{ width: 22, height: 22, borderWidth: 2, borderStyle: 'solid', ...dotStyle }}>
                  {done ? '✓' : i === STAGES.length - 1 ? '★' : i + 1}
                </div>
                <span style={{ fontSize: 11, lineHeight: 1.15, color: isCurrent ? '#1B2A4A' : '#9CA3AF', fontWeight: isCurrent ? 600 : 500 }}>
                  {st.label}
                </span>
              </div>
              {i < STAGES.length - 1 && (
                <div style={{ flex: 1, height: 2, marginTop: 10, background: done ? '#16A34A' : '#E5E7EB' }} />
              )}
            </Fragment>
          );
        })}
      </div>
    );
  };

  return (
    <div className="min-h-screen bg-gray-50">

      {/* Header */}
      <header className="text-white" style={{ backgroundColor: '#1B2A4A' }}>
        <div className="max-w-5xl mx-auto px-6 py-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-lg flex items-center justify-center" style={{ backgroundColor: '#F5A623' }}>
              <span className="font-bold" style={{ color: '#2a1c00' }}>P</span>
            </div>
            <span className="font-bold text-lg">Portal de Oportunidades</span>
          </div>
          <button className="hover:opacity-80" onClick={() => navigate('/company/dashboard')}>Voltar</button>
        </div>
      </header>

      {/* Conteúdo */}
      <main className="max-w-5xl mx-auto px-6 py-8">
        <button onClick={() => navigate('/company/dashboard')}
          className="text-sm text-gray-500 hover:text-gray-700 mb-3">← Voltar para minhas vagas</button>

        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>Candidatos</h1>
        <p className="text-gray-500 mb-6">{applications.length} candidato(s) para esta vaga</p>

        {loading ? (
          <p className="text-gray-500">Carregando...</p>
        ) : applications.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-xl border border-gray-100">
            <p className="text-gray-400">Nenhum candidato ainda.</p>
          </div>
        ) : (
          <>
            {/* resumo do funil */}
            <div className="grid grid-cols-3 md:grid-cols-5 gap-3 mb-6">
              {[
                { n: stats.sent, l: 'Enviadas', accent: false },
                { n: stats.review, l: 'Em análise', accent: true },
                { n: stats.interview, l: 'Entrevista', accent: false },
                { n: stats.approved, l: 'Aprovados', accent: false },
                { n: stats.rejected, l: 'Recusados', accent: false },
              ].map((t, i) => (
                <div key={i} className="bg-white rounded-2xl border border-gray-100 p-4">
                  <div className="text-2xl font-bold" style={{ color: t.accent ? '#B07400' : '#1B2A4A' }}>{t.n}</div>
                  <div className="text-xs text-gray-500 mt-1 font-medium">{t.l}</div>
                </div>
              ))}
            </div>

            {/* filtro */}
            <div className="flex gap-2 flex-wrap mb-5">
              {tabBtn('all', `Todos (${applications.length})`)}
              {tabBtn('open', 'Em aberto')}
              {tabBtn('approved', 'Aprovados')}
              {tabBtn('rejected', 'Recusados')}
            </div>

            {/* lista */}
            {filtered.length === 0 ? (
              <div className="text-center py-12 bg-white rounded-xl border border-gray-100">
                <p className="text-gray-400">Nenhum candidato neste filtro.</p>
              </div>
            ) : (
              <div className="space-y-4">
                {filtered.map((app) => {
                  const config = statusConfig[app.status];
                  const actions = availableActions[app.status] || [];
                  const rejected = REJECTED.includes(app.status);
                  return (
                    <div key={app.id} className="bg-white rounded-2xl border border-gray-100 p-5 shadow-sm">
                      <div className="flex justify-between items-start gap-3 flex-wrap">
                        <div className="flex items-center gap-3 min-w-0">
                          <div className="w-12 h-12 rounded-full flex items-center justify-center text-white font-bold flex-shrink-0"
                            style={{ backgroundColor: '#1B2A4A' }}>
                            {getInitials(app.candidateName)}
                          </div>
                          <div className="min-w-0">
                            <h3 className="font-semibold text-lg truncate" style={{ color: '#1B2A4A' }}>{app.candidateName}</h3>
                            <p className="text-sm text-gray-500">
                              Candidatura em {new Date(app.createdAt).toLocaleDateString('pt-BR')}
                            </p>
                          </div>
                        </div>
                        <span className="text-sm px-4 py-1.5 rounded-full font-semibold whitespace-nowrap"
                          style={{ backgroundColor: config.bg, color: config.color }}>
                          {config.label}
                        </span>
                      </div>

                      {renderPipeline(app.status)}

                      {rejected && app.status === JobApplicationStatus.Withdrawn && (
                        <p className="text-sm font-medium mt-3" style={{ color: '#6B7280' }}>
                          O candidato cancelou esta candidatura.
                        </p>
                      )}

                      <div className="flex gap-2 items-center flex-wrap pt-4 mt-4 border-t border-gray-100">
                        <button
                          onClick={() => openCurriculum(app)}
                          className="text-sm px-4 py-2 rounded-lg font-medium border"
                          style={{ borderColor: '#1B2A4A', color: '#1B2A4A' }}>
                          Ver currículo
                        </button>
                        {app.status === JobApplicationStatus.Approved && (
                          <span className="text-sm font-semibold" style={{ color: '#16A34A' }}>
                            Processo concluído — aprovado ✓
                          </span>
                        )}
                        {actions.map((action) => {
                          const danger = action.status === JobApplicationStatus.NotAdvanced;
                          return (
                            <button
                              key={action.status}
                              onClick={() => handleUpdateStatus(app.id, action.status, app.candidateName)}
                              disabled={updating === app.id}
                              className="text-sm px-4 py-2 rounded-lg font-medium disabled:opacity-50 transition"
                              style={danger
                                ? { border: '1px solid #FCA5A5', color: '#DC2626' }
                                : { backgroundColor: '#1B2A4A', color: 'white' }}>
                              {updating === app.id ? '...' : action.label}
                            </button>
                          );
                        })}
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </>
        )}
      </main>

      {/* Modal de currículo */}
      <Modal open={curriOpen} onClose={() => setCurriOpen(false)} title={`Currículo — ${selectedName}`}>
        {curriLoading ? (
          <p className="text-gray-500">Carregando currículo...</p>
        ) : !curri ? (
          <p className="text-gray-400">Este candidato ainda não preencheu o currículo.</p>
        ) : (
          <div className="space-y-5">
            <div>
              <div className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-1">Formação</div>
              <div className="text-gray-800">{EDUCATION_LABELS[curri.educationLevel] ?? 'Não informado'}</div>
            </div>
            <div>
              <div className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-2">Hard skills</div>
              {curri.hardSkills?.length ? (
                <div className="flex flex-wrap gap-2">
                  {curri.hardSkills.map((s, i) => (
                    <span key={i} className="text-sm px-3 py-1 rounded-lg"
                      style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>{s}</span>
                  ))}
                </div>
              ) : <span className="text-gray-400 text-sm">Nenhuma informada</span>}
            </div>
            <div>
              <div className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-2">Soft skills</div>
              {curri.softSkills?.length ? (
                <div className="flex flex-wrap gap-2">
                  {curri.softSkills.map((s, i) => (
                    <span key={i} className="text-sm px-3 py-1 rounded-lg"
                      style={{ backgroundColor: '#FEF3E2', color: '#B07400' }}>{s}</span>
                  ))}
                </div>
              ) : <span className="text-gray-400 text-sm">Nenhuma informada</span>}
            </div>
          </div>
        )}
      </Modal>
    </div>
  );
}