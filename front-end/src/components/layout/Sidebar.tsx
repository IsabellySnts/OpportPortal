import { useNavigate, useLocation } from 'react-router-dom';

interface SidebarItem {
  label: string;
  path: string;
  badge?: number;
}

interface SidebarSection {
  title: string;
  items: SidebarItem[];
}

interface SidebarProps {
  sections: SidebarSection[];
}

export default function Sidebar({ sections }: SidebarProps) {
  const navigate = useNavigate();
  const location = useLocation();

  return (
    <aside className="w-56 flex-shrink-0">
      {sections.map((section) => (
        <div key={section.title} className="mb-8">
          <p className="text-xs font-semibold text-gray-400 mb-3 uppercase">
            {section.title}
          </p>
          <nav className="space-y-1">
            {section.items.map((item) => {
              const isActive = location.pathname === item.path;
              return (
                <button
                  key={item.label}
                  onClick={() => navigate(item.path)}
                  className={`w-full text-left px-4 py-2 rounded-lg flex justify-between items-center ${
                    isActive ? 'font-medium' : 'text-gray-600 hover:bg-gray-100'
                  }`}
                  style={isActive ? { backgroundColor: '#EEF2F9', color: '#1B2A4A' } : undefined}
                >
                  {item.label}
                  {item.badge !== undefined && (
                    <span className="text-xs px-2 py-0.5 rounded-full text-white"
                      style={{ backgroundColor: '#F5A623' }}>
                      {item.badge}
                    </span>
                  )}
                </button>
              );
            })}
          </nav>
        </div>
      ))}
    </aside>
  );
}