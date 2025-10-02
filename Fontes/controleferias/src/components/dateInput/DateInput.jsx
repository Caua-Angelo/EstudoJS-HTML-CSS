import * as React from 'react';
import dayjs from 'dayjs';
import { DemoContainer, DemoItem } from '@mui/x-date-pickers/internals/demo';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import 'dayjs/locale/en-gb';

export default function ResponsiveDatePickers(props) {
    return (
        <LocalizationProvider
            dateAdapter={AdapterDayjs}
            adapterLocale={'en-gb'}>
            {props.disabled ?
                <DatePicker
                    sx={{ position: 'relative', top: 16 }}
                    disabled
                    label="Data de Retorno"
                    onChange={e => props.onChange(e)}
                    value={dayjs(props.value)} />
                :
                <DatePicker
                    sx={{ position: 'relative', top: 16 }}
                    label="Data de Início"
                    onChange={e => props.onChange(e)}
                    value={dayjs(props.value)}
                />

            }
        </LocalizationProvider>
    );
}