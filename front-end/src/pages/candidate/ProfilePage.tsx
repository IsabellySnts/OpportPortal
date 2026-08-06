import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { candidateService } from '../../services/candidateService';
import { curriculumService } from '../../services/curriculumService';
import Header from '../../components/layout/Header';

const educationLevels = [
  { value: 0, label: 'Ensino Médio' },
  { value: 1, label: 'Técnico' },
  { value: 2, label: 'Superior' },
  { value: 3, label: 'Pós-graduação' },
  { value: 4, label: 'Mestrado' },
  { value: 5, label: 'Doutorado' },
];

export default function ProfilePage() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
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

  useEffect(() => {
    loadProfile();
  }, []);

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

        // Carrega currículo
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

  const handleChange = (field: string, value: string) => {
    setPersonal(prev => ({ ...prev, [field]: value }));
  };

  const handleSave = async () => {
    setSaving(true);
    setMessage('');
    try {
      // Atualiza candidato
      await candidateService.update(candidateId, {
        id: candidateId,
        ...personal,
        birthDate: new Date(personal.birthDate).toISOString(),
        complement: personal.complement || undefined,
        urlLinkedin: personal.urlLinkedin || undefined,
      });

      // Atualiza currículo
      if (curriculumId) {
        await curriculumService.update(curriculumId, {
          id: curriculumId,
          educationLevel,
          hardSkills,
          softSkills,
        });
      }

      setMessage('Perfil atualizado com sucesso!');
    } catch {
      setMessage('Erro ao salvar. Tente novamente.');
    } finally {
      setSaving(false);
    }
  };

  const inputClass = "w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white";
  const labelClass = "block text-sm font-medium mb-1";

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
  

      {/* Conteúdo */}
      <main className="max-w-3xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
          Meu Perfil
        </h1>
        <p className="text-gray-500 mb-6">Edite seus dados e currículo</p>

        {/* Dados pessoais */}
        <div className="bg-white rounded-2xl shadow-sm p-8 mb-6 space-y-4">
          <h2 className="font-semibold text-lg" style={{ color: '#1B2A4A' }}>
            Dados pessoais
          </h2>

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

        {/* Currículo */}
        <div className="bg-white rounded-2xl shadow-sm p-8 mb-6 space-y-4">
          <h2 className="font-semibold text-lg" style={{ color: '#1B2A4A' }}>
            Currículo
          </h2>

          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Formação</label>
            <select className={inputClass} value={educationLevel}
              onChange={(e) => setEducationLevel(Number(e.target.value))}>
              {educationLevels.map(lvl => (
                <option key={lvl.value} value={lvl.value}>{lvl.label}</option>
              ))}
            </select>
          </div>

          {/* Hard Skills */}
          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Hard Skills</label>
            <div className="flex gap-2">
              <input className={inputClass} value={hardInput}
                onChange={(e) => setHardInput(e.target.value)}
                onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); if (hardInput.trim()) { setHardSkills([...hardSkills, hardInput.trim()]); setHardInput(''); } } }}
                placeholder="Digite e pressione Enter" />
            </div>
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

          {/* Soft Skills */}
          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Soft Skills</label>
            <div className="flex gap-2">
              <input className={inputClass} value={softInput}
                onChange={(e) => setSoftInput(e.target.value)}
                onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); if (softInput.trim()) { setSoftSkills([...softSkills, softInput.trim()]); setSoftInput(''); } } }}
                placeholder="Digite e pressione Enter" />
            </div>
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

        {message && (
          <p className="mb-4 text-sm" style={{ color: message.includes('sucesso') ? '#16A34A' : '#DC2626' }}>
            {message}
          </p>
        )}

        <button onClick={handleSave} disabled={saving}
          className="w-full py-3 rounded-xl font-semibold text-white disabled:opacity-50"
          style={{ backgroundColor: '#1B2A4A' }}>
          {saving ? 'Salvando...' : 'Salvar alterações'}
        </button>
      </main>
    </div>
  );
}