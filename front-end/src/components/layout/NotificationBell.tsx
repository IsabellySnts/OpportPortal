import { useEffect, useRef, useState } from 'react';
import { notificationService, NotificationItem } from '../../services/notificationService';

export default function NotificationBell() {
  const [count, setCount] = useState(0);
  const [items, setItems] = useState<NotificationItem[]>([]);
  const [open, setOpen] = useState(false);
  const ref = useRef<HTMLDivElement>(null);

  const loadCount = async () => {
    try {
      const res = await notificationService.getUnreadCount();
      if (res.isSuccess) setCount(res.data);
    } catch { /* ignora */ }
  };

  const loadItems = async () => {
    try {
      const res = await notificationService.getMy();
      if (res.isSuccess) setItems(res.data);
    } catch { /* ignora */ }
  };

  // Polling do contador (a cada 30s)
  useEffect(() => {
    loadCount();
    const interval = setInterval(loadCount, 30000);
    return () => clearInterval(interval);
  }, []);

  // Fecha ao clicar fora
  useEffect(() => {
    const handler = (e: MouseEvent) => {
      if (ref.current && !ref.current.contains(e.target as Node)) setOpen(false);
    };
    document.addEventListener('mousedown', handler);
    return () => document.removeEventListener('mousedown', handler);
  }, []);

  const toggle = async () => {
    const next = !open;
    setOpen(next);
    if (next) await loadItems();
  };

  const handleClick = async (n: NotificationItem) => {
    if (!n.isRead) {
      await notificationService.markAsRead(n.id);
      setItems((prev) => prev.map((x) => (x.id === n.id ? { ...x, isRead: true } : x)));
      setCount((c) => Math.max(0, c - 1));
    }
  };

  const handleMarkAll = async () => {
    await notificationService.markAllAsRead();
    setItems((prev) => prev.map((x) => ({ ...x, isRead: true })));
    setCount(0);
  };

  const formatDate = (iso: string) => {
    const d = new Date(iso);
    return `${d.toLocaleDateString('pt-BR')} ${d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' })}`;
  };

  return (
    <div className="relative" ref={ref}>
      <button onClick={toggle} title="Notificações"
        className="relative p-2 rounded-lg text-white/80 hover:text-white hover:bg-white/10 transition">
        <svg xmlns="http://www.w3.org/2000/svg" width="20" height="20" viewBox="0 0 24 24"
          fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
          <path d="M6 8a6 6 0 0 1 12 0c0 7 3 9 3 9H3s3-2 3-9" />
          <path d="M10.3 21a1.94 1.94 0 0 0 3.4 0" />
        </svg>
        {count > 0 && (
          <span className="absolute -top-0.5 -right-0.5 min-w-[18px] h-[18px] px-1 rounded-full text-[11px] font-bold flex items-center justify-center"
            style={{ backgroundColor: '#F5A623', color: '#1B2A4A' }}>
            {count > 9 ? '9+' : count}
          </span>
        )}
      </button>

      {open && (
        <div className="absolute right-0 mt-2 w-80 bg-white rounded-xl shadow-lg border border-gray-100 z-50 overflow-hidden">
          <div className="flex items-center justify-between px-4 py-3 border-b border-gray-100">
            <span className="font-semibold" style={{ color: '#1B2A4A' }}>Notificações</span>
            {items.some((i) => !i.isRead) && (
              <button onClick={handleMarkAll} className="text-xs font-medium" style={{ color: '#F5A623' }}>
                Marcar todas como lidas
              </button>
            )}
          </div>

          <div className="max-h-96 overflow-y-auto">
            {items.length === 0 ? (
              <p className="text-center text-gray-400 text-sm py-8">Nenhuma notificação.</p>
            ) : (
              items.map((n) => (
                <button key={n.id} onClick={() => handleClick(n)}
                  className="w-full text-left px-4 py-3 border-b border-gray-50 hover:bg-gray-50 transition flex gap-3">
                  <span className="mt-1.5 w-2 h-2 rounded-full flex-shrink-0"
                    style={{ backgroundColor: n.isRead ? 'transparent' : '#F5A623' }} />
                  <div className="flex-1">
                    <p className="text-sm font-semibold" style={{ color: '#1B2A4A' }}>{n.title}</p>
                    <p className="text-sm text-gray-500">{n.message}</p>
                    <p className="text-xs text-gray-400 mt-1">{formatDate(n.createdAt)}</p>
                  </div>
                </button>
              ))
            )}
          </div>
        </div>
      )}
    </div>
  );
}