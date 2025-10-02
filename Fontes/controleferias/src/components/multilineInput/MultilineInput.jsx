import * as React from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';

export default function MultilineTextFields(props) {
    return (
        <Box
            component="form"
            sx={{
                '& .MuiTextField-root': { margin: '0px 17px 0px 17px', width: '97.7%' },
            }}
            noValidate
            autoComplete="off"
        >
            <div>
                <TextField
                    id="outlined-multiline-static"
                    label="Comentário"
                    multiline
                    rows={4}
                    onChange={e => props.onChange(e.target.value)}
                    value={props.value}
                />
            </div>
        </Box>
    );
}