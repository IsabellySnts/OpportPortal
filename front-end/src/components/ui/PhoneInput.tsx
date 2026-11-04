import { formatPhone, onlyDigits } from '../../utils/Masks';

interface PhoneInputProps {
  value: string; // só dígitos
  onChange: (digits: string) => void;
  placeholder?: string;
  required?: boolean;
}

export default function PhoneInput({ value, onChange, placeholder = '(11) 99999-9999', required }: PhoneInputProps) {
  return (
    <input
      type="tel"
      value={formatPhone(value)}
      onChange={(e) => onChange(onlyDigits(e.target.value).slice(0, 11))}
      placeholder={placeholder}
      required={required}
      className="w-full border border-gray-200 rounded-xl px-4 py-3 focus:outline-none focus:ring-2 bg-white"
    />
  );
}