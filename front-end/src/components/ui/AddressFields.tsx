import { useRef } from 'react';
import { formatCep, onlyDigits } from '../../utils/Masks';
import { fetchAddressByCep } from '../../services/cepService';
import { UFS } from '../../constants/options';
import { notifyError } from '../ui/Alerts';

export interface Address {
  zipCode: string;   // dígitos
  street: string;
  number: string;
  neighborhood: string;
  city: string;
  state: string;
  complement: string;
}

export const emptyAddress: Address = {
  zipCode: '', street: '', number: '', neighborhood: '',
  city: '', state: '', complement: '',
};

interface Props {
  value: Address;
  onChange: (address: Address) => void;
}

const inputClass = 'w-full border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 bg-white';
const labelStyle = { color: '#1B2A4A' };
const labelClass = 'block text-sm font-medium mb-1';

export default function AddressFields({ value, onChange }: Props) {
  const numberRef = useRef<HTMLInputElement>(null);
  const set = (field: keyof Address, v: string) => onChange({ ...value, [field]: v });

  const handleCepBlur = async () => {
    if (onlyDigits(value.zipCode).length !== 8) return;
    try {
      const result = await fetchAddressByCep(value.zipCode);
      if (!result) { notifyError('CEP não encontrado.'); return; }
      onChange({ ...value, ...result });
      numberRef.current?.focus();
    } catch {
      notifyError('Não foi possível buscar o CEP.');
    }
  };

  return (
    <>
      <div className="grid grid-cols-2 gap-3">
        <div>
          <label className={labelClass} style={labelStyle}>CEP</label>
          <input value={formatCep(value.zipCode)}
            onChange={(e) => set('zipCode', onlyDigits(e.target.value).slice(0, 8))}
            onBlur={handleCepBlur} placeholder="00000-000" className={inputClass} required />
        </div>
        <div>
          <label className={labelClass} style={labelStyle}>Rua</label>
          <input value={value.street} onChange={(e) => set('street', e.target.value)} className={inputClass} required />
        </div>
      </div>

      <div className="grid grid-cols-2 gap-3">
        <div>
          <label className={labelClass} style={labelStyle}>Número</label>
          <input ref={numberRef} value={value.number} onChange={(e) => set('number', e.target.value)} className={inputClass} required />
        </div>
        <div>
          <label className={labelClass} style={labelStyle}>Bairro</label>
          <input value={value.neighborhood} onChange={(e) => set('neighborhood', e.target.value)} className={inputClass} required />
        </div>
      </div>

      <div className="grid grid-cols-2 gap-3">
        <div>
          <label className={labelClass} style={labelStyle}>Cidade</label>
          <input value={value.city} onChange={(e) => set('city', e.target.value)} className={inputClass} required />
        </div>
        <div>
          <label className={labelClass} style={labelStyle}>UF</label>
          <select value={value.state} onChange={(e) => set('state', e.target.value)} className={inputClass} required>
            <option value="">UF</option>
            {UFS.map((uf) => <option key={uf} value={uf}>{uf}</option>)}
          </select>
        </div>
      </div>

      <div>
        <label className={labelClass} style={labelStyle}>Complemento (opcional)</label>
        <input value={value.complement} onChange={(e) => set('complement', e.target.value)} className={inputClass} />
      </div>
    </>
  );
}