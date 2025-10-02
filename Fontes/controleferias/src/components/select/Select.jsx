import * as React from 'react';
import Box from '@mui/material/Box';
import InputLabel from '@mui/material/InputLabel';
import MenuItem from '@mui/material/MenuItem';
import FormControl from '@mui/material/FormControl';
import Select from '@mui/material/Select';
import useGlobalStore from '../../store/storeConfig';

export default function BasicSelect(props) {
    const { setAlerta } = useGlobalStore()
    const [equipes, setEquipes] = React.useState([])

    function listaDias() {
        var lista = []
        for (let i = 5; i <= 30; i++) {
            lista.push(i)
        }
        return lista
    }

    React.useEffect(() => {
        fetch('https://localhost:7244/Home/ConsultarEquipe', {
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
                setEquipes(json);
            })
            .catch(error => {
                setAlerta(true, "Ocorreu um erro ao fazer a consulta das equipes!", "error");
            });
    }, []);

    return (
        <Box sx={{ minWidth: 120 }}>
            <FormControl fullWidth>
                <InputLabel id="demo-simple-select-label">{props.label}</InputLabel>
                <Select
                    labelId="demo-simple-select-label"
                    id="demo-simple-select"
                    label={props.label}
                    sx={{ width: '250px' }}
                    onChange={e => props.onChange(e.target.value)}
                    value={props.value}
                    error={props.error}
                    disabled={props.equipe ? true : false}
                >
                    {props.equipe ?
                        Array.from(equipes).map(equipes => (
                            <MenuItem key={equipes.id} value={equipes.sNome}>{equipes.sNome}</MenuItem>
                        ))
                        :
                        listaDias().map(n => (
                            <MenuItem
                                key={n}
                                value={n}
                            >{n}</MenuItem>
                        ))
                    }
                </Select>
            </FormControl>
        </Box>
    );
}