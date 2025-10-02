import * as React from 'react';
import TextField from '@mui/material/TextField';
import Autocomplete from '@mui/material/Autocomplete';
import useGlobalStore from '../../store/storeConfig';

export default function ComboBox(props) {
    const { colaboradores } = useGlobalStore()

    function listaColab(colabs) {
        let arraycolab = []
        Array.from(colabs).map(colab => {
            let obj = {
                id: colab.id,
                label: colab.sNome,
                equipe: colab.equipe.sNome
            }
            arraycolab.push(obj)
        })
        return arraycolab
    }

    return (
        <Autocomplete
            value={props.value}
            isOptionEqualToValue={(option, value) => option?.value === value?.value}
            onChange={(e, newValue) => {
                props.onChange(newValue?.label)
                props.idvalue(newValue?.id)
                props.equipevalue(newValue?.equipe)
            }}
            disablePortal
            id="combo-box-demo"
            options={listaColab(colaboradores)}
            sx={props.error ? { width: 250, border: '1px solid red', borderRadius: '5px' } : { width: 250 }}
            renderInput={(params) => <TextField {...params} label="Colaborador" />}
        />
    );
}