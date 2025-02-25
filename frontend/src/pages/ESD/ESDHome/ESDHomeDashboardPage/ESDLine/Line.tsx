import React, { useState, useEffect } from "react";
import Station from "../ESDStation/Station";
import "./Line.css"; // Importando o CSS
import { Alert, Snackbar } from "@mui/material";
import {
  createStation,
  deleteStation,
  getAllStations,
  getStationByName,
} from "../../../../../api/stationApi";
import { createLink, deleteLink } from "../../../../../api/linkStationLine";
import { useNavigate } from "react-router-dom";
import RemoveCircleOutlineOutlinedIcon from "@mui/icons-material/RemoveCircleOutlineOutlined";
import AddCircleOutlineRoundedIcon from "@mui/icons-material/AddCircleOutlineRounded";
import { Button, Tooltip } from "antd"; // Importa o botão do Ant Design

interface StationLine {
  id: number;
  linkStationAndLineID: number;
  name: string;
  sizeX: number;
  sizeY: number;
}

interface MonitorDetails {
  id: number;
  serialNumberEsp: string;
  description: string;
  statusJig: string;
  statusOperador: string;
  linkStationAndLineID: number;
}

interface Monitor {
  positionSequence: number;
  monitorsEsd: MonitorDetails;
}

interface StationEntry {
  station: StationLine;
  linkStationAndLineID: number;
  monitorsEsd: Monitor[];
}

interface LineData {
  id?: number;
  name: string;
}

interface Link {
  id: number;
  line: LineData;
  stations: StationEntry[];
}

interface ESDStationProps {
  lineData: Link;
  onUpdate: () => void; // Nova prop para receber a função onUpdate
}

type SnackbarSeverity = "success" | "error";

const Line: React.FC<ESDStationProps> = ({ lineData, onUpdate }) => {
  const [createdLinks, setCreatedLinks] = useState<Set<number>>(new Set()); // Estado para controlar os links criados
  const [stations, setStations] = useState<StationEntry[]>(
    lineData.stations || []
  ); // Estado para armazenar as estações
  const [selectedStationId, setSelectedStationId] = useState<number | null>(
    null
  );
  const [isEditing, setIsEditing] = useState<boolean>(false); // Estado para controlar o modo de edição
  const [state, setState] = useState({
    snackbarMessage: "", // Mensagem do Snackbar
    snackbarOpen: false,
    snackbarSeverity: "success" as SnackbarSeverity, // Severidade do Snackbar
  });

  // Atualiza o estado com os tipos corretos
  const handleStateChange = (changes: Partial<typeof state>) => {
    setState((prevState) => ({ ...prevState, ...changes }));
  };

  const navigate = useNavigate();

  const showSnackbar = (
    message: string,
    severity: SnackbarSeverity = "success"
  ) => {
    handleStateChange({
      snackbarMessage: message,
      snackbarSeverity: severity,
      snackbarOpen: true, // Abrir o Snackbar
    });
  };

  const fetchStations = async () => {
    try {
      const stationsData = await getAllStations();
      setStations(stationsData); // Atualiza o estado com as estações
    } catch (error: any) {
      console.error("Erro ao buscar as estações:", error);
      localStorage.removeItem("token");
      navigate("/");
      if (error.message === "Request failed with status code 401") {
        localStorage.removeItem("token");
        navigate("/");
      }
    }
  };

  useEffect(() => {
    fetchStations();
  }, []); // Agora ele será chamado quando `stations` mudar

  const handleStationSelect = (stationEntry: StationEntry) => {
    console.log("station", stationEntry);
    setSelectedStationId(stationEntry.station.id);
    const selectedInfo = {
      line: lineData.line,
      station: stationEntry.station,
      linkId: stationEntry.linkStationAndLineID,
    };
    return selectedInfo;
  };

  const handleCreateStation = async () => {
    const randomStationName = `Estação ${Math.floor(
      Math.random() * 100000000000
    )}`;
    const linkId = lineData.id;
    const station = {
      name: randomStationName,
      sizeX: 6,
      sizeY: 6,
    };

    if (createdLinks.has(linkId)) {
      return; // Se o link já foi criado, não faz nada
    }

    try {
      const newStation = await createStation(station);
      const stationName = await getStationByName(newStation.name);
      const link = {
        ordersList: stationName.id,
        lineID: lineData.line.id,
        stationID: stationName.id,
        // lastUpdated: new Date().toISOString()
      };

      await createLink(link);

      // Adiciona a nova estação ao estado
      setStations((prevStations) => [
        ...prevStations,
        {
          station: {
            id: stationName.id,
            name: stationName.name,
            sizeX: newStation.sizeX,
            sizeY: newStation.sizeY,
            linkStationAndLineID: linkId,
          },
          linkStationAndLineID: linkId,
          monitorsEsd: [], // Adiciona um array vazio de monitores, se necessário
        },
      ]);

      // Chama a função onUpdate após a criação da nova estação
      onUpdate();

      // Mostra o Snackbar de sucesso
      showSnackbar("Estação criada com sucesso!", "success");
    } catch (error: any) {
      if (error.message === "Request failed with status code 401") {
        localStorage.removeItem("token");
        navigate("/");
      }
      console.error("Erro ao criar e mapear a estação:", error);

      // Mostra o Snackbar de erro
      showSnackbar("Erro ao criar a estação.", "error");
    }
  };

  const handleDeleteStation = async () => {
    if (
      !lineData?.stations ||
      lineData.stations.length === 0 ||
      !selectedStationId
    ) {
      console.error(
        "Lista de estações ou ID de estação selecionada não estão definidos."
      );
      return;
    }

    const selectedStation = lineData.stations.find(
      (stationEntry) => stationEntry?.station?.id === selectedStationId
    );

    if (!selectedStation) {
      console.error("Estação selecionada não encontrada.");
      return;
    }

    // Verificação para garantir que a estação não seja a única da linha
    if (lineData.stations.length <= 1) {
      showSnackbar("Não é possível excluir a única estação da linha.", "error");
      return;
    }

    try {
      const selectedInfo = handleStationSelect(selectedStation);
      console.log('selectedStation', selectedStation.monitorsEsd)
      await deleteLink(selectedInfo.linkId);

      await deleteStation(selectedInfo.station.id);

      // Chama a função onUpdate após a exclusão da estação
      onUpdate();

      if (selectedStation.monitorsEsd.length > 0) {
        showSnackbar("Não é possível excluir estação", "error");
      }

      if (selectedStation.monitorsEsd.length == 0) {
        showSnackbar("Estação excluída com sucesso!", "success");
      }
      // Mostra o Snackbar de sucesso
      
    } catch (error: any) {
      console.error("Erro ao excluir a estação:", error);

      // Mostra o Snackbar de erro
      showSnackbar("Erro ao excluir a estação.", "error");

      if (error.message === "Request failed with status code 401") {
        localStorage.removeItem("token");
        navigate("/");
      }
    }
  };

  return (
    <>
      <div className="container">
        <div className="button-for-edit">
          <div className="add-button-container-stations">
            {isEditing && (
              <>
                <Button
                  type="primary"
                  shape="round"
                  icon={<RemoveCircleOutlineOutlinedIcon />}
                  size="small"
                  onClick={handleDeleteStation}
                  disabled={!isEditing}
                  className="white-background-button no-border"
                />
                <Button
                  type="primary"
                  shape="round"
                  icon={<AddCircleOutlineRoundedIcon />}
                  size="small"
                  disabled={!isEditing}
                  onClick={handleCreateStation}
                  className="white-background-button no-border"
                />
              </>
            )}
            <Button
              type="primary"
              shape="round"
              onClick={() => setIsEditing(!isEditing)} // Alterna o modo de edição
              className="white-background-button no-border color-button"
            >
              {isEditing ? "Finalizar Edição" : "Editar Estações"}
            </Button>
          </div>
        </div>
        <div className="line-container">
          <div className="line-content">
            <div className="esd-line-container">
            <div className="stations-container">
              {lineData.stations.map((stationEntry) => (
                <div key={stationEntry.station.id}>
                  {isEditing && ( // Renderiza os botões de rádio apenas no modo de edição
                    <div className="radio-button">
                      <Tooltip
                        title={
                          stationEntry.monitorsEsd.length > 0
                            ? "Não é possível excluir uma estação com monitores inseridos a ela."
                            : ""
                          }
                      >
                          <input
                            type="radio"
                            name="selectedStation"
                            id={`station-${stationEntry.station.id}`} // ID único para cada estação
                            value={stationEntry.station.id}
                            onChange={() => handleStationSelect(stationEntry)}
                            checked={ selectedStationId === stationEntry.station.id}
                            title={`Select station ${stationEntry.station.name}`}
                            disabled={stationEntry.monitorsEsd.length > 0}
                          />
                      </Tooltip>
                    </div>
                  )}
                  <div className="stations-wrapper">
                  <Station stationEntry={stationEntry} onUpdate={onUpdate} />
                  </div>
                </div>
              ))}
            </div>
            </div>
          </div>

          <Snackbar
            open={state.snackbarOpen}
            autoHideDuration={6000}
            onClose={() => handleStateChange({ snackbarOpen: false })}
            anchorOrigin={{ vertical: "top", horizontal: "right" }}
            className={`ant-snackbar ant-snackbar-${state.snackbarSeverity}`}
          >
            <Alert
              onClose={() => handleStateChange({ snackbarOpen: false })}
              severity={state.snackbarSeverity}
              sx={{ width: "100%" }}
            >
              {state.snackbarMessage}
            </Alert>
          </Snackbar>
        </div>
      </div>
    </>
  );
};

export default Line;
