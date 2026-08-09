import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { companyService } from '../../services/companyService';
import { opportunityService } from '../../services/opportunityService';
import { Opportunity, OpportunityType } from '../../types';
import Header from '../../components/layout/Header';

const typeLabels: Record<number, string> = {
  [OpportunityType.Internship]: 'Estágio',
  [OpportunityType.Freelance]: 'Freelance',
  [OpportunityType.CLT]: 'CLT',
  [OpportunityType.PJ]: 'PJ',
};

export default function CompanyDashboardPage() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const [opportunities, setOpportunities] = useState<Opportunity[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadOpportunities();
  }, []);

  const loadOpportunities = async () => {
    setLoading(true);
    try {
      const companyResult = await companyService.getMe();
      if (!companyResult.isSuccess) {
        setLoading(false);
        return;
      }

      const oppsResult = await opportunityService.getByCompany(companyResult.data.id);
      if (oppsResult.isSuccess) {
        setOpportunities(oppsResult.data);
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = async (id: number) => {
    if (!window.confirm('Deseja encerrar esta vaga?')) return;

    const result = await opportunityService.close(id);
    if (result.isSuccess) {
      loadOpportunities();
    } else {
      alert(result.message);
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-gray-50">

      {/* Header */}
      <Header
        navItems={[
          { label: 'Minhas vagas', path: '/company/dashboard' },
          { label: 'Meu Perfil', path: '/my-profile' },
        ]}
      />

      {/* Conteúdo */}
      <main className="max-w-5xl mx-auto px-6 py-8">
        <div className="flex justify-between items-center mb-6">
          <div>
            <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
              Minhas Vagas
            </h1>
            <p className="text-gray-500">Gerencie suas oportunidades publicadas</p>
          </div>
          <button
            onClick={() => navigate('/company/new-opportunity')}
            className="px-5 py-2.5 rounded-lg text-white font-medium"
            style={{ backgroundColor: '#F5A623' }}>
            + Nova vaga
          </button>
        </div>

        {loading ? (
          <p className="text-gray-500">Carregando...</p>
        ) : opportunities.length === 0 ? (
          <div className="text-center py-16 bg-white rounded-xl border border-gray-100">
            <p className="text-gray-400 mb-4">Você ainda não publicou nenhuma vaga.</p>
            <button
              onClick={() => navigate('/company/new-opportunity')}
              className="px-6 py-2 rounded-lg text-white font-medium"
              style={{ backgroundColor: '#1B2A4A' }}>
              Publicar primeira vaga
            </button>
          </div>
        ) : (
          <div className="space-y-4">
            {opportunities.map((opp) => (
              <div key={opp.id}
                className="bg-white rounded-xl border border-gray-100 p-5">
                <div className="flex justify-between items-start mb-3">
                  <div>
                    <div className="flex items-center gap-2 mb-1">
                      <h3 className="font-semibold" style={{ color: '#1B2A4A' }}>
                        {opp.title}
                      </h3>
                      <span className="text-xs px-2 py-0.5 rounded-full font-medium"
                        style={opp.status === 1
                          ? { backgroundColor: '#DCFCE7', color: '#16A34A' }
                          : { backgroundColor: '#F3F4F6', color: '#6B7280' }}>
                        {opp.status === 1 ? 'Ativa' : 'Encerrada'}
                      </span>
                    </div>
                    <p className="text-sm text-gray-500">
                      {typeLabels[opp.type]} · {opp.fieldOfWork} · {opp.state}
                    </p>
                  </div>
                  <span className="font-semibold" style={{ color: '#1B2A4A' }}>
                    {opp.salary > 0 ? `R$ ${opp.salary.toLocaleString('pt-BR')}` : 'A combinar'}
                  </span>
                </div>

                <div className="flex gap-3 pt-3 border-t border-gray-100">
                  <button
                    onClick={() => navigate(`/company/opportunity/${opp.id}/candidates`)}
                    className="text-sm px-4 py-2 rounded-lg font-medium text-white"
                    style={{ backgroundColor: '#1B2A4A' }}>
                    Ver candidatos
                  </button>
                  {opp.status === 1 && (
                    <button
                      onClick={() => handleClose(opp.id)}
                      className="text-sm px-4 py-2 rounded-lg font-medium border"
                      style={{ borderColor: '#DC2626', color: '#DC2626' }}>
                      Encerrar vaga
                    </button>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </main>
    </div>
  );
}