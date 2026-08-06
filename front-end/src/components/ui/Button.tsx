import { ButtonHTMLAttributes, ReactNode } from 'react';

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  children: ReactNode;
  variant?: 'primary' | 'secondary' | 'outline' | 'danger';
  fullWidth?: boolean;
}

export default function Button({
  children,
  variant = 'primary',
  fullWidth = false,
  className = '',
  ...props
}: ButtonProps) {
  const variants = {
    primary: { backgroundColor: '#1B2A4A', color: 'white', border: 'none' },
    secondary: { backgroundColor: '#F5A623', color: 'white', border: 'none' },
    outline: { backgroundColor: 'white', color: '#1B2A4A', border: '2px solid #1B2A4A' },
    danger: { backgroundColor: 'white', color: '#DC2626', border: '1px solid #DC2626' },
  };

  return (
    <button
      {...props}
      className={`py-3 px-5 rounded-xl font-semibold transition-opacity disabled:opacity-50 ${fullWidth ? 'w-full' : ''} ${className}`}
      style={variants[variant]}
    >
      {children}
    </button>
  );
}