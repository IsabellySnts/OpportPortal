import { useNavigate, useLocation } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useAuth } from '../../contexts/AuthContext';
import { candidateService } from '../../services/candidateService';
import { companyService } from '../../services/companyService';
import logo from '../../assets/graduate.png';

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

        <nav className="flex items-center gap-8">
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
          <div className="w-9 h-9 rounded-full flex items-center justify-center cursor-pointer"
            style={{ backgroundColor: '#F5A623' }}
            onClick={handleLogout} title="Sair">
            <span className="font-bold text-sm">{initials}</span>
          </div>
        </nav>
      </div>
    </header>
  );
}