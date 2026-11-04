import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { companyService } from '../../services/companyService';
import { opportunityService } from '../../services/opportunityService';
import { jobApplicationService } from '../../services/jobApplicationService';
import { Opportunity, OpportunityType, JobFormat, JobApplication, JobApplicationStatus } from '../../types';
import Header from '../../components/layout/Header';
import { confirmAction, notifyError, notifySuccess } from '../../components/ui/Alerts';

const typeLabels: Record<number, string> = {
  [OpportunityType.Internship]: 'Estágio',
  [OpportunityType.Freelance]: 'Freelance',
  [OpportunityType.CLT]: 'CLT',
  [OpportunityType.PJ]: 'PJ',
};
const formatLabels: Record<number, string> = {
  [JobFormat.OnSite]: 'Presencial',
  [JobFormat.Remote]: 'Remoto',
  [JobFormat.Hybrid]: 'Híbrido',
};

const formatSalary = (v: number) =>
  v > 0 ? `R$ ${Number(v).toLocaleString('pt-BR', { maximumFractionDigits: 0 })}` : 'A combinar';

type Filter = 'all' | 'active' | 'closed';

export default function CompanyDashboardPage() {
  const navigate = useNavigate();
  const [opportunities, setOpportunities] = useState<Opportunity[]>([]);
  const [appsByOpp, setAppsByOpp] = useState<Record<number, JobApplication[]>>({});
  const [loading, setLoading] = useState(true);
  const [filter, setFilter] = useState<Filter>('all');

  useEffect(() => { loadData(); }, []);

  const loadData = async () => {
    setLoading(true);
    try {
      const companyResult = await companyService.getMe();
      if (!companyResult.isSuccess) { setLoading(false); return; }

      const oppsResult = await opportunityService.getByCompany(companyResult.data.id);
      if (!oppsResult.isSuccess) { setLoading(false); return; }

      const opps = oppsResult.data;
      setOpportunities(opps);

      // Carrega as candidaturas de cada vaga (pro resumo e o funil)
      const map: Record<number, JobApplication[]> = {};
      await Promise.all(opps.map(async (o) => {
        try {
          const r = await jobApplicationService.getByOpportunity(o.id);
          if (r.isSuccess) map[o.id] = r.data;
        } catch { /* ignora */ }
      }));
      setAppsByOpp(map);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = async (id: number) => {
    const ok = await confirmAction({
      title: 'Encerrar vaga',
      text: 'Tem certeza que deseja encerrar esta vaga?',
      confirmText: 'Sim, encerrar',
      cancelText: 'Voltar',
      danger: true,
    });
    if (!ok) return;

    const result = await opportunityService.close(id);
    if (result.isSuccess) {
      notifySuccess('Vaga encerrada.');
      loadData();
    } else {
      notifyError(result.message ?? 'Não foi possível encerrar a vaga.');
    }
  };

  // ---- estatísticas (ignorando candidaturas canceladas) ----
  const active = (a: JobApplication[]) => a.filter((x) => x.status !== JobApplicationStatus.Withdrawn);
  const allApps = active(Object.values(appsByOpp).flat());
  const inProcessSet = [JobApplicationStatus.UnderReview, JobApplicationStatus.InterviewScheduled, JobApplicationStatus.FinalProcess];

  const stats = {
    activeVagas: opportunities.filter((o) => o.status === 1).length,
    candidates: allApps.length,
    inProcess: allApps.filter((a) => inProcessSet.includes(a.status)).length,
    approved: allApps.filter((a) => a.status === JobApplicationStatus.Approved).length,
  };

  const vagaStats = (oppId: number) => {
    const apps = active(appsByOpp[oppId] || []);
    return {
      total: apps.length,
      review: apps.filter((a) => a.status === JobApplicationStatus.UnderReview).length,
      interview: apps.filter((a) => a.status === JobApplicationStatus.InterviewScheduled || a.status === JobApplicationStatus.FinalProcess).length,
      approved: apps.filter((a) => a.status === JobApplicationStatus.Approved).length,
    };
  };

  const filtered = opportunities.filter((o) =>
    filter === 'all' ? true : filter === 'active' ? o.status === 1 : o.status !== 1
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

  return (
    <div className="min-h-screen bg-gray-50">
      <Header navItems={[
        { label: 'Minhas vagas', path: '/company/dashboard' },
        { label: 'Meu Perfil', path: '/my-profile' },
      ]} />

      <main className="max-w-5xl mx-auto px-6 py-8">
        {/* Topo */}
        <div className="flex justify-between items-end gap-4 flex-wrap mb-6">
          <div>
            <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>Minhas Vagas</h1>
            <p className="text-gray-500">Gerencie suas oportunidades publicadas</p>
          </div>
          <button onClick={() => navigate('/company/new-opportunity')}
            className="px-5 py-2.5 rounded-lg text-white font-medium"
            style={{ backgroundColor: '#F5A623', color: '#2a1c00' }}>
            + Nova vaga
          </button>
        </div>

        {loading ? (
          <p className="text-gray-500">Carregando...</p>
        ) : (
          <>
            {/* Resumo */}
            <div className="grid grid-cols-2 md:grid-cols-4 gap-3 mb-6">
              {[
                { n: stats.activeVagas, l: 'Vagas ativas', accent: false },
                { n: stats.candidates, l: 'Candidatos no total', accent: true },
                { n: stats.inProcess, l: 'Em processo', accent: false },
                { n: stats.approved, l: 'Aprovados', accent: false },
              ].map((t, i) => (
                <div key={i} className="bg-white rounded-2xl border border-gray-100 p-4">
                  <div className="text-3xl font-bold" style={{ color: t.accent ? '#946008' : '#1B2A4A' }}>{t.n}</div>
                  <div className="text-xs text-gray-500 mt-1 font-medium">{t.l}</div>
                </div>
              ))}
            </div>

            {/* Filtro */}
            <div className="flex gap-2 flex-wrap mb-5">
              {tabBtn('all', `Todas (${opportunities.length})`)}
              {tabBtn('active', 'Ativas')}
              {tabBtn('closed', 'Encerradas')}
            </div>

            {/* Lista */}
            {filtered.length === 0 ? (
              <div className="text-center py-16 bg-white rounded-xl border border-gray-100">
                <p className="text-gray-400 mb-4">
                  {opportunities.length === 0 ? 'Você ainda não publicou nenhuma vaga.' : 'Nenhuma vaga neste filtro.'}
                </p>
                {opportunities.length === 0 && (
                  <button onClick={() => navigate('/company/new-opportunity')}
                    className="px-6 py-2 rounded-lg text-white font-medium" style={{ backgroundColor: '#1B2A4A' }}>
                    Publicar primeira vaga
                  </button>
                )}
              </div>
            ) : (
              <div className="space-y-4">
                {filtered.map((opp) => {
                  const vs = vagaStats(opp.id);
                  const activeVaga = opp.status === 1;
                  return (
                    <div key={opp.id} className="bg-white rounded-2xl border border-gray-100 p-5 shadow-sm">
                      <div className="flex justify-between items-start gap-4 flex-wrap">
                        <div className="min-w-0">
                          <div className="flex items-center gap-2 flex-wrap mb-2">
                            <h3 className="font-semibold text-lg" style={{ color: '#1B2A4A' }}>{opp.title}</h3>
                            <span className="text-xs px-2.5 py-0.5 rounded-full font-bold uppercase tracking-wide"
                              style={activeVaga ? { backgroundColor: '#E7F6EC', color: '#16A34A' } : { backgroundColor: '#F1F1EE', color: '#6B7280' }}>
                              {activeVaga ? 'Ativa' : 'Encerrada'}
                            </span>
                          </div>
                          <div className="flex gap-2 flex-wrap">
                            {[typeLabels[opp.type], opp.fieldOfWork, opp.state, formatLabels[opp.format]].filter(Boolean).map((m, i) => (
                              <span key={i} className="text-sm px-2.5 py-1 rounded-lg border border-gray-100 bg-gray-50 text-gray-600">{m}</span>
                            ))}
                          </div>
                        </div>
                        <div className="text-right">
                          <div className="font-bold text-lg whitespace-nowrap" style={{ color: '#1B2A4A' }}>{formatSalary(opp.salary)}</div>
                          {opp.salary > 0 && <div className="text-xs text-gray-400">por mês</div>}
                        </div>
                      </div>

                      {/* Funil de candidatos */}
                      <div className="flex items-center gap-4 flex-wrap mt-4">
                        <div className="flex items-baseline gap-1.5">
                          <span className="font-bold text-xl" style={{ color: '#1B2A4A' }}>{vs.total}</span>
                          <span className="text-sm text-gray-500">{vs.total === 1 ? 'candidato' : 'candidatos'}</span>
                        </div>
                        <div className="flex gap-1.5 flex-wrap">
                          {vs.review > 0 && <span className="text-xs font-semibold px-2.5 py-0.5 rounded-full" style={{ backgroundColor: 'rgba(2,132,199,.12)', color: '#0284C7' }}>{vs.review} em análise</span>}
                          {vs.interview > 0 && <span className="text-xs font-semibold px-2.5 py-0.5 rounded-full" style={{ backgroundColor: 'rgba(245,166,35,.14)', color: '#946008' }}>{vs.interview} entrevista</span>}
                          {vs.approved > 0 && <span className="text-xs font-semibold px-2.5 py-0.5 rounded-full" style={{ backgroundColor: '#E7F6EC', color: '#16A34A' }}>{vs.approved} aprovado{vs.approved > 1 ? 's' : ''}</span>}
                        </div>
                      </div>

                      <div className="flex gap-3 flex-wrap pt-4 mt-4 border-t border-gray-100">
                        <button onClick={() => navigate(`/company/opportunity/${opp.id}/candidates`)}
                          className="text-sm px-4 py-2 rounded-lg font-medium text-white" style={{ backgroundColor: '#1B2A4A' }}>
                          Ver candidatos{vs.total > 0 ? ` (${vs.total})` : ''}
                        </button>
                        {activeVaga && (
                          <button onClick={() => handleClose(opp.id)}
                            className="text-sm px-4 py-2 rounded-lg font-medium border"
                            style={{ borderColor: '#FCA5A5', color: '#DC2626' }}>
                            Encerrar vaga
                          </button>
                        )}
                      </div>
                    </div>
                  );
                })}
              </div>
            )}
          </>
        )}
      </main>
    </div>
  );
}