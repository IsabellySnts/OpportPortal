import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { opportunityService } from '../../services/opportunityService';
import { onlyDigits } from '../../utils/Masks';
import Header from '../../components/layout/Header';

const types = [
  { value: 1, label: 'Estágio' },
  { value: 2, label: 'Freelance' },
  { value: 3, label: 'CLT' },
  { value: 4, label: 'PJ' },
];

const formats = [
  { value: 1, label: 'Presencial' },
  { value: 2, label: 'Remoto' },
  { value: 3, label: 'Híbrido' },
];

const maskSalary = (v: string) => {
  const d = onlyDigits(v);
  return d ? Number(d).toLocaleString('pt-BR') : '';
};

export default function NewOpportunityPage() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [form, setForm] = useState({
    title: '',
    description: '',
    type: 1,
    format: 1,
    requirements: '',
    fieldOfWork: '',
    state: '',
    salary: '', // guarda já formatado: "6.000"
  });

  const handleChange = (field: string, value: string | number) => {
    setForm(prev => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const result = await opportunityService.create({
        title: form.title,
        description: form.description,
        type: form.type,
        format: form.format,
        requirements: form.requirements,
        fieldOfWork: form.fieldOfWork,
        state: form.state,
        salary: form.salary ? Number(onlyDigits(form.salary)) : 0,
      });

      if (!result.isSuccess) {
        setError(result.message);
        return;
      }

      navigate('/company/dashboard');
    } catch {
      setError('Erro ao publicar vaga. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const inputClass = "w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white";
  const labelClass = "block text-sm font-medium mb-1";

  return (
    <div className="min-h-screen bg-gray-50">

      <Header
        navItems={[
          { label: 'Minhas vagas', path: '/company/dashboard' },
          { label: 'Meu Perfil', path: '/my-profile' },
        ]}
      />

      {/* Formulário */}
      <main className="max-w-2xl mx-auto px-6 py-8">
        <h1 className="text-2xl font-bold mb-1" style={{ color: '#1B2A4A' }}>
          Nova Vaga
        </h1>
        <p className="text-gray-500 mb-6">Preencha os dados da oportunidade</p>

        <form onSubmit={handleSubmit} className="bg-white rounded-2xl shadow-sm p-8 space-y-4">
          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Título</label>
            <input className={inputClass} required
              value={form.title}
              onChange={(e) => handleChange('title', e.target.value)}
              placeholder="Ex: Desenvolvedor Full Stack Junior" />
          </div>

          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Descrição</label>
            <textarea className={inputClass} required rows={4}
              value={form.description}
              onChange={(e) => handleChange('description', e.target.value)}
              placeholder="Descreva a vaga..." />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Tipo</label>
              <select className={inputClass}
                value={form.type}
                onChange={(e) => handleChange('type', Number(e.target.value))}>
                {types.map(t => <option key={t.value} value={t.value}>{t.label}</option>)}
              </select>
            </div>
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Formato</label>
              <select className={inputClass}
                value={form.format}
                onChange={(e) => handleChange('format', Number(e.target.value))}>
                {formats.map(f => <option key={f.value} value={f.value}>{f.label}</option>)}
              </select>
            </div>
          </div>

          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Requisitos</label>
            <textarea className={inputClass} required rows={3}
              value={form.requirements}
              onChange={(e) => handleChange('requirements', e.target.value)}
              placeholder="Ex: Conhecimento em C#, React, SQL..." />
          </div>

          <div className="grid grid-cols-3 gap-4">
            <div className="col-span-2">
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Área</label>
              <input className={inputClass} required
                value={form.fieldOfWork}
                onChange={(e) => handleChange('fieldOfWork', e.target.value)}
                placeholder="Ex: Tecnologia da Informação" />
            </div>
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Estado</label>
              <input className={inputClass} required maxLength={2}
                value={form.state}
                onChange={(e) => handleChange('state', e.target.value.toUpperCase())}
                placeholder="SP" />
            </div>
          </div>

          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>
              Remuneração (deixe vazio para "A combinar")
            </label>
            <div className="relative">
              <span className="absolute left-4 top-1/2 -translate-y-1/2 text-gray-500 pointer-events-none">R$</span>
              <input className={inputClass + ' pl-11'} inputMode="numeric"
                value={form.salary}
                onChange={(e) => handleChange('salary', maskSalary(e.target.value))}
                placeholder="6.000" />
            </div>
          </div>

          {error && <p className="text-red-500 text-sm">{error}</p>}

          <div className="flex gap-3 pt-2">
            <button type="button" onClick={() => navigate('/company/dashboard')}
              className="flex-1 py-3 rounded-xl font-semibold border-2"
              style={{ borderColor: '#1B2A4A', color: '#1B2A4A' }}>
              Cancelar
            </button>
            <button type="submit" disabled={loading}
              className="flex-1 py-3 rounded-xl font-semibold text-white disabled:opacity-50"
              style={{ backgroundColor: '#1B2A4A' }}>
              {loading ? 'Publicando...' : 'Publicar vaga'}
            </button>
          </div>
        </form>
      </main>
    </div>
  );
}
