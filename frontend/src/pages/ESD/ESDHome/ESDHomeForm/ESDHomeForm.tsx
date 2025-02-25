import React, { useState, useEffect } from "react";
import {
  Typography,
  Box,
  Paper,
  FormControl,
  Modal,
  Button,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { getAllLines, getLine } from "../../../../api/linerApi";
import { getAllStations, getStation } from "../../../../api/stationApi";
import {
  getAllMonitors,
  createMonitor,
  getMonitor,
  deleteMonitor,
  updateMonitor,
} from "../../../../api/monitorApi";
import { getAllLinks, getLink } from "../../../../api/linkStationLine";

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

interface ESDHomeFormProps {
  open: boolean;
  handleClose: () => void;
  onSubmit: (mappedItem: MappedItem) => Promise<void>;
}

interface MappedItem {
  monitorEsdId: number;
  linkStationAndLineId: number;
}

interface Monitor {
  id: number;
  serialNumber: string;
}

interface Link {
  id: number;
}

const ESDHomeForm: React.FC<ESDHomeFormProps> = ({
  open,
  handleClose,
  onSubmit,
}) => {
  const { t } = useTranslation();
  const [allMonitors, setAllMonitors] = useState<Monitor[]>([]);
  const [allLinks, setAllLinks] = useState<Link[]>([]);
  const [mappedItem, setMappedItem] = useState<MappedItem>({
    monitorEsdId: 0,
    linkStationAndLineId: 0,
  });

  const handleChange = (e: SelectChangeEvent<number>) => {
    const { name, value } = e.target;
    setMappedItem((prev) => ({
      ...prev,
      [name]: Number(value), // Certifique-se de converter o valor para number
    }));
  };

  // const handleChange = (e: React.ChangeEvent<{ name?: string; value: unknown }>) => {
  //   const { name, value } = e.target;
  //   setMappedItem((prev) => ({
  //     ...prev,
  //     [name]: Number(value), // Certifique-se de converter o valor para number
  //   }));
  // };

  
  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      await onSubmit(mappedItem);
      handleClose();
    } catch (error) {
      console.error("Error creating bracelet:", error);
    }
  };

  useEffect(() => {
    const fetchData = async () => {
      try {
        const resultMonitors = await getAllMonitors();
        const resultLinks = await getAllLinks();
        setAllMonitors(resultMonitors);
        setAllLinks(resultLinks);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };
    fetchData();
  }, []);

  const handleMonitorSelect = async (id: number) => {
    try {
      await getMonitor(id);
    } catch (error) {
      console.error("Error fetching monitor details:", error);
    }
  };

  const handleLinkSelect = async (id: number) => {
    try {
      await getLink(id);
    } catch (error) {
      console.error("Error fetching link details:", error);
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
          {t("LINK_STATION_LINE.ADD_LINK_STATION_LINE", {
            appName: "App for Translations",
          })}
        </Typography>
        <Box component="form" onSubmit={handleSubmit} sx={{ mt: 2 }}>
          <FormControl fullWidth margin="normal" required>
            <InputLabel id="monitorEsdId">Monitor ESD</InputLabel>
            <Select
              labelId="monitorEsdId"
              id="monitorEsdId"
              name="monitorEsdId"
              value={mappedItem.monitorEsdId}
              onChange={(e) => {
                handleChange(e);
                handleMonitorSelect(Number(e.target.value));
              }}
              label="Monitor ESD"
            >
              {allMonitors.map((monitor) => (
                <MenuItem key={monitor.id} value={monitor.id}>
                  {monitor.serialNumber}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <FormControl fullWidth margin="normal" required>
            <InputLabel id="linkStationAndLineId">
              {t("LINK_STATION_LINE.TABLE.LINE", {
                appName: "App for Translations",
              })}
            </InputLabel>
            <Select
              labelId="linkStationAndLineId"
              id="linkStationAndLineId"
              name="linkStationAndLineId"
              value={mappedItem.linkStationAndLineId}
              onChange={(e) => {
                handleChange(e);
                handleLinkSelect(Number(e.target.value));
              }}
              label="Ligação"
            >
              {allLinks.map((link) => (
                <MenuItem key={link.id} value={link.id}>
                  {link.id}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <Box sx={{ mt: 2, display: "flex", justifyContent: "flex-end" }}>
            <Button
              type="submit"
              variant="contained"
              color="success"
              sx={{ mt: 2 }}
            >
              {t("LINK_STATION_LINE.DIALOG.SAVE", {
                appName: "App for Translations",
              })}
            </Button>
          </Box>
        </Box>
      </Paper>
    </Modal>
  );
};

export default ESDHomeForm;
