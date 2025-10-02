import api from "./api";

export const ConsultarColaborador = () => api.get("/Colaborador/ConsultarColaborador");
//export const getColaboradorById = (id) => api.get(`colaboradores/ConsultarColaboradorPorId/${id}`);
export const IncluirColaborador = (payload) => api.post("/Colaborador/IncluirColaborador", payload);
export const AlterarColaborador = (id, payload) => api.post(`/Colaborador/AlterarColaborador/${id}`, payload);
export const ExcluirColaborador = (id) => api.post(`/Colaborador/ExcluirColaborador/${id}`);
