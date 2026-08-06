import { InputHTMLAttributes } from 'react';

interface InputProps extends InputHTMLAttributes<HTMLInputElement> {
  label?: string;
}

export default function Input({ label, className = '', ...props }: InputProps) {
  return (
    <div>
      {label && (
        <label className="block text-sm font-medium mb-1" style={{ color: '#1B2A4A' }}>
          {label}
        </label>
      )}
      <input
        {...props}
        className={`w-full border border-gray-200 rounded-xl px-4 py-2.5 focus:outline-none focus:ring-2 bg-white ${className}`}
      />
    </div>
  );
}