import { useNavigate, useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { candidateService } from '../../services/candidateService';
import { companyService } from '../../services/companyService';
import logo from '../../assets/graduate.png';
import NotificationBell from './NotificationBell';

interface NavItem {
  label: string;
  path: string;
}

interface HeaderProps {
  navItems?: NavItem[];
}

function getInitials(name: string): string {
  return name
    .trim()
    .split(' ')
    .filter(Boolean)
    .map(w => w[0])
    .slice(0, 2)
    .join('')
    .toUpperCase();
}

export default function Header({ navItems = [] }: HeaderProps) {
  const { logout, userType } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();
  const [initials, setInitials] = useState('US');

  // Iniciais levam ao perfil (ajuste o destino da empresa se tiver uma tela própria)
  const profilePath = userType === 'Company' ? '/company/dashboard' : '/profile';

  useEffect(() => {
    loadUserName();
  }, [userType]);

  const loadUserName = async () => {
    try {
      if (userType === 'Company') {
        const result = await companyService.getMe();
        if (result.isSuccess) {
          setInitials(getInitials(result.data.companyName));
        }
      } else {
        const result = await candidateService.getMe();
        if (result.isSuccess) {
          setInitials(getInitials(result.data.fullName));
        }
      }
    } catch {
      setInitials('US');
    }
  };

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  return (
    <header className="text-white" style={{ backgroundColor: '#1B2A4A' }}>
      <div className="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
        <div className="flex items-center gap-3 cursor-pointer"
          onClick={() => navigate('/opportunities')}>
          <div className="w-9 h-9 rounded-lg flex items-center justify-center"
            style={{ backgroundColor: '#FFFFFF' }}>
            <img src={logo} alt="Logo" className="w-6 h-6 object-contain" />
          </div>
          <span className="font-bold text-lg">Portal de Oportunidades</span>
        </div>

        <nav className="flex items-center gap-6">
          {navItems.map((item) => {
            const isActive = location.pathname === item.path;
            return (
              <button
                key={item.label}
                onClick={() => navigate(item.path)}
                className={isActive ? 'font-semibold' : 'hover:opacity-80'}
                style={isActive ? { color: '#F5A623' } : undefined}
              >
                {item.label}
              </button>
            );
          })}
          <NotificationBell />

          {/* Iniciais → perfil */}
          <div
            className="w-9 h-9 rounded-full flex items-center justify-center cursor-pointer hover:opacity-90 transition"
            style={{ backgroundColor: '#F5A623' }}
            onClick={() => navigate(profilePath)}
            title="Meu perfil"
          >
            <span className="font-bold text-sm" style={{ color: '#1B2A4A' }}>{initials}</span>
          </div>

          {/* Ícone de logout */}
          <button
            onClick={handleLogout}
            title="Sair"
            aria-label="Sair"
            className="p-2 rounded-lg hover:bg-white/10 transition"
          >
            <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24"
              fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
              <path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4" />
              <polyline points="16 17 21 12 16 7" />
              <line x1="21" y1="12" x2="9" y2="12" />
            </svg>
          </button>
        </nav>
      </div>
    </header>
  );
}