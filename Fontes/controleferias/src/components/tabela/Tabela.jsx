import * as React from 'react';
import { DataGrid, GridToolbar } from '@mui/x-data-grid';
import dayjs from 'dayjs';

export default function CustomLocaleTextGrid(props) {
    return (
        <div style={{ height: '100%', width: '100%' }}>
            <DataGrid
                rows={Array.from(props.colaborador).map(colab => (
                    {
                        id: colab.id,
                        Nome: colab.colaborador[0].sNome,
                        Equipe: colab.colaborador[0].equipe.sNome,
                        Data: dayjs(colab.dDataInicio).format("DD/MM/YYYY"),
                        Dias: colab.sDias,
                        Retorno: dayjs(colab.dDataFinal).format("DD/MM/YYYY"),
                        Comentario: colab.sComentario
                    }
                ))}
                columns={[
                    {
                        field: 'id',
                    },
                    {
                        field: 'Nome', width: 200
                    },
                    {
                        field: 'Equipe',
                    },
                    {
                        field: 'Data',
                    },
                    {
                        field: 'Dias',
                    },
                    {
                        field: 'Retorno',
                    },
                    {
                        field: 'Comentario', width: 200
                    },
                ]}
                localeText={{
                    toolbarDensity: 'Size',
                    toolbarDensityLabel: 'Size',
                    toolbarDensityCompact: 'Small',
                    toolbarDensityStandard: 'Medium',
                    toolbarDensityComfortable: 'Large',
                }}
                slots={{
                    toolbar: GridToolbar,
                }}
            />
        </div>
    );
}