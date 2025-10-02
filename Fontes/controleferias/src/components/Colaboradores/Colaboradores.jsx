'use client'
import './Colaboradores.css'
import EnhancedTable from '../../components/tabela/Tabela'
import { useEffect, useState } from 'react'
import useGlobalStore from '../../store/storeConfig'

export function Colaboradores() {
    const { ferias, setFerias, colaboradores, setColaboradores, setAlerta } = useGlobalStore()

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

    useEffect(() => {
        fetch(`${apiBaseUrl}Home`, {
            method: 'GET',
            headers: {
                'Access-Control-Allow-Origin': '*',
            }
        })
            .then(resp => {
                if (!resp.ok) {
                    throw Error(resp.statusText);
                }
                return resp.json();
            })
            .then(json => {
                setColaboradores(json);
            })
            .catch(error => {
                setAlerta(true, "Ocorreu um erro ao fazer a solicitação do servidor!", "error");
            });
    }, []);

    useEffect(() => {
        fetch(`${apiBaseUrl}Home/ConsultarFerias`, {
            method: 'GET',
            headers: {
                'Access-Control-Allow-Origin': '*',
            }
        })
            .then(resp => {
                if (!resp.ok) {
                    throw Error(resp.statusText);
                }
                return resp.json();
            })
            .then(json => {
                setFerias(json);
            })
            .catch(error => {
                setAlerta(true, "Ocorreu um erro ao fazer a solicitação do servidor!", "error");
            });
    }, []);

    return (
        <div className="cards ferias">
            <div className='tabela'>
                <EnhancedTable colaborador={ferias} />
            </div>
        </div>
    )
}