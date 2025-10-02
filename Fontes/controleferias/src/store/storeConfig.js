import { create } from 'zustand';

const useGlobalStore = create((set) => ({
    colaboradores: {},
    setColaboradores: (newValue) => set({ colaboradores: newValue }),
    ferias: {},
    setFerias: (newValue) => set({ ferias: newValue }),
    alerta: false,
    mensagem: "",
    variante: "success",
    setAlerta: (alert, mens, variant) => set({ alerta: alert, mensagem: mens, variante: variant })
}));

export default useGlobalStore;