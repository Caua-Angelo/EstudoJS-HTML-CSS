import * as React from 'react';
import Box from '@mui/material/Box';
import Alert from '@mui/material/Alert';
import IconButton from '@mui/material/IconButton';
import Collapse from '@mui/material/Collapse';
import Button from '@mui/material/Button';
import CloseIcon from '@mui/icons-material/Close';
import useGlobalStore from '../../store/storeConfig';

export default function TransitionAlerts() {
    const [open, setOpen] = React.useState(false);
    const { alerta, mensagem, variante, setAlerta } = useGlobalStore();

    React.useEffect(() => {
        setOpen(alerta)
    }, [alerta])

    return (
        <Box sx={{ width: '80%', position: 'absolute', top: '-15px', zIndex: 99 }}>
            <Collapse in={open}>
                <Alert
                    severity={variante}
                    variant='filled'
                    action={
                        <IconButton
                            aria-label="close"
                            color="inherit"
                            size="small"
                            onClick={() => {
                                setOpen(false);
                                setAlerta(false, "", "")
                            }}
                        >
                            <CloseIcon fontSize="inherit" />
                        </IconButton>
                    }
                    sx={{ mb: 2, mt: '7%' }}
                >
                    {mensagem}
                </Alert>
            </Collapse>
        </Box>
    );
}