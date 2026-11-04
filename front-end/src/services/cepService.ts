export interface AddressFromCep {
  street: string;
  neighborhood: string;
  city: string;
  state: string;
}

export async function fetchAddressByCep(cep: string): Promise<AddressFromCep | null> {
  const clean = cep.replace(/\D/g, '');
  if (clean.length !== 8) return null;

  const response = await fetch(`https://viacep.com.br/ws/${clean}/json/`);
  const data = await response.json();
  if (data.erro) return null;

  return {
    street: data.logradouro || '',
    neighborhood: data.bairro || '',
    city: data.localidade || '',
    state: data.uf || '',
  };
}