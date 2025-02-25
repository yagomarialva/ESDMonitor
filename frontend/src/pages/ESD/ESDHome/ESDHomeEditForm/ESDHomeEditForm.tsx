import React, { useState, useEffect } from "react";
import {
  Typography,
  Box,
  Paper,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  Modal,
  TextField,
  Button,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { SelectChangeEvent } from "@mui/material";

interface Monitor {
  id: string;
  description: string;
  serialNumber: string;
  status: string;
  statusOperador: string;
  statusJig: string;
}

interface ESDHomeEditFormProps {
  open: boolean;
  handleClose: () => void;
  onSubmit: (monitor: Monitor) => Promise<void>;
  initialData?: Monitor | null; // Allow null
}

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  boxShadow: 24,
  p: 4,
};

const ESDHomeEditForm: React.FC<ESDHomeEditFormProps> = ({ open, handleClose, onSubmit, initialData }) => {
  const {
    t,
    i18n: { changeLanguage, language },
  } = useTranslation();
  const [currentLanguage, setCurrentLanguage] = useState(language);

  const handleChangeLanguage = () => {
    const newLanguage = currentLanguage === "en" ? "pt" : "en";
    setCurrentLanguage(newLanguage);
    changeLanguage(newLanguage);
  };

  const [monitor, setMonitor] = useState<Monitor>({
    id: initialData?.id || "",
    description: initialData?.description || "",
    serialNumber: initialData?.serialNumber || "",
    status: initialData?.status || "",
    statusOperador: initialData?.statusOperador || "",
    statusJig: initialData?.statusJig || "",
  });

  const [isDescriptionEditable, setIsDescriptionEditable] = useState(false);
  const [isDescriptionModified, setIsDescriptionModified] = useState(false); // Track description modification

  useEffect(() => {
    setIsDescriptionEditable(false);
    setIsDescriptionModified(false);
    if (initialData) {
      setMonitor(initialData);
    } else {
      setMonitor({
        id: "",
        description: "",
        serialNumber: "",
        status: "",
        statusOperador: "",
        statusJig: "",
      });
    }
  }, [initialData]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement> | SelectChangeEvent<string>) => {
    const { name, value } = e.target;

    if ('checked' in e.target) { // Check if the target has a checked property
      const checked = e.target.checked;
      setMonitor((prev) => ({
        ...prev,
        [name]: checked,
      }));
    } else {
      setMonitor((prev) => ({
        ...prev,
        [name]: value,
      }));
    }

    // Check if one of the selects was changed
    if (name === "statusOperador" || name === "statusJig") {
      setIsDescriptionEditable(true); // Enable description field
    }

    // Check if the description was modified
    if (name === "description") {
      setIsDescriptionModified(value !== initialData?.description);
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await onSubmit(monitor);
      handleClose();
    } catch (error) {
      console.error("Error creating or updating monitor:", error);
    }
  };

  return (
    <Modal
      open={open}
      onClose={handleClose}
      aria-labelledby="modal-modal-title"
      aria-describedby="modal-modal-description"
    >
      <Paper sx={style}>
        <Typography id="modal-modal-title" variant="h6" component="h2">
          {t("ESD_MONITOR.DIALOG.EDIT_MONITOR", {
            appName: "App for Translations",
          })}
        </Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
          <TextField
            required
            disabled
            fullWidth
            margin="normal"
            id="outlined-title"
            name="serialNumber"
            label={t("ESD_MONITOR.TABLE.NAME", {
              appName: "App for Translations",
            })}
            value={monitor.serialNumber}
            onChange={handleChange}
          />

          <FormControl fullWidth margin="normal" required>
            <InputLabel id="statusOperador">Status do Operador</InputLabel>
            <Select
              labelId="statusOperador"
              id="statusOperador"
              name="statusOperador"
              value={monitor.statusOperador}
              onChange={handleChange}
              label="Status do Operador"
            >
              <MenuItem value="PASS">Pass</MenuItem>
              <MenuItem value="FAIL">Fail</MenuItem>
            </Select>
          </FormControl>

          <FormControl fullWidth margin="normal" required>
            <InputLabel id="statusJig">Status do Jig</InputLabel>
            <Select
              labelId="statusJig"
              id="statusJig"
              name="statusJig"
              value={monitor.statusJig}
              onChange={handleChange}
              label="Status do Jig"
            >
              <MenuItem value="PASS">Pass</MenuItem>
              <MenuItem value="FAIL">Fail</MenuItem>
            </Select>
          </FormControl>

          <TextField
            required={isDescriptionEditable} // Becomes required after change
            disabled={!isDescriptionEditable} // Disabled until change
            fullWidth
            margin="normal"
            id="outlined-description"
            name="description"
            label="Descrição"
            helperText={
              isDescriptionEditable
                ? "Para alterar o status é necessário justificar o motivo."
                : ""
            }
            value={monitor.description}
            onChange={handleChange}
          />

          <Box sx={{ display: "flex", justifyContent: "flex-end", mt: 2 }}>
            <Button
              onClick={handleClose}
              variant="outlined"
              color="success"
              sx={{ mr: 2 }}
            >
              {t("ESD_MONITOR.DIALOG.CLOSE", {
                appName: "App for Translations",
              })}
            </Button>
            <Button
              type="submit"
              variant="contained"
              color="success"
              disabled={!isDescriptionModified} // Disabled until description is modified
            >
              {t("ESD_MONITOR.DIALOG.SAVE", {
                appName: "App for Translations",
              })}
            </Button>
          </Box>
        </Box>
      </Paper>
    </Modal>
  );
};

export default ESDHomeEditForm;
