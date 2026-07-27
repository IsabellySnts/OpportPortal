import { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { authService } from '../../services/authService';
import { UserType } from '../../types';

export default function RegisterPage() {
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [type, setType] = useState<UserType>(UserType.Candidate);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (password !== confirmPassword) {
      setError('As senhas não coincidem.');
      return;
    }

    setLoading(true);

    try {
      const result = await authService.register(email, password, type);

      if (!result.isSuccess) {
        setError(result.message);
        return;
      }

      setSuccess('Conta criada com sucesso! Redirecionando...');
      setTimeout(() => navigate('/login'), 1500);
    } catch {
      setError('Erro ao criar conta. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen flex">

      {/* Lado esquerdo — branding */}
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
            Comece sua jornada profissional hoje mesmo.
          </p>
        </div>
      </div>

      {/* Lado direito — formulário */}
      <div className="w-full lg:w-1/2 flex items-center justify-center px-8 bg-gray-50 py-8">
        <div className="w-full max-w-md">

          <div className="lg:hidden text-center mb-8">
            <div className="w-12 h-12 rounded-xl flex items-center justify-center mx-auto mb-3"
              style={{ backgroundColor: '#1B2A4A' }}>
              <span className="text-white font-bold text-xl">P</span>
            </div>
          </div>

          <h2 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
            Criar conta
          </h2>
          <p className="text-gray-500 mb-8">Preencha os dados para começar</p>

          <form onSubmit={handleSubmit} className="space-y-5">

            {/* Tipo de conta */}
            <div>
              <label className="block text-sm font-medium mb-2"
                style={{ color: '#1B2A4A' }}>
                Tipo de conta
              </label>
              <div className="grid grid-cols-2 gap-3">
                <button
                  type="button"
                  onClick={() => setType(UserType.Candidate)}
                  className="py-3 rounded-xl font-medium border-2 transition"
                  style={type === UserType.Candidate
                    ? { backgroundColor: '#1B2A4A', color: 'white', borderColor: '#1B2A4A' }
                    : { backgroundColor: 'white', color: '#1B2A4A', borderColor: '#E5E7EB' }}
                >
                  Candidato
                </button>
                <button
                  type="button"
                  onClick={() => setType(UserType.Company)}
                  className="py-3 rounded-xl font-medium border-2 transition"
                  style={type === UserType.Company
                    ? { backgroundColor: '#1B2A4A', color: 'white', borderColor: '#1B2A4A' }
                    : { backgroundColor: 'white', color: '#1B2A4A', borderColor: '#E5E7EB' }}
                >
                  Empresa
                </button>
              </div>
            </div>

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

            <div>
              <label className="block text-sm font-medium mb-1"
                style={{ color: '#1B2A4A' }}>
                Confirmar senha
              </label>
              <input
                type="password"
                value={confirmPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
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

            {success && (
              <div className="bg-green-50 border border-green-200 rounded-xl px-4 py-3">
                <p className="text-green-600 text-sm">{success}</p>
              </div>
            )}

            <button
              type="submit"
              disabled={loading}
              className="w-full py-3 rounded-xl font-semibold text-white transition-opacity disabled:opacity-50"
              style={{ backgroundColor: '#1B2A4A' }}
            >
              {loading ? 'Criando conta...' : 'Criar conta'}
            </button>
          </form>

          <p className="text-center text-sm text-gray-500 mt-6">
            Já tem conta?{' '}
            <Link to="/login" className="font-medium hover:underline"
              style={{ color: '#F5A623' }}>
              Fazer login
            </Link>
          </p>
        </div>
      </div>
    </div>
  );
}