import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { candidateService } from '../../services/candidateService';
import { curriculumService } from '../../services/curriculumService';

const educationLevels = [
  { value: 0, label: 'Ensino Médio' },
  { value: 1, label: 'Técnico' },
  { value: 2, label: 'Superior' },
  { value: 3, label: 'Pós-graduação' },
  { value: 4, label: 'Mestrado' },
  { value: 5, label: 'Doutorado' },
];

export default function CandidateSetupPage() {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  // Etapa 1 — Dados pessoais
  const [personal, setPersonal] = useState({
    fullName: '',
    birthDate: '',
    phoneNumber: '',
    urlLinkedin: '',
    zipCode: '',
    street: '',
    number: '',
    neighborhood: '',
    city: '',
    state: '',
    country: 'Brasil',
    complement: '',
  });

  // Etapa 2 — Currículo
  const [educationLevel, setEducationLevel] = useState(2);
  const [hardSkills, setHardSkills] = useState<string[]>([]);
  const [softSkills, setSoftSkills] = useState<string[]>([]);
  const [hardInput, setHardInput] = useState('');
  const [softInput, setSoftInput] = useState('');

  const handlePersonalChange = (field: string, value: string) => {
    setPersonal(prev => ({ ...prev, [field]: value }));
  };

  const addHardSkill = () => {
    if (hardInput.trim()) {
      setHardSkills([...hardSkills, hardInput.trim()]);
      setHardInput('');
    }
  };

  const addSoftSkill = () => {
    if (softInput.trim()) {
      setSoftSkills([...softSkills, softInput.trim()]);
      setSoftInput('');
    }
  };

  const handleNext = (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setStep(2);
  };

  const handleFinish = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');

    if (hardSkills.length === 0 || softSkills.length === 0) {
      setError('Adicione pelo menos uma hard skill e uma soft skill.');
      return;
    }

    setLoading(true);
    try {
      // 1. Cria o candidato
      const candidateResult = await candidateService.create({
        ...personal,
        birthDate: new Date(personal.birthDate).toISOString(),
        complement: personal.complement || undefined,
        urlLinkedin: personal.urlLinkedin || undefined,
      });

      if (!candidateResult.isSuccess) {
        setError(candidateResult.message);
        return;
      }

      // 2. Cria o currículo
      const curriculumResult = await curriculumService.create({
        educationLevel,
        hardSkills,
        softSkills,
      });

      if (!curriculumResult.isSuccess) {
        setError(curriculumResult.message);
        return;
      }

      navigate('/opportunities');
    } catch {
      setError('Erro ao salvar. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const inputClass = "w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white";
  const labelClass = "block text-sm font-medium mb-1";

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-2xl mx-auto">

        {/* Progresso */}
        <div className="flex items-center justify-center gap-4 mb-8">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-full flex items-center justify-center text-white font-bold"
              style={{ backgroundColor: step >= 1 ? '#1B2A4A' : '#D1D5DB' }}>
              1
            </div>
            <span style={{ color: step >= 1 ? '#1B2A4A' : '#9CA3AF' }}>Dados pessoais</span>
          </div>
          <div className="w-12 h-0.5 bg-gray-300" />
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-full flex items-center justify-center text-white font-bold"
              style={{ backgroundColor: step >= 2 ? '#1B2A4A' : '#D1D5DB' }}>
              2
            </div>
            <span style={{ color: step >= 2 ? '#1B2A4A' : '#9CA3AF' }}>Currículo</span>
          </div>
        </div>

        <div className="bg-white rounded-2xl shadow-sm p-8">

          {step === 1 && (
            <form onSubmit={handleNext} className="space-y-4">
              <h2 className="text-xl font-bold mb-4" style={{ color: '#1B2A4A' }}>
                Complete seu perfil
              </h2>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Nome completo</label>
                <input className={inputClass} required
                  value={personal.fullName}
                  onChange={(e) => handlePersonalChange('fullName', e.target.value)} />
              </div>

              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Data de nascimento</label>
                  <input type="date" className={inputClass} required
                    value={personal.birthDate}
                    onChange={(e) => handlePersonalChange('birthDate', e.target.value)} />
                </div>
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Telefone</label>
                  <input className={inputClass} required placeholder="11999999999"
                    value={personal.phoneNumber}
                    onChange={(e) => handlePersonalChange('phoneNumber', e.target.value)} />
                </div>
              </div>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>LinkedIn (opcional)</label>
                <input className={inputClass} placeholder="https://linkedin.com/in/seu-perfil"
                  value={personal.urlLinkedin}
                  onChange={(e) => handlePersonalChange('urlLinkedin', e.target.value)} />
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>CEP</label>
                  <input className={inputClass} required placeholder="00000000"
                    value={personal.zipCode}
                    onChange={(e) => handlePersonalChange('zipCode', e.target.value)} />
                </div>
                <div className="col-span-2">
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Rua</label>
                  <input className={inputClass} required
                    value={personal.street}
                    onChange={(e) => handlePersonalChange('street', e.target.value)} />
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Número</label>
                  <input className={inputClass} required
                    value={personal.number}
                    onChange={(e) => handlePersonalChange('number', e.target.value)} />
                </div>
                <div className="col-span-2">
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Bairro</label>
                  <input className={inputClass} required
                    value={personal.neighborhood}
                    onChange={(e) => handlePersonalChange('neighborhood', e.target.value)} />
                </div>
              </div>

              <div className="grid grid-cols-3 gap-4">
                <div className="col-span-2">
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>Cidade</label>
                  <input className={inputClass} required
                    value={personal.city}
                    onChange={(e) => handlePersonalChange('city', e.target.value)} />
                </div>
                <div>
                  <label className={labelClass} style={{ color: '#1B2A4A' }}>UF</label>
                  <input className={inputClass} required maxLength={2} placeholder="SP"
                    value={personal.state}
                    onChange={(e) => handlePersonalChange('state', e.target.value.toUpperCase())} />
                </div>
              </div>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Complemento (opcional)</label>
                <input className={inputClass}
                  value={personal.complement}
                  onChange={(e) => handlePersonalChange('complement', e.target.value)} />
              </div>

              {error && <p className="text-red-500 text-sm">{error}</p>}

              <button type="submit"
                className="w-full py-3 rounded-xl font-semibold text-white"
                style={{ backgroundColor: '#1B2A4A' }}>
                Continuar
              </button>
            </form>
          )}

          {step === 2 && (
            <form onSubmit={handleFinish} className="space-y-5">
              <h2 className="text-xl font-bold mb-4" style={{ color: '#1B2A4A' }}>
                Seu currículo
              </h2>

              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>Formação</label>
                <select className={inputClass}
                  value={educationLevel}
                  onChange={(e) => setEducationLevel(Number(e.target.value))}>
                  {educationLevels.map(lvl => (
                    <option key={lvl.value} value={lvl.value}>{lvl.label}</option>
                  ))}
                </select>
              </div>

              {/* Hard Skills */}
              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>
                  Hard Skills (ex: C#, SQL, React)
                </label>
                <div className="flex gap-2">
                  <input className={inputClass}
                    value={hardInput}
                    onChange={(e) => setHardInput(e.target.value)}
                    onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); addHardSkill(); } }}
                    placeholder="Digite e pressione Enter" />
                  <button type="button" onClick={addHardSkill}
                    className="px-4 rounded-xl text-white font-medium"
                    style={{ backgroundColor: '#F5A623' }}>+</button>
                </div>
                <div className="flex flex-wrap gap-2 mt-2">
                  {hardSkills.map((skill, i) => (
                    <span key={i}
                      className="px-3 py-1 rounded-full text-sm flex items-center gap-2"
                      style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>
                      {skill}
                      <button type="button" onClick={() => setHardSkills(hardSkills.filter((_, idx) => idx !== i))}>×</button>
                    </span>
                  ))}
                </div>
              </div>

              {/* Soft Skills */}
              <div>
                <label className={labelClass} style={{ color: '#1B2A4A' }}>
                  Soft Skills (ex: Comunicação, Liderança)
                </label>
                <div className="flex gap-2">
                  <input className={inputClass}
                    value={softInput}
                    onChange={(e) => setSoftInput(e.target.value)}
                    onKeyDown={(e) => { if (e.key === 'Enter') { e.preventDefault(); addSoftSkill(); } }}
                    placeholder="Digite e pressione Enter" />
                  <button type="button" onClick={addSoftSkill}
                    className="px-4 rounded-xl text-white font-medium"
                    style={{ backgroundColor: '#F5A623' }}>+</button>
                </div>
                <div className="flex flex-wrap gap-2 mt-2">
                  {softSkills.map((skill, i) => (
                    <span key={i}
                      className="px-3 py-1 rounded-full text-sm flex items-center gap-2"
                      style={{ backgroundColor: '#EEF2F9', color: '#1B2A4A' }}>
                      {skill}
                      <button type="button" onClick={() => setSoftSkills(softSkills.filter((_, idx) => idx !== i))}>×</button>
                    </span>
                  ))}
                </div>
              </div>

              {error && <p className="text-red-500 text-sm">{error}</p>}

              <div className="flex gap-3">
                <button type="button" onClick={() => setStep(1)}
                  className="flex-1 py-3 rounded-xl font-semibold border-2"
                  style={{ borderColor: '#1B2A4A', color: '#1B2A4A' }}>
                  Voltar
                </button>
                <button type="submit" disabled={loading}
                  className="flex-1 py-3 rounded-xl font-semibold text-white disabled:opacity-50"
                  style={{ backgroundColor: '#1B2A4A' }}>
                  {loading ? 'Salvando...' : 'Finalizar'}
                </button>
              </div>
            </form>
          )}
        </div>
      </div>
    </div>
  );
}