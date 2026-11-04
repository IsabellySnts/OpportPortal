import { useEffect, useState } from 'react';
import Header from '../../components/layout/Header';
import { candidateService } from '../../services/candidateService';
import { curriculumService } from '../../services/curriculumService';
import { formatPhone, formatCep } from '../../utils/Masks';

const educationLevels = [
  { value: 0, label: 'Ensino Médio' },
  { value: 1, label: 'Técnico' },
  { value: 2, label: 'Superior' },
  { value: 3, label: 'Pós-graduação' },
  { value: 4, label: 'Mestrado' },
  { value: 5, label: 'Doutorado' },
];

const getInitials = (name: string) =>
  name.trim().split(' ').filter(Boolean).map((w) => w[0]).slice(0, 2).join('').toUpperCase() || '?';

const formatDateBR = (iso: string) => {
  if (!iso) return '';
  const [y, m, d] = iso.split('-');
  return `${d}/${m}/${y}`;
};

export default function ProfilePage() {
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [editing, setEditing] = useState(false);
  const [message, setMessage] = useState('');

  const [candidateId, setCandidateId] = useState<number>(0);
  const [curriculumId, setCurriculumId] = useState<number>(0);

  const [personal, setPersonal] = useState({
    fullName: '', birthDate: '', phoneNumber: '', urlLinkedin: '',
    zipCode: '', street: '', number: '', neighborhood: '',
    city: '', state: '', country: 'Brasil', complement: '',
  });

  const [educationLevel, setEducationLevel] = useState(2);
  const [hardSkills, setHardSkills] = useState<string[]>([]);
  const [softSkills, setSoftSkills] = useState<string[]>([]);
  const [hardInput, setHardInput] = useState('');
  const [softInput, setSoftInput] = useState('');

  useEffect(() => { loadProfile(); }, []);

  const loadProfile = async () => {
    setLoading(true);
    try {
      const candResult = await candidateService.getMe();
      if (candResult.isSuccess) {
        const c = candResult.data;
        setCandidateId(c.id);
        setPersonal({
          fullName: c.fullName,
          birthDate: c.birthDate.split('T')[0],
          phoneNumber: c.phoneNumber,
          urlLinkedin: c.urlLinkedin || '',
          zipCode: c.zipCode,
          street: c.street,
          number: c.number,
          neighborhood: c.neighborhood,
          city: c.city,
          state: c.state,
          country: c.country,
          complement: c.complement || '',
        });

        const curResult = await curriculumService.getByCandidate(c.id);
        if (curResult.isSuccess) {
          setCurriculumId(curResult.data.id);
          setEducationLevel(curResult.data.educationLevel);
          setHardSkills(curResult.data.hardSkills);
          setSoftSkills(curResult.data.softSkills);
        }
      }
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (field: string, value: string) =>
    setPersonal((prev) => ({ ...prev, [field]: value }));

  const handleSave = async () => {
    setSaving(true);
    setMessage('');
    try {
      await candidateService.update(candidateId, {
        id: candidateId,
        ...personal,
        birthDate: new Date(personal.birthDate).toISOString(),
        complement: personal.complement || undefined,
        urlLinkedin: personal.urlLinkedin || undefined,
      });

      if (curriculumId) {
        await curriculumService.update(curriculumId, {
          id: curriculumId,
          educationLevel,
          hardSkills,
          softSkills,
        });
      }

      setMessage('Perfil atualizado com sucesso!');
      setEditing(false);
    } catch {
      setMessage('Erro ao salvar. Tente novamente.');
    } finally {
      setSaving(false);
    }
  };

  const handleCancel = () => {
    setMessage('');
    loadProfile();   // reverte para os dados salvos
    setEditing(false);
  };

  const inputClass = 'w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white';
  const labelClass = 'block text-sm font-medium mb-1';
  const educationLabel = educationLevels.find((l) => l.value === educationLevel)?.label ?? '—';

  // Campo somente-leitura
  const field = (label: string, value: string) => (
    <div className="min-w-0">
      <p className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-0.5">{label}</p>
      {value
        ? <p className="text-sm font-medium break-words" style={{ color: '#1B2A4A' }}>{value}</p>
        : <p className="text-sm italic text-gray-400">Não informado</p>}
    </div>
  );

  const cardHead = (icon: React.ReactNode, title: string) => (
    <div className="flex items-center gap-2.5 mb-4">
      <span className="w-7 h-7 rounded-lg grid place-items-center flex-shrink-0"
        style={{ backgroundColor: 'rgba(245,166,35,.14)', color: '#946008' }}>{icon}</span>
      <h2 className="font-semibold" style={{ color: '#1B2A4A' }}>{title}</h2>
    </div>
  );

  if (loading) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <p className="text-gray-500">Carregando perfil...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <Header
        navItems={[
          { label: 'Oportunidades', path: '/opportunities' },
          { label: 'Candidaturas', path: '/my-applications' },
          { label: 'Meu Perfil', path: '/my-profile' },
        ]}
      />

      <main className="max-w-3xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>Meu Perfil</h1>
        <p className="text-gray-500 mb-6">Seus dados e currículo</p>

        {message && (
          <p className="mb-4 text-sm font-medium"
            style={{ color: message.includes('sucesso') ? '#16A34A' : '#DC2626' }}>
            {message}
          </p>
        )}

        {/* ===== Cabeçalho ===== */}
        <div className="rounded-2xl p-6 mb-6 flex items-center gap-5 flex-wrap shadow-sm"
          style={{ background: 'linear-gradient(135deg, #1B2A4A, #26416e)' }}>
          <div className="w-16 h-16 rounded-full flex items-center justify-center font-bold text-xl flex-shrink-0"
            style={{ backgroundColor: '#F5A623', color: '#2a1c00', boxShadow: '0 0 0 4px rgba(255,255,255,.12)' }}>
            {getInitials(personal.fullName)}
          </div>
          <div className="flex-1 min-w-0">
            <p className="font-semibold text-lg text-white truncate">{personal.fullName}</p>
            <div className="flex items-center gap-3 flex-wrap mt-1.5">
              <span className="text-xs font-semibold uppercase tracking-wide px-2.5 py-0.5 rounded-full"
                style={{ backgroundColor: 'rgba(245,166,35,.18)', color: '#F5B749', border: '1px solid rgba(245,166,35,.45)' }}>
                Candidato
              </span>
              {personal.urlLinkedin
                ? <a href={personal.urlLinkedin} target="_blank" rel="noreferrer" className="text-sm text-white/75 hover:text-white underline">LinkedIn</a>
                : <span className="text-sm text-white/60">LinkedIn não informado</span>}
            </div>
          </div>
          {!editing && (
            <button onClick={() => setEditing(true)}
              className="px-4 py-2.5 rounded-xl font-semibold text-sm flex-shrink-0"
              style={{ backgroundColor: '#F5A623', color: '#2a1c00' }}>
              Editar perfil
            </button>
          )}
        </div>

        {!editing ? (
          /* ============ MODO VISUALIZAÇÃO ============ */
          <div className="space-y-6">
            <section className="bg-white rounded-2xl shadow-sm p-6">
              {cardHead(
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2" /><circle cx="12" cy="7" r="4" /></svg>,
                'Dados pessoais')}
              <div className="grid grid-cols-2 gap-x-6 gap-y-4">
                {field('Nome completo', personal.fullName)}
                {field('Data de nascimento', formatDateBR(personal.birthDate))}
                {field('Telefone', formatPhone(personal.phoneNumber))}
                {field('LinkedIn', personal.urlLinkedin)}
              </div>
            </section>

            <section className="bg-white rounded-2xl shadow-sm p-6">
              {cardHead(
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z" /><circle cx="12" cy="10" r="3" /></svg>,
                'Endereço')}
              <div className="grid grid-cols-2 gap-x-6 gap-y-4">
                {field('CEP', formatCep(personal.zipCode))}
                {field('Cidade / UF', `${personal.city} / ${personal.state}`)}
                {field('Rua', `${personal.street}, ${personal.number}`)}
                {field('Bairro', personal.neighborhood)}
                {field('Complemento', personal.complement)}
              </div>
            </section>

            <section className="bg-white rounded-2xl shadow-sm p-6">
              {cardHead(
                <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="M4 19.5A2.5 2.5 0 0 1 6.5 17H20" /><path d="M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2Z" /></svg>,
                'Currículo')}
              <div className="mb-5">{field('Formação', educationLabel)}</div>

              <p className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-2">Hard skills</p>
              <div className="flex flex-wrap gap-2 mb-5">
                {hardSkills.length ? hardSkills.map((s, i) => (
                  <span key={i} className="text-sm font-medium px-3 py-1 rounded-full"
                    style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>{s}</span>
                )) : <span className="text-sm italic text-gray-400">Nenhuma</span>}
              </div>

              <p className="text-xs font-semibold uppercase tracking-wide text-gray-400 mb-2">Soft skills</p>
              <div className="flex flex-wrap gap-2">
                {softSkills.length ? softSkills.map((s, i) => (
                  <span key={i} className="text-sm font-medium px-3 py-1 rounded-full border"
                    style={{ backgroundColor: 'rgba(245,166,35,.14)', color: '#946008', borderColor: 'rgba(245,166,35,.4)' }}>{s}</span>
                )) : <span className="text-sm italic text-gray-400">Nenhuma</span>}
              </div>
            </section>
          </div>
        ) : (
          /* ============ MODO EDIÇÃO (seu formulário) ============ */
          <div className="space-y-6">
            <div className="bg-white rounded-2xl shadow-sm p-8 space-y-4">
              <h2 className="font-semibold text-lg" style={{ color: '#1B2A4A' }}>Dados pessoais</h2>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Nome completo</label>
                <input className={inputClass} value={personal.fullName}
                  onChange={(e) => handleChange('fullName', e.target.value)} />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Data de nascimento</label>
                  <input type="date" className={inputClass} value={personal.birthDate}
                    onChange={(e) => handleChange('birthDate', e.target.value)} />
                </div>
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Telefone</label>
                  <input className={inputClass} value={personal.phoneNumber}
                    onChange={(e) => handleChange('phoneNumber', e.target.value)} />
                </div>
              </div>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>LinkedIn</label>
                <input className={inputClass} value={personal.urlLinkedin}
                  onChange={(e) => handleChange('urlLinkedin', e.target.value)} />
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>CEP</label>
                  <input className={inputClass} value={personal.zipCode}
                    onChange={(e) => handleChange('zipCode', e.target.value)} />
                </div>
                <div className="col-span-2">
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Rua</label>
                  <input className={inputClass} value={personal.street}
                    onChange={(e) => handleChange('street', e.target.value)} />
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Número</label>
                  <input className={inputClass} value={personal.number}
                    onChange={(e) => handleChange('number', e.target.value)} />
                </div>
                <div className="col-span-2">
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Bairro</label>
                  <input className={inputClass} value={personal.neighborhood}
                    onChange={(e) => handleChange('neighborhood', e.target.value)} />
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div className="col-span-2">
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Cidade</label>
                  <input className={inputClass} value={personal.city}
                    onChange={(e) => handleChange('city', e.target.value)} />
                </div>
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>UF</label>
                  <input className={inputClass} maxLength={2} value={personal.state}
                    onChange={(e) => handleChange('state', e.target.value.toUpperCase())} />
                </div>
              </div>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Complemento</label>
                <input className={inputClass} value={personal.complement}
                  onChange={(e) => handleChange('complement', e.target.value)} />
              </div>
            </div>

            <div className="bg-white rounded-2xl shadow-sm p-8 space-y-4">
              <h2 className="font-semibold text-lg" style={{ color: '#1B2A4A' }}>Currículo</h2>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Formação</label>
                <select className={inputClass} value={educationLevel}
                  onChange={(e) => setEducationLevel(Number(e.target.value))}>
                  {educationLevels.map((lvl) => (
                    <option key={lvl.value} value={lvl.value}>{lvl.label}</option>
                  ))}
                </select>
              </div>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Hard Skills</label>
                <input className={inputClass} value={hardInput}
                  onChange={(e) => setHardInput(e.target.value)}
                  onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); if (hardInput.trim()) { setHardSkills([...hardSkills, hardInput.trim()]); setHardInput(''); } } }}
                  placeholder="Digite e pressione Enter" />
                <div className="flex flex-wrap gap-2 mt-2">
                  {hardSkills.map((skill, i) => (
                    <span key={i} className="px-3 py-1 rounded-full text-sm flex items-center gap-2"
                      style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>
                      {skill}
                      <button type="button" onClick={() => setHardSkills(hardSkills.filter((_, idx) => idx !== i))}>×</button>
                    </span>
                  ))}
                </div>
              </div>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Soft Skills</label>
                <input className={inputClass} value={softInput}
                  onChange={(e) => setSoftInput(e.target.value)}
                  onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); if (softInput.trim()) { setSoftSkills([...softSkills, softInput.trim()]); setSoftInput(''); } } }}
                  placeholder="Digite e pressione Enter" />
                <div className="flex flex-wrap gap-2 mt-2">
                  {softSkills.map((skill, i) => (
                    <span key={i} className="px-3 py-1 rounded-full text-sm flex items-center gap-2"
                      style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>
                      {skill}
                      <button type="button" onClick={() => setSoftSkills(softSkills.filter((_, idx) => idx !== i))}>×</button>
                    </span>
                  ))}
                </div>
              </div>
            </div>

            <div className="flex gap-3">
              <button onClick={handleCancel}
                className="flex-1 py-3 rounded-xl font-semibold border-2"
                style={{ borderColor: '#1B2A4A', color: '#1B2A4A' }}>
                Cancelar
              </button>
              <button onClick={handleSave} disabled={saving}
                className="flex-1 py-3 rounded-xl font-semibold text-white disabled:opacity-50"
                style={{ backgroundColor: '#1B2A4A' }}>
                {saving ? 'Salvando...' : 'Salvar alterações'}
              </button>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}