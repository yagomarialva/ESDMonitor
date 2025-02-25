import React from "react";
import {
  Typography,
  Paper,
  Modal,
  TextField,
  Button,
  Box,
} from "@mui/material";
import { useTranslation } from "react-i18next";

const style = {
  position: "absolute" as const,
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  boxShadow: 24,
  p: 4,
};

interface ESDHomeModalProps {
  open: boolean;
  handleClose: () => void;
  produce?: {
    monitorsEsd?: {
      stationName?: string;
    };
    lineName?: string;
    status?: string;
    statusJig?: string;
    statusOperador?: string;
  };
}

const ESDHomeModal: React.FC<ESDHomeModalProps> = ({ open, handleClose, produce }) => {
  const { t } = useTranslation();

  return (
    <Modal
      open={open}
      onClose={handleClose}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Paper sx={style}>
        <Box component="form" noValidate autoComplete="off">
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            <TextField
              fullWidth
              disabled
              required
              label="Data"
              defaultValue={produce?.monitorsEsd?.stationName ?? "N/A"}
              margin="normal"
            />
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            <TextField
              fullWidth
              disabled
              required
              label="Hora"
              defaultValue={produce?.lineName ?? "N/A"}
              margin="normal"
            />
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            <TextField
              fullWidth
              disabled
              required
              label="Operador"
              defaultValue={produce?.status ?? "N/A"}
              margin="normal"
            />
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            <TextField
              fullWidth
              disabled
              required
              label="Logs"
              defaultValue={produce?.statusJig ?? "N/A"}
              margin="normal"
            />
          </Typography>
          <Typography id="modal-modal-description" sx={{ mt: 2 }}>
            <TextField
              fullWidth
              disabled
              required
              label="Logs"
              defaultValue={produce?.statusOperador ?? "N/A"}
              margin="normal"
            />
          </Typography>
        </Box>
        <Box sx={{ mt: 2, display: "flex", justifyContent: "flex-end" }}>
          <Button variant="contained" color="success" onClick={handleClose}>
            {t("ESD_OPERATOR.DIALOG.CLOSE", {
              appName: "App for Translations",
            })}
          </Button>
        </Box>
      </Paper>
    </Modal>
  );
};

export default ESDHomeModal;
