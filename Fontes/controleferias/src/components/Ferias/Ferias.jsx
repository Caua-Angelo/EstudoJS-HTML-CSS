'use client'
import TextField from '@mui/material/TextField';
import './Ferias.css'
import SelectAutoWidth from '../../components/select/Select';
import ValidationTextFields from '../../components/textInput/TextInput';
import SelectVariants from '../../components/select/Select';
import ResponsiveDatePickers from '../../components/dateInput/DateInput';
import MultilineTextFields from '../../components/multilineInput/MultilineInput';
import CheckIcon from '@mui/icons-material/Check';
import ShortcutIcon from '@mui/icons-material/Shortcut';
import { useEffect, useState } from 'react';
import dayjs from 'dayjs';
import Button from '../../components/button/button';
import useGlobalStore from '../../store/storeConfig';
import ComboBox from '../../components/textInput/TextInput';
import Snackbar from '@mui/material/Snackbar';
import Alert from '@mui/material/Alert';

export default function Ferias() {
    const [IdColaborador, setIdColaborador] = useState('')
    const [Nome, setNome] = useState('')
    const [Equipe, setEquipe] = useState('')
    const [Data, setData] = useState(dayjs(Date.now()))
    const [Dias, setDias] = useState('')
    const [DataFinal, setDataFinal] = useState(dayjs(Date.now()))
    const [Comentario, setComentario] = useState('')
    const [NomeError, setNomeError] = useState(false)
    const [EquipeError, setEquipeErro] = useState(false)
    const [DiasError, setDiasError] = useState(false)
    const [open, setOpen] = useState(false);
    const { ferias, setFerias, colaboradores, setColaboradores, setAlerta } = useGlobalStore()

    const handleClick = () => {
        setOpen(true);
    };

    const handleClose = (event, reason) => {
        if (reason === 'clickaway') {
            return;
        }

        setOpen(false);
    };

    let apiBaseUrl;

    if (import.meta.env === 'development') {
        apiBaseUrl = import.meta.env.API_BASE_URL_DES;
    } else if (import.meta.env === 'homologation') {
        apiBaseUrl = import.meta.env.API_BASE_URL_HOM;
    } else if (import.meta.env === 'production') {
        apiBaseUrl = import.meta.env.API_BASE_URL_PROD;
    } else {
        apiBaseUrl = 'https://localhost:7244/';
    }

    function limparCampos() {
        setNome('')
        setEquipe('')
        setData(dayjs(Date.now()))
        setDias('')
        setComentario('')
    }

    function validarCampos() {
        let validado = true

        if (Nome == "") {
            setNomeError(true)
            validado = false
        }
        else {
            setNomeError(false)
        }
        if (Equipe == "") {
            setEquipeErro(true)
            validado = false
        }
        else {
            setEquipeErro(false)
        }
        if (Dias == "") {
            setDiasError(true)
            validado = false
        }
        else {
            setDiasError(false)
        }
        return validado
    }

    function postColaboradores() {
        if (validarCampos() == false) {
            setAlerta(true, "Insira os campos informados!", "warning");
            return
        }
        let obj = {
            dDataInicio: Data.toISOString(),
            sDias: Dias.toString(),
            dDataFinal: DataFinal.toISOString(),
            sComentario: Comentario,
            colaboradorFerias: [
                {
                    colaboradorId: IdColaborador
                },
            ]
        }
        fetch(`${apiBaseUrl}Home/AdicionarFerias`,
            {
                method: 'post',
                headers: {
                    'Access-Control-Allow-Origin': '*',
                    'Content-Type': 'application/json; charset=utf-8'
                },
                body: JSON.stringify(obj)
            }).then(response => fetch(`${apiBaseUrl}Home/ConsultarFerias`,
                {
                    method: 'get',
                    headers: {
                        'Access-Control-Allow-Origin': '*',
                    }
                },
            )
                .then(r => r.json())
                .then(j =>
                    setFerias(j),
                    handleClick()
                ))
            .catch(erro => {
                setAlerta(true, "Ocorreu um erro ao inserir o colaborador!", "error");
            })

        limparCampos()
    }

    useEffect(() => {
        const novaData = new Date(Data)
        novaData.setDate(novaData.getDate() + Dias)
        setDataFinal(dayjs(novaData))
    }, [Dias])

    return (
        <div className="cards bordas w-[100vw] bg-black">
            <h2>Férias</h2>
            <div className='inputs'>
                <ComboBox value={Nome} equipevalue={setEquipe} idvalue={setIdColaborador} onChange={setNome} error={NomeError} />
                <SelectVariants onChange={setEquipe} value={Equipe} label='Equipe' equipe={true} error={EquipeError} />
                <ResponsiveDatePickers onChange={setData} value={Data} />
                <SelectVariants onChange={setDias} value={Dias} label='Dias' error={DiasError} />
                <ResponsiveDatePickers onChange={setDataFinal} disabled={true} value={DataFinal} />
            </div>
            <div>
                <MultilineTextFields onChange={setComentario} value={Comentario} />
            </div>
            <div className='btns'>
                <Button
                    span={'Cancelar'}
                    icon={<ShortcutIcon sx={{ transform: 'rotate(180deg)', transform: 'scaleX(-1)' }} />}
                    onclick={limparCampos}
                />
                <Button
                    span={'Salvar'}
                    icon={<CheckIcon />}
                    onclick={postColaboradores} />
            </div>
            <Snackbar
                open={open}
                autoHideDuration={5000}
                onClose={handleClose}
                message="This Snackbar will be dismissed in 5 seconds."
            >
                <Alert
                    onClose={handleClose}
                    severity="success"
                    variant="filled"
                    sx={{ width: '100%' }}
                >
                    Férias adicionada ao colaborador com sucesso!
                </Alert>
            </Snackbar>
        </div>
    )
}