import React, { useEffect, useState } from "react";
import Line from "../ESDLine/Line";
import "./FactoryMap.css";
import {
  createLine,
  deleteLine,
  getAllLines,
} from "../../../../../api/linerApi";
import {
  createStation,
  deleteStation,
  getAllStations,
  getStationByName,
} from "../../../../../api/stationApi";
import { useTranslation } from "react-i18next";
import { createLink, deleteLink } from "../../../../../api/linkStationLine";
import { useNavigate } from "react-router-dom";
import {
  Button,
  Modal,
  message,
  Tooltip,
  Checkbox,
  Alert,
  Table,
  Input,
  Row,
  Col,
  Empty,
} from "antd";
import { ColumnsType } from "antd/es/table";
import {
  PlusOutlined,
  DeleteOutlined,
  ExclamationCircleOutlined,
} from "@ant-design/icons";
import signalRService from "../../../../../api/signalRService";
import {
  getAllStationMapper,
  getStationMapper,
} from "../../../../../api/mapingAPI";
import * as XLSX from "xlsx";

interface Station {
  id: number;
  linkStationAndLineID: number;
  name: string;
  sizeX: number;
  sizeY: number;
}

interface LogData {
  serialNumber: string;
  status: number;
  // Adicione outros campos conforme necessário
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
  station: Station;
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

interface FactoryMapProps {
  lines: Link[];
  onUpdate: () => void;
}

const { confirm } = Modal;

const FactoryMap: React.FC<FactoryMapProps> = ({ lines, onUpdate }) => {
  const navigate = useNavigate();
  const { t } = useTranslation();
  const [selectedLineId, setSelectedLineId] = useState<number | null>(null);
  const [selectedLinkId, setSelectedLinkId] = useState<number | null>(null);
  const [selectedStationsId, setSelectedStationsId] = useState<number | null>(
    null
  );
  const [isEditing, setIsEditing] = useState<boolean>(false);
  const [logs, setLogs] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [reload, setReload] = useState(false); // Estado para controlar o reload
  const [connectionStatus, setConnectionStatus] = useState<boolean>(true);
  const [viewMode, setViewMode] = useState<"map" | "data">("map");
  const [searchText, setSearchText] = useState<{ [key: string]: string }>({});

  const handleSearch = (value: string, dataIndex: string) => {
    setSearchText({ ...searchText, [dataIndex]: value });
  };

  const handleReset = (dataIndex: string) => {
    setSearchText({ ...searchText, [dataIndex]: "" });
  };
  const showMessage = (content: string, type: "success" | "error") => {
    message[type](content);
  };

  useEffect(() => {
    
    const connectToSignalR = async () => {
      if (reload) {
        onUpdate(); // Chama a atualização das linhas
        setReload(false); // Reseta o estado reload
      }
      try {
        await signalRService.startConnection();
        setError(null);
        setConnectionStatus(true);
      } catch (err) {
        setError("Falha ao conectar ao SignalR");
        setConnectionStatus(false);
        console.log("lines");
      } finally {
        setLoading(false);
      }
    };

    if (connectionStatus) {
      setTimeout(() => {
        connectToSignalR();
      }, 1000);
    }

    const checkConnection = setInterval(() => {
      const currentState = signalRService.getConnectionState();
      if (currentState !== "Connected" && connectionStatus) {
        setConnectionStatus(false);
      } else if (currentState === "Connected" && !connectionStatus) {
        setConnectionStatus(true);
      }
    }, 5000);

    signalRService.onReceiveAlert((log: LogData) => {
      if (![0, 1].includes(log.status)) {
        const updatedLog = {
          ...log,
          status: -1,
          description: "Monitor desconectado",
          lastUpdated: new Date().toISOString(),
        };
        setLogs((prevLogs) => [updatedLog, ...prevLogs].slice(0, 100));
      } else {
        setLogs((prevLogs) => [log, ...prevLogs].slice(0, 100));
      }
    });

    return () => {
      clearInterval(checkConnection);
      signalRService.stopConnection();
    };
  }, [reload, onUpdate, connectionStatus]);

  const handleError = (error: any) => {
    console.error("Erro:", error);
    if (error.message === "Request failed with status code 401") {
      showMessage(
        t("ESD_MONITOR.MAP_FACTORY.TOAST.EXPIRED_SESSION", {
          appName: "App for Translations",
        }),
        "error"
      );
      localStorage.removeItem("token");
      navigate("/");
    } else {
      showMessage(
        t("Erro ao criar estação", {
          appName: "App for Translations",
        }),
        "error"
      );
    }
  };

  const handleCreateLine = async () => {
    const randomLineName = `Linha ${Math.floor(Math.random() * 1000000)}`;
    try {
      const createdLine = await createLine({ name: randomLineName });
      const station = { name: createdLine.name, sizeX: 6, sizeY: 6 };
      const stationCreated = await createStation(station);
      const stationName = await getStationByName(stationCreated.name);

      const link = {
        ordersList: createdLine.id,
        lineID: createdLine.id,
        stationID: stationName.id,
        // lastUpdated: new Date().toISOString()
      };
      await createLink(link);
      onUpdate();
      showMessage(
        t("LINE.TOAST.CREATE_SUCCESS", { appName: "App for Translations" }),
        "success"
      );
    } catch (error: any) {
      handleError(error);
    }
  };

  const handleConfirmDelete = () => {
    confirm({
      title: "Confirmação de Exclusão",
      icon: <DeleteOutlined />,
      content: "Tem certeza de que deseja excluir esta linha?",
      className: "custom-modal",
      onOk: async () => {
        try {
          await deleteLink(selectedLinkId);
          await deleteLine(selectedLineId!);
          await deleteStation(selectedStationsId!);
          await getAllStationMapper();
          showMessage("Linha excluída com sucesso!", "success");
          onUpdate();
        } catch (error: any) {
          if (error.message === "Request failed with status code 404") {
            window.location.reload();
          }
          console.error("Erro ao excluir a linha:", error);
          showMessage("Erro ao excluir a linha.", "error");
        }
      },
      okButtonProps: {
        className: "custom-button",
      },
      cancelButtonProps: {
        className: "custom-cancel-button",
      },
    });
  };

  const handleLineChange = (link: Link) => {
    setSelectedLineId(link.line.id || null);
    setSelectedLinkId(link.stations[0]?.linkStationAndLineID || null);
    setSelectedStationsId(link.stations[0]?.station.id || null);
  };

  const exportToExcel = () => {
    const workbook = XLSX.utils.book_new();

    // Cria uma aba no Excel com os logs
    const logSheet = XLSX.utils.json_to_sheet(
      logs.map((log) => ({
        Data: log.lastUpdated,
        SerialNumber: log.serialNumberEsp,
        Descrição: log.description,
        Tipo: log.messageType,
      }))
    );
    XLSX.utils.book_append_sheet(workbook, logSheet, "Logs");

    // Salva o arquivo
    XLSX.writeFile(workbook, "LogsSignalR.xlsx");
  };

  const getColumnSearchProps = (dataIndex: string) => ({
    filterDropdown: ({
      setSelectedKeys,
      selectedKeys,
      confirm,
      clearFilters,
    }: any) => (
      <div style={{ padding: 8 }}>
        <Input
          placeholder={`Search ${dataIndex}`}
          value={selectedKeys[0]}
          onChange={(e) =>
            setSelectedKeys(e.target.value ? [e.target.value] : [])
          }
          onPressEnter={() => handleSearch(selectedKeys[0], dataIndex)}
          style={{ marginBottom: 8, display: "block" }}
        />
        <Button
          type="primary"
          onClick={() => handleSearch(selectedKeys[0], dataIndex)}
          icon="search"
          size="small"
          style={{ width: 90, marginRight: 8 }}
        >
          Search
        </Button>
        <Button
          onClick={() => handleReset(dataIndex)}
          size="small"
          style={{ width: 90 }}
        >
          Reset
        </Button>
      </div>
    ),
    filterIcon: (filtered: boolean) => (
      <span
        className="anticon anticon-search"
        style={{ color: filtered ? "#1890ff" : undefined }}
      />
    ),
    onFilter: (value: any, record: any) =>
      record[dataIndex].toString().toLowerCase().includes(value.toLowerCase()),
    render: (text: any) => (searchText[dataIndex] ? <span>{text}</span> : text),
  });

  const columns: ColumnsType<any> = [
    {
      title: "Data",
      dataIndex: "date",
      key: "date",
      ...getColumnSearchProps("date"),
      render: (text: string) =>
        new Date(text).toLocaleString("pt-BR", { hour12: false }),
    },
    {
      title: "Serial Number",
      dataIndex: "serialNumber",
      key: "serialNumber",
      ...getColumnSearchProps("serialNumber"),
    },
    {
      title: "Descrição",
      dataIndex: "description",
      key: "description",
      ...getColumnSearchProps("description"),
    },
    {
      title: "Tipo",
      dataIndex: "type",
      key: "type",
      ...getColumnSearchProps("type"),
    },
  ];

  return (
    <>
      {!connectionStatus ? (
        <div className="app-container">
          <div
            className="no-connection-message"
            style={{ textAlign: "center", padding: "20px" }}
          >
            <Alert
              message="Sem Conexão"
              description="Aparentemente, você está desconectado. Verifique sua conexão de internet e tente novamente."
              type="error"
              showIcon
              icon={
                <ExclamationCircleOutlined
                  style={{ fontSize: "24px", color: "#ffcc00" }}
                />
              }
              style={{ maxWidth: "600px", margin: "0 auto" }}
            />
          </div>
        </div>
      ) : (
        <>
          <div className="app-container">
            <header className="container-title">
              <h1>{t("LINE.TABLE_HEADER")}</h1>
              <div className="header-buttons">
                {isEditing && (
                  <>
                    <Button
                      type="link"
                      icon={<DeleteOutlined />}
                      disabled={!selectedLineId}
                      onClick={handleConfirmDelete}
                      className="white-background-button no-border remove-button-container"
                    >
                      {t("LINE.CONFIRM_DIALOG.DELETE_LINE", {
                        appName: "App for Translations",
                      })}
                    </Button>
                    <Button
                      type="link"
                      icon={<PlusOutlined />}
                      onClick={handleCreateLine}
                      className="white-background-button no-border add-button-container"
                    >
                      {t("LINE.ADD_LINE", { appName: "App for Translations" })}
                    </Button>
                  </>
                )}
                <Button
                  type="primary"
                  onClick={() => setIsEditing(!isEditing)}
                  className="white-background-button no-border enable-button-container"
                >
                  {isEditing ? "Finalizar Edição" : "Editar Linhas"}
                </Button>
              </div>
            </header>

            {viewMode === "map" ? (
              <div className="body">
                {lines.length === 0 ? (
                  <div className="no-lines-message">
                    <Empty
                      image={Empty.PRESENTED_IMAGE_SIMPLE}
                      description={
                        <span>Não há linhas para serem exibidas</span>
                      }
                    />
                  </div>
                ) : (
                  lines.map((line) => (
                    <div className="card" key={line.id} >
                    <div className="card-header">
                      {isEditing && (
                        <Tooltip
                          title={
                            line.stations.length > 1
                              ? "Não é possível excluir uma linha com mais de uma estação."
                              : ""
                          }
                        >
                          <Checkbox
                            checked={selectedLineId === line.line.id}
                            onChange={() => handleLineChange(line)}
                            disabled={line.stations.length > 1} // Desabilita se houver mais de uma estação
                          />
                        </Tooltip>
                      )}
                    </div>
                    <div className="card-body" >
                      <Line lineData={line} onUpdate={onUpdate} />
                    </div>
                  </div>
                  ))
                )}
              </div>
            ) : (
              <div className="data-view">
                <Button
                  onClick={exportToExcel}
                  type="primary"
                  style={{ marginTop: 16, backgroundColor: "#009B2D" }}
                >
                  Exportar para Excel
                </Button>
                {/* <Table
                  dataSource={logs.map((log, index) => ({
                    key: index,
                    date: log.lastUpdated,
                    serialNumber: log.serialNumberEsp,
                    description: log.description,
                    status: log.status,
                    type: log.messageType,
                  }))}
                  columns={columns}
                /> */}
              </div>
            )}
          </div>
        </>
      )}
    </>
  );
};

export default FactoryMap;
