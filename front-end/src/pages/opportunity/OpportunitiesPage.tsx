import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { opportunityService } from '../../services/opportunityService';
import { jobApplicationService } from '../../services/jobApplicationService';
import { Opportunity, OpportunityType, JobFormat } from '../../types';
import Header from '../../components/layout/Header';
import { candidateService } from '../../services/candidateService';

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

const tabs = [
  { label: 'Todos', value: null },
  { label: 'Estágios', value: OpportunityType.Internship },
  { label: 'Freelance', value: OpportunityType.Freelance },
  { label: 'CLT', value: OpportunityType.CLT },
  { label: 'PJ', value: OpportunityType.PJ },
];

function getInitials(name: string) {
  return name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase();
}

export default function OpportunitiesPage() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const [opportunities, setOpportunities] = useState<Opportunity[]>([]);
  const [loading, setLoading] = useState(true);
  const [search, setSearch] = useState('');
  const [activeTab, setActiveTab] = useState<OpportunityType | null>(null);
  const [applying, setApplying] = useState<number | null>(null);

  useEffect(() => {
    loadOpportunities();
    loadMyApplications();
  }, []);

  const [appliedIds, setAppliedIds] = useState<number[]>([]);

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

  const loadMyApplications = async () => {
    try {
      const candidate = await candidateService.getMe();
      if (candidate.isSuccess) {
        const apps = await jobApplicationService.getByCandidate(candidate.data.id);
        if (apps.isSuccess) {
          setAppliedIds(apps.data.map(a => a.opportunityId));
        }
      }
    } catch (err) {
      console.error(err);
    }
  };

  const handleApply = async (opportunityId: number) => {
    setApplying(opportunityId);
    try {
      const result = await jobApplicationService.create(opportunityId);
      if (result.isSuccess) {
        alert('Candidatura realizada com sucesso!');
        setAppliedIds([...appliedIds, opportunityId]);
      } else {
        alert(result.message);
      }
    } catch {
      alert('Erro ao candidatar. Tente novamente.');
    } finally {
      setApplying(null);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  const filtered = opportunities
    .filter(o => activeTab === null || o.type === activeTab)
    .filter(o =>
      o.title.toLowerCase().includes(search.toLowerCase()) ||
      o.companyName.toLowerCase().includes(search.toLowerCase()) ||
      o.fieldOfWork.toLowerCase().includes(search.toLowerCase())
    );

  return (
    <div className="min-h-screen bg-gray-50">

      <Header
        navItems={[
          { label: 'Oportunidades', path: '/opportunities' },
          { label: 'Candidaturas', path: '/my-applications' },
          { label: 'Meu Perfil', path: '/my-profile' },
        ]}
      />

      <div className="max-w-7xl mx-auto px-6 py-8 flex gap-8">

        {/* Sidebar */}
        <aside className="w-56 flex-shrink-0">
          <p className="text-xs font-semibold text-gray-400 mb-3 uppercase">Navegação</p>
          <nav className="space-y-1 mb-8">
            <button className="w-full text-left px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-100">
              Início
            </button>
            <button className="w-full text-left px-4 py-2 rounded-lg font-medium flex justify-between items-center"
              style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>
              Oportunidades
              <span className="text-xs px-2 py-0.5 rounded-full text-white"
                style={{ backgroundColor: '#F5A623' }}>
                {opportunities.length}
              </span>
            </button>
            <button className="w-full text-left px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-100"
              onClick={() => navigate('/my-applications')}>
              Candidaturas
            </button>
          </nav>

          <p className="text-xs font-semibold text-gray-400 mb-3 uppercase">Conta</p>
          <nav className="space-y-1">
            <button className="w-full text-left px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-100" onClick={() => navigate('/my-profile')}>
              Meu Perfil
            </button>
            <button className="w-full text-left px-4 py-2 rounded-lg text-gray-600 hover:bg-gray-100" onClick={() => navigate('/settings')}>
              Configurações
            </button>
          </nav>
        </aside>

        {/* Conteúdo principal */}
        <main className="flex-1">
          <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
            Oportunidades
          </h1>
          <p className="text-gray-500 mb-6">
            Encontre estágios, projetos e freelances para sua carreira
          </p>

          {/* Busca */}
          <input
            type="text"
            value={search}
            onChange={(e) => setSearch(e.target.value)}
            placeholder="Buscar por cargo, área, empresa..."
            className="w-full border border-gray-200 rounded-xl px-4 py-3 mb-4 focus:outline-none focus:ring-2 bg-white"
          />

          {/* Abas */}
          <div className="flex gap-2 mb-6">
            {tabs.map((tab) => (
              <button
                key={tab.label}
                onClick={() => setActiveTab(tab.value)}
                className="px-4 py-2 rounded-lg text-sm font-medium transition"
                style={activeTab === tab.value
                  ? { backgroundColor: '#1B2A4A', color: 'white' }
                  : { backgroundColor: 'white', color: '#6B7280', border: '1px solid #E5E7EB' }}
              >
                {tab.label}
              </button>
            ))}
          </div>

          {/* Lista */}
          {loading ? (
            <p className="text-gray-500">Carregando...</p>
          ) : filtered.length === 0 ? (
            <div className="text-center py-16">
              <p className="text-gray-400">Nenhuma oportunidade encontrada.</p>
            </div>
          ) : (
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-4">
              {filtered.map((opp) => (
                <div
                  key={opp.id}
                  className="bg-white rounded-xl border border-gray-100 p-5 hover:shadow-md transition"
                >
                  <div className="flex items-start gap-3 mb-3">
                    <div className="w-10 h-10 rounded-lg flex items-center justify-center flex-shrink-0 text-white text-sm font-bold"
                      style={{ backgroundColor: '#1B2A4A' }}>
                      {getInitials(opp.companyName)}
                    </div>
                    <div className="flex-1">
                      <div className="flex justify-between items-start">
                        <div>
                          <h3 className="font-semibold" style={{ color: '#1B2A4A' }}>
                            {opp.title}
                          </h3>
                          <p className="text-xs text-gray-500">
                            {opp.companyName} · {opp.state}
                          </p>
                        </div>
                        <span className="text-xs px-3 py-1 rounded-full font-medium"
                          style={{ backgroundColor: '#FEF3E2', color: '#F5A623' }}>
                          {typeLabels[opp.type]}
                        </span>
                      </div>
                    </div>
                  </div>

                  <div className="flex justify-between items-center">
                    <span className="text-sm text-gray-500">
                      {opp.salary > 0
                        ? `R$ ${opp.salary.toLocaleString('pt-BR')}`
                        : 'A combinar'} · {formatLabels[opp.format]}
                    </span>
                    {(() => {
                      const alreadyApplied = appliedIds.includes(opp.id);
                      return (
                        <button
                          onClick={() => handleApply(opp.id)}
                          disabled={applying === opp.id || alreadyApplied}
                          className="px-4 py-2 rounded-lg text-sm font-medium disabled:opacity-60"
                          style={alreadyApplied
                            ? { backgroundColor: '#DCFCE7', color: '#16A34A', cursor: 'default' }
                            : { backgroundColor: '#1B2A4A', color: 'white' }}
                        >
                          {applying === opp.id
                            ? 'Enviando...'
                            : alreadyApplied
                              ? '✓ Candidatado'
                              : 'Candidatar'}
                        </button>
                      );
                    })()}
                  </div>
                </div>
              ))}
            </div>
          )}
        </main>
      </div>
    </div>
  );
}