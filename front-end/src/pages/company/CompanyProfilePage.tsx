import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { companyService, CompanyResponse } from '../../services/companyService';
import { formatCnpj, formatPhone } from '../../utils/Masks';
import Header from '../../components/layout/Header';

const getInitials = (name: string) =>
  (name || '?').trim().split(/\s+/).map(w => w[0]).slice(0, 2).join('').toUpperCase();

export default function CompanyProfilePage() {
  const navigate = useNavigate();
  const [company, setCompany] = useState<CompanyResponse | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => { loadProfile(); }, []);

  const loadProfile = async () => {
    setLoading(true);
    try {
      const result = await companyService.getMe();
      if (result.isSuccess) setCompany(result.data);
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const Field = ({ label, value }: { label: string; value?: string | null }) => (
    <div>
      <div className="text-xs font-semibold tracking-wide uppercase text-gray-400 mb-1">{label}</div>
      {value
        ? <div className="text-gray-800">{value}</div>
        : <div className="text-gray-400 italic">Não informado</div>}
    </div>
  );

  const Card = ({ icon, title, children }: { icon: string; title: string; children: React.ReactNode }) => (
    <div className="bg-white rounded-2xl border border-gray-100 shadow-sm p-6">
      <div className="flex items-center gap-2.5 mb-5">
        <div className="w-8 h-8 rounded-lg flex items-center justify-center text-base"
          style={{ backgroundColor: '#FEF3E2' }}>{icon}</div>
        <h2 className="font-semibold text-lg" style={{ color: '#1B2A4A' }}>{title}</h2>
      </div>
      {children}
    </div>
  );

  return (
    <div className="min-h-screen bg-gray-50">
      <Header navItems={[
        { label: 'Minhas vagas', path: '/company/dashboard' },
        { label: 'Meu Perfil', path: '/my-profile' },
      ]} />

      <main className="max-w-3xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>Meu Perfil</h1>
        <p className="text-gray-500 mb-6">Dados da sua empresa</p>

        {loading ? (
          <p className="text-gray-500">Carregando...</p>
        ) : !company ? (
          <div className="text-center py-16 bg-white rounded-xl border border-gray-100">
            <p className="text-gray-400">Não foi possível carregar o perfil.</p>
          </div>
        ) : (
          <div className="space-y-5">

            {/* Hero */}
            <div className="rounded-2xl p-6 flex items-center gap-5 flex-wrap"
              style={{ background: 'linear-gradient(135deg, #1B2A4A 0%, #22365e 100%)' }}>
              <div className="w-16 h-16 rounded-full flex items-center justify-center font-bold text-xl flex-shrink-0"
                style={{ backgroundColor: '#F5A623', color: '#2a1c00' }}>
                {getInitials(company.companyName)}
              </div>
              <div className="flex-1 min-w-0">
                <h2 className="text-white font-bold text-xl truncate">{company.companyName}</h2>
                <div className="flex items-center gap-2 mt-1 flex-wrap">
                  <span className="text-xs font-bold px-2.5 py-0.5 rounded-full"
                    style={{ backgroundColor: 'rgba(245,166,35,.2)', color: '#F5A623' }}>EMPRESA</span>
                  <span className="text-sm" style={{ color: '#A9B6CE' }}>
                    {company.sector || 'Ramo não informado'}
                  </span>
                </div>
              </div>
              <button
                onClick={() => navigate('/company/setup')}
                className="px-5 py-2.5 rounded-lg font-medium flex-shrink-0"
                style={{ backgroundColor: '#F5A623', color: '#2a1c00' }}>
                Editar dados
              </button>
            </div>

            {/* Dados da empresa */}
            <Card icon="🏢" title="Dados da empresa">
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-8 gap-y-5">
                <Field label="Razão social / Nome" value={company.companyName} />
                <Field label="CNPJ" value={company.cnpj ? formatCnpj(company.cnpj) : undefined} />
                <Field label="Ramo de atuação" value={company.sector} />
                <Field label="E-mail" value={company.email} />
                <Field label="Telefone" value={company.phoneNumber ? formatPhone(company.phoneNumber) : undefined} />
              </div>
            </Card>

            {/* Localização */}
            <Card icon="📍" title="Localização">
              <div className="grid grid-cols-1 sm:grid-cols-2 gap-x-8 gap-y-5">
                <Field label="Cidade" value={company.city} />
                <Field label="Estado (UF)" value={company.state} />
              </div>
            </Card>

          </div>
        )}
      </main>
    </div>
  );
}
