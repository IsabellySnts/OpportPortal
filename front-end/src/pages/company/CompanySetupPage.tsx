import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { companyService } from '../../services/companyService';
import AddressFields, { Address, emptyAddress } from '../../components/ui/AddressFields';
import PhoneInput from '../../components/ui/PhoneInput';
import { SECTORS } from '../../constants/options';
import { formatCnpj, onlyDigits } from '../../utils/Masks';
import { notifySuccess, notifyError } from '../../components/ui/Alerts';
import logo from '../../assets/graduate.png';


export default function CompanySetupPage() {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [loadingData, setLoadingData] = useState(true);
  const [isEdit, setIsEdit] = useState(false);
  const [companyId, setCompanyId] = useState<number | null>(null);

  const [form, setForm] = useState({
    companyName: '',
    cnpj: '',
    sector: '',
    email: '',
    phoneNumber: '',
  });

  const [address, setAddress] = useState<Address>(emptyAddress);

  // Se a empresa já existe, entra em modo edição e pré-preenche os dados
  useEffect(() => {
    (async () => {
      try {
        const result = await companyService.getMe();
        if (result.isSuccess && result.data) {
          const c = result.data;
          setIsEdit(true);
          setCompanyId(c.id);
          setForm({
            companyName: c.companyName ?? '',
            cnpj: c.cnpj ?? '',
            sector: c.sector ?? '',
            email: c.email ?? '',
            phoneNumber: c.phoneNumber ?? '',
          });
          setAddress({
            ...emptyAddress,
            zipCode: c.zipCode ?? '',
            street: c.street ?? '',
            number: c.number ?? '',
            neighborhood: c.neighborhood ?? '',
            city: c.city ?? '',
            state: c.state ?? '',
            complement: c.complement ?? '',
          });
        }
      } catch {
        // sem empresa ainda -> modo criação
      } finally {
        setLoadingData(false);
      }
    })();
  }, []);

  const handleChange = (field: string, value: string) =>
    setForm((prev) => ({ ...prev, [field]: value }));

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);

    const payload = {
      ...form,
      ...address,
      country: 'Brasil',
      complement: address.complement || undefined,
    };

    try {
      const result = isEdit && companyId != null
        ? await companyService.update(companyId, payload)
        : await companyService.create(payload);

      if (!result.isSuccess) {
        notifyError(result.message ?? 'Não foi possível salvar os dados da empresa.');
        return;
      }

      notifySuccess(isEdit ? 'Dados atualizados com sucesso!' : 'Empresa cadastrada com sucesso!');
      navigate(isEdit ? '/my-profile' : '/company/dashboard');
    } catch {
      notifyError('Erro ao salvar os dados. Tente novamente.');
    } finally {
      setLoading(false);
    }
  };

  const inputClass = 'w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white';
  const labelClass = 'block text-sm font-medium mb-1';

  if (loadingData) {
    return (
      <div className="min-h-screen bg-gray-50 flex items-center justify-center">
        <p className="text-gray-500">Carregando...</p>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-50 py-8 px-4">
      <div className="max-w-2xl mx-auto">

        <div className="text-center mb-8">
          <div className="w-16 h-16 rounded-2xl flex items-center justify-center mx-auto mb-6"
            style={{ backgroundColor: '#FFFFFF' }}>
            <img src={logo} alt="Logo" className="w-6 h-6 object-contain" />
          </div>
          <h1 className="text-2xl font-bold" style={{ color: '#1B2A4A' }}>
            {isEdit ? 'Editar dados da empresa' : 'Complete o cadastro da empresa'}
          </h1>
          <p className="text-gray-500">
            {isEdit ? 'Atualize as informações da sua empresa' : 'Preencha os dados para começar a publicar vagas'}
          </p>
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
                value={formatCnpj(form.cnpj)}
                onChange={(e) => handleChange('cnpj', onlyDigits(e.target.value).slice(0, 14))}
                placeholder="00.000.000/0000-00" />
            </div>
            <div>
              <label className={labelClass} style={{ color: '#1B2A4A' }}>Ramo</label>
              <select className={inputClass} required
                value={form.sector}
                onChange={(e) => handleChange('sector', e.target.value)}>
                <option value="">Selecione</option>
                {SECTORS.map((s) => <option key={s} value={s}>{s}</option>)}
              </select>
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
              <PhoneInput value={form.phoneNumber} onChange={(v) => handleChange('phoneNumber', v)} required />
            </div>
          </div>

          {/* Endereço reutilizável (CEP preenche Rua/Bairro/Cidade/UF) */}
          <AddressFields value={address} onChange={setAddress} />

          <div className="flex gap-3 pt-2">
            {isEdit && (
              <button type="button" onClick={() => navigate('/my-profile')}
                className="flex-1 py-3 rounded-xl font-semibold border-2"
                style={{ borderColor: '#1B2A4A', color: '#1B2A4A' }}>
                Cancelar
              </button>
            )}
            <button type="submit" disabled={loading}
              className="flex-1 py-3 rounded-xl font-semibold text-white disabled:opacity-50"
              style={{ backgroundColor: '#1B2A4A' }}>
              {loading
                ? (isEdit ? 'Salvando...' : 'Cadastrando...')
                : (isEdit ? 'Salvar alterações' : 'Finalizar cadastro')}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
