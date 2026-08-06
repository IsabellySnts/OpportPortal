import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { companyService } from '../../services/companyService';

export default function CompanySetupPage() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const [form, setForm] = useState({
    companyName: '',
    cnpj: '',
    sector: '',
    email: '',
    phoneNumber: '',
    zipCode: '',
    street: '',
    number: '',
    neighborhood: '',
    city: '',
    state: '',
    country: 'Brasil',
    complement: '',
  });

  const handleChange = (field: string, value: string) => {
    setForm(prev => ({ ...prev, [field]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const result = await companyService.create({
        ...form,
        complement: form.complement || undefined,
      });

      if (!result.isSuccess) {
        setError(result.message);
        return;
      }

      navigate('/company/dashboard');
    } catch {
      setError('Erro ao cadastrar empresa. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const inputClass = "w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white";
  const labelClass = "block text-sm font-medium mb-1";

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-2xl mx-auto">

        <div className="text-center mb-8">
          <div className="w-12 h-12 rounded-xl flex items-center justify-center mx-auto mb-3"
            style={{ backgroundColor: '#1B2A4A' }}>
            <span className="text-white font-bold text-xl">P</span>
          </div>
          <h1 className="text-2xl font-bold" style={{ color: '#1B2A4A' }}>
            Complete o cadastro da empresa
          </h1>
          <p className="text-gray-500">Preencha os dados para começar a publicar vagas</p>
        </div>

        <form onSubmit={handleSubmit} className="bg-white rounded-2xl shadow-sm p-8 space-y-4">

          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Nome da empresa</label>
            <input className={inputClass} required
              value={form.companyName}
              onChange={(e) => handleChange('companyName', e.target.value)}
              placeholder="Tech Solutions Ltda" />
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>CNPJ</label>
              <input className={inputClass} required
                value={form.cnpj}
                onChange={(e) => handleChange('cnpj', e.target.value)}
                placeholder="12345678000195" />
            </div>
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Ramo</label>
              <input className={inputClass} required
                value={form.sector}
                onChange={(e) => handleChange('sector', e.target.value)}
                placeholder="Tecnologia" />
            </div>
          </div>

          <div className="grid grid-cols-2 gap-4">
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>E-mail de contato</label>
              <input className={inputClass} type="email" required
                value={form.email}
                onChange={(e) => handleChange('email', e.target.value)}
                placeholder="contato@empresa.com" />
            </div>
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Telefone</label>
              <input className={inputClass} required
                value={form.phoneNumber}
                onChange={(e) => handleChange('phoneNumber', e.target.value)}
                placeholder="11999999999" />
            </div>
          </div>

          <div className="grid grid-cols-3 gap-4">
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>CEP</label>
              <input className={inputClass} required
                value={form.zipCode}
                onChange={(e) => handleChange('zipCode', e.target.value)}
                placeholder="00000000" />
            </div>
            <div className="col-span-2">
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Rua</label>
              <input className={inputClass} required
                value={form.street}
                onChange={(e) => handleChange('street', e.target.value)} />
            </div>
          </div>

          <div className="grid grid-cols-3 gap-4">
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Número</label>
              <input className={inputClass} required
                value={form.number}
                onChange={(e) => handleChange('number', e.target.value)} />
            </div>
            <div className="col-span-2">
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Bairro</label>
              <input className={inputClass} required
                value={form.neighborhood}
                onChange={(e) => handleChange('neighborhood', e.target.value)} />
            </div>
          </div>

          <div className="grid grid-cols-3 gap-4">
            <div className="col-span-2">
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Cidade</label>
              <input className={inputClass} required
                value={form.city}
                onChange={(e) => handleChange('city', e.target.value)} />
            </div>
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>UF</label>
              <input className={inputClass} required maxLength={2}
                value={form.state}
                onChange={(e) => handleChange('state', e.target.value.toUpperCase())}
                placeholder="SP" />
            </div>
          </div>

          <div>
            <label className={labelClass} style={{ color: '#1B2A4A' }}>Complemento (opcional)</label>
            <input className={inputClass}
              value={form.complement}
              onChange={(e) => handleChange('complement', e.target.value)} />
          </div>

          {error && <p className="text-red-500 text-sm">{error}</p>}

          <button type="submit" disabled={loading}
            className="w-full py-3 rounded-xl font-semibold text-white disabled:opacity-50"
            style={{ backgroundColor: '#1B2A4A' }}>
            {loading ? 'Cadastrando...' : 'Finalizar cadastro'}
          </button>
        </form>
      </div>
    </div>
  );
}