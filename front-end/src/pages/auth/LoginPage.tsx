import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuth } from '../../contexts/AuthContext';
import { authService } from '../../services/authService';
import { candidateService } from '../../services/candidateService';
import { companyService } from '../../services/companyService';
import logo from '../../assets/graduate.png';
import PasswordInput from '../../components/ui/PasswordInput';
import { GoogleLogin } from '@react-oauth/google';

export default function LoginPage() {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  // Redireciona conforme o papel do usuário (usado no login normal e no Google)
  const handleAfterLogin = async (token: string) => {
    login(token);
    const payload = JSON.parse(atob(token.split('.')[1]));
    const role = payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (role === 'Company') {
      try {
        const company = await companyService.getMe();
        navigate(company.isSuccess ? '/company/dashboard' : '/company/setup');
      } catch { navigate('/company/setup'); }
    } else {
      try {
        const candidate = await candidateService.getMe();
        navigate(candidate.isSuccess ? '/opportunities' : '/setup/candidate');
      } catch { navigate('/setup/candidate'); }
    }
  };

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
      await handleAfterLogin(result.data.token);
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
            style={{ backgroundColor: '#FFFFFF' }}>
            <img src={logo} alt="Logo" className="w-6 h-6 object-contain" />
          </div>
          <h1 className="text-4xl font-bold text-white mb-4">OpportPortal</h1>
          <p className="text-lg" style={{ color: '#A8B8D0' }}>
            Conectando estudantes às melhores oportunidades acadêmicas e profissionais.
          </p>
        </div>
      </div>

      {/* Lado direito — formulário */}
      <div className="w-full lg:w-1/2 flex items-center justify-center px-8 bg-gray-50">
        <div className="w-full max-w-md">

          <div className="lg:hidden text-center mb-8">
            <div className="w-12 h-12 rounded-xl flex items-center justify-center mx-auto mb-3"
              style={{ backgroundColor: '#1B2A4A' }}>
              <img src={logo} alt="Logo" className="w-6 h-6 object-contain" />
            </div>
            <h1 className="text-2xl font-bold" style={{ color: '#1B2A4A' }}>OpportPortal</h1>
          </div>

          <h2 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>Bem-vindo de volta!</h2>
          <p className="text-gray-500 mb-8">Acesse sua conta para continuar</p>

          <form onSubmit={handleSubmit} className="space-y-5">
            <div>
              <label className="block text-sm font-medium mb-1" style={{ color: '#1B2A4A' }}>E-mail</label>
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
              <label className="block text-sm font-medium mb-1" style={{ color: '#1B2A4A' }}>Senha</label>
              <PasswordInput value={password} onChange={setPassword} required />
            </div>

            {error && (
              <div className="bg-red-50 border border-red-200 rounded-xl px-4 py-3">
                <p className="text-red-600 text-sm">{error}</p>
              </div>
            )}

            <button type="submit" disabled={loading}
              className="w-full py-3 rounded-xl font-semibold text-white transition-opacity disabled:opacity-50"
              style={{ backgroundColor: '#1B2A4A' }}>
              {loading ? 'Entrando...' : 'Entrar'}
            </button>

            <button type="button"
              className="w-full py-3 rounded-xl font-semibold transition-opacity border-2"
              style={{ borderColor: '#F5A623', color: '#F5A623' }}
              onClick={() => navigate('/register')}>
              Criar conta
            </button>
          </form>

          {/* Divisor + Google */}
          <div className="flex items-center gap-3 my-6">
            <div className="flex-1 h-px bg-gray-200" />
            <span className="text-sm text-gray-400">ou</span>
            <div className="flex-1 h-px bg-gray-200" />
          </div>

          <div className="flex justify-center">
            <GoogleLogin
              text="continue_with"
              onSuccess={async (cred) => {
                if (!cred.credential) return;
                setError('');
                const result = await authService.googleAuth(cred.credential); // login (sem type)

                if (!result.isSuccess) {
                  setError(result.message ?? 'Erro ao autenticar com Google.');
                  return;
                }

                if (result.data.requiresRegistration) {
                  // conta nova → leva pro cadastro já autenticada
                  navigate('/register', { state: { googleCredential: cred.credential } });
                } else if (result.data.token) {
                  await handleAfterLogin(result.data.token);
                }
              }}
              onError={() => setError('Erro ao autenticar com Google.')}
            />
          </div>

          <p className="text-center text-sm text-gray-400 mt-8">
            Ao entrar, você concorda com nossos{' '}
            <span className="underline cursor-pointer" style={{ color: '#1B2A4A' }}>Termos de Uso</span>
          </p>
        </div>
      </div>
    </div>
  );
}