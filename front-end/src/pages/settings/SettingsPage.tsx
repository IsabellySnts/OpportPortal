import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';

export default function SettingsPage() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <div className="min-h-screen bg-gray-50">

      {/* Header */}
      <header className="text-white" style={{ backgroundColor: '#1B2A4A' }}>
        <div className="max-w-4xl mx-auto px-6 py-4 flex items-center justify-between">
          <div className="flex items-center gap-3">
            <div className="w-9 h-9 rounded-lg flex items-center justify-center"
              style={{ backgroundColor: '#F5A623' }}>
              <span className="font-bold">P</span>
            </div>
            <span className="font-bold text-lg">Portal de Oportunidades</span>
          </div>
          <button className="hover:opacity-80" onClick={() => navigate('/opportunities')}>
            Voltar
          </button>
        </div>
      </header>

      {/* Conteúdo */}
      <main className="max-w-2xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
          Configurações
        </h1>
        <p className="text-gray-500 mb-6">Gerencie sua conta</p>

        <div className="bg-white rounded-2xl shadow-sm p-8">
          <h2 className="font-semibold mb-2" style={{ color: '#1B2A4A' }}>
            Sair da conta
          </h2>
          <p className="text-sm text-gray-500 mb-4">
            Você será desconectado e precisará fazer login novamente.
          </p>
          <button
            onClick={handleLogout}
            className="px-6 py-2.5 rounded-lg font-medium text-white"
            style={{ backgroundColor: '#DC2626' }}>
            Sair
          </button>
        </div>
      </main>
    </div>
  );
}