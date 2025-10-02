// src/pages/ColaboradorPage.jsx
import { useState, useEffect } from "react";
import { ConsultarColaborador, IncluirColaborador} from "../services/colaboradorService.js";


export default function ColaboradorPage() {
  const [Consultarcolaboradores, setColaboradores] = useState([]);
  const [sNome, setNome] = useState("");
  const [EquipeId, setEquipeId] = useState("");
  const [erro, setErro] = useState("");

  // Carregar colaboradores ao montar a página
  useEffect(() => {
    ConsultarColaborador()
      .then((res) => setColaboradores(res.data))
      .catch((err) => console.error("Erro ao consultar colaboradores:", err));
  }, []);

  // Função para adicionar colaborador
  const FuncIncluirColaborador = async () => {
    const payload = { sNome, EquipeId: Number(EquipeId) };
    console.log("Payload enviado:", payload);
  
    try {
      const res = await IncluirColaborador(payload);
      console.log("Resposta da API:", res.data);
  
      alert("Colaborador cadastrado com sucesso!");
      setNome("");
      setEquipeId("");
      setErro("");
  
      // Atualiza a lista sem recarregar a página
      setColaboradores([...Consultarcolaboradores, payload]);
     
    } catch (err) {
      //console.log("Erro completo do Axios:", err);
      console.log("Erro completo do Axios:", err.response.data);
  
      // Tenta pegar a mensagem do backend
      let mensagemErro = err.response.data;
  
      if (err.response?.data) {
        // Para seu backend, a propriedade é 'Mensagem'
        mensagemErro = err.response.data.Mensagem || mensagemErro;
      } else if (err.message) {
        // Caso seja outro tipo de erro (ex.: network error)
        mensagemErro = err.message;
      }
  
      console.log("Mensagem exibida:", mensagemErro);
      setErro(mensagemErro);
    }
  };
  const FuncConsultarColaborador = async () => {
    try {
      const res = await ConsultarColaborador();
      console.log("Colaboradores Recebidos:", res.data);
  
      setColaboradores(res.data); 
      setErro("");
    } catch (err) {
      //console.log("Erro completo do Axios:", err);
      console.log("Erro completo do Axios:", err.response.data);
  
      // Tenta pegar a mensagem do backend
      let mensagemErro = err.response.data;
  
      if (err.response?.data) {
        // Para seu backend, a propriedade é 'Mensagem'
        mensagemErro = err.response.data.Mensagem || mensagemErro;
      } else if (err.message) {
        // Caso seja outro tipo de erro (ex.: network error)
        mensagemErro = err.message;
      }
  
      console.log("Mensagem exibida:", mensagemErro);
      setErro(mensagemErro);
    }
  };


  return (
    <div style={{ padding: "50px" }}>
      <h3>Incluir Colaborador</h3>

      <div style={{ marginBottom: "10px" }}>
        <input
          type="text"
          value={sNome}
          onChange={(e) => setNome(e.target.value)}
          placeholder="Nome"
        />
      </div>

      <div style={{ marginBottom: "10px" }}>
        <input
          type="number"
          value={EquipeId}
          onChange={(e) => setEquipeId(e.target.value)}
          placeholder="Id da equipe"
        />
      </div>

      <button onClick={FuncIncluirColaborador}>Cadastrar</button>
      {erro && (
  <p style={{ color: "red", marginTop: "10px" }}>
    {erro}
  </p>
  
)}

      <h4 style={{ marginTop: "20px" }}>Colaboradores cadastrados:</h4>
      <ul>
        {Consultarcolaboradores.map((c, index) => (
          <li key={index}>
            {c.sNome} - Equipe ID: {c.EquipeId}
          </li>
        ))}
      </ul>
    </div>
  );
  <button onClick={ConsultarColaborador}>ConsultarColaboradores</button>
}
