import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { authService } from '../../services/authService';

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const result = await authService.login(email, password);

      if (!result.isSuccess) {
        setError(result.message);
        return;
      }

      login(result.data.token);
      navigate('/opportunities');
    } catch {
      setError('Erro ao fazer login. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex">
      <div className="hidden lg:flex w-1/2 flex-col justify-center items-center px-16"
        style={{ backgroundColor: '#1B2A4A' }}>
        <div className="text-center">
          <div className="w-16 h-16 rounded-2xl flex items-center justify-center mx-auto mb-6"
            style={{ backgroundColor: '#F5A623' }}>
            <span className="text-white font-bold text-2xl">P</span>
          </div>
          <h1 className="text-4xl font-bold text-white mb-4">
            OpportPortal
          </h1>
          <p className="text-lg" style={{ color: '#A8B8D0' }}>
            Conectando estudantes às melhores oportunidades acadêmicas e profissionais.
          </p>
        </div>
      </div>

      {/* Lado direito — formulário */}
      <div className="w-full lg:w-1/2 flex items-center justify-center px-8 bg-gray-50">
        <div className="w-full max-w-md">

          {/* Logo mobile */}
          <div className="lg:hidden text-center mb-8">
            <div className="w-12 h-12 rounded-xl flex items-center justify-center mx-auto mb-3"
              style={{ backgroundColor: '#1B2A4A' }}>
              <span className="text-white font-bold text-xl">P</span>
            </div>
            <h1 className="text-2xl font-bold" style={{ color: '#1B2A4A' }}>
              OpportPortal
            </h1>
          </div>

          <h2 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
            Bem-vinda de volta!
          </h2>
          <p className="text-gray-500 mb-8">Acesse sua conta para continuar</p>

          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-sm font-medium mb-1"
                style={{ color: '#1B2A4A' }}>
                E-mail
              </label>
              <input
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                className="w-full border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 bg-white"
                style={{ '--tw-ring-color': '#1B2A4A' } as React.CSSProperties}
                placeholder="seu@email.com"
                required
              />
            </div>

            <div>
              <label className="block text-sm font-medium mb-1"
                style={{ color: '#1B2A4A' }}>
                Senha
              </label>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="w-full border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 bg-white"
                placeholder="••••••••"
                required
              />
            </div>

            {error && (
              <div className="bg-red-50 border border-red-200 rounded-xl px-4 py-3">
                <p className="text-red-600 text-sm">{error}</p>
              </div>
            )}

            <button
              type="submit"
              disabled={loading}
              className="w-full py-3 rounded-xl font-semibold text-white transition-opacity disabled:opacity-50"
              style={{ backgroundColor: '#1B2A4A' }}
            >
              {loading ? 'Entrando...' : 'Entrar'}
            </button>

            <button
              type="button"
              className="w-full py-3 rounded-xl font-semibold transition-opacity border-2"
              style={{ borderColor: '#F5A623', color: '#F5A623' }}
              onClick={() => navigate('/register')}
            >
              Criar conta
            </button>
          </form>

          <p className="text-center text-sm text-gray-400 mt-8">
            Ao entrar, você concorda com nossos{' '}
            <span className="underline cursor-pointer" style={{ color: '#1B2A4A' }}>
              Termos de Uso
            </span>
          </p>
        </div>
      </div>
    </div>
  );
}