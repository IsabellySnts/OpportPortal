import Swal from 'sweetalert2';

const NAVY = '#1B2A4A';
const RED = '#DC2626';

export async function confirmAction(options: {
  title?: string;
  text?: string;
  confirmText?: string;
  cancelText?: string;
  danger?: boolean;
}): Promise<boolean> {
  const result = await Swal.fire({
    title: options.title ?? 'Tem certeza?',
    text: options.text,
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: options.confirmText ?? 'Confirmar',
    cancelButtonText: options.cancelText ?? 'Voltar',
    confirmButtonColor: options.danger ? RED : NAVY,
    cancelButtonColor: '#9CA3AF',
    reverseButtons: true,
  });
  return result.isConfirmed;
}

const toast = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  timer: 3000,
  timerProgressBar: true,
});

export function notifySuccess(message: string) {
  toast.fire({ icon: 'success', title: message });
}

export function notifyError(message: string) {
  toast.fire({ icon: 'error', title: message });
}