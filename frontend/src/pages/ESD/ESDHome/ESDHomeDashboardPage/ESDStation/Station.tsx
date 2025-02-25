import React, { useEffect, useState, useCallback, useRef } from "react";
import {
  Tooltip,
  message,
  Modal,
  Card,
  Button,
  Row,
  Col,
  Typography,
  Alert,
} from "antd";
import {
  PlusCircleOutlined,
  LaptopOutlined,
  UserOutlined,
  ToolOutlined,
  ExclamationCircleOutlined,
} from "@ant-design/icons";
import "./Station.css";
import { useTranslation } from "react-i18next";
import { useNavigate } from "react-router-dom";
import {
  createMonitor,
  getMonitor,
  updateMonitor,
} from "../../../../../api/monitorApi";
import { createStationMapper } from "../../../../../api/mapingAPI";
import ReusableModal from "../../ReausableModal/ReusableModal";
import MonitorForm from "../../MonitorForm/MonitorForm";
import signalRService from "../../../../../api/signalRService";

interface Station {
  id?: number
  linkStationAndLineID: number
  name: string
  sizeX: number
  sizeY: number
}

interface LogData {
  serialNumberEsp: any
  serialNumber: string
  status: number
  description: string
  messageType: string
  timestamp: string
  lastUpdated: string
}

interface MonitorDetails {
  id: number
  serialNumberEsp: string
  description: string
  statusJig: string
  statusOperador: string
  linkStationAndLineID: number
}

interface Monitor {
  positionSequence: number
  monitorsEsd: MonitorDetails
}

interface StationEntry {
  station: Station
  linkStationAndLineID: number
  monitorsEsd: Monitor[]
}

interface StationProps {
  stationEntry: StationEntry
  onUpdate: () => void
}

type AlertType = "success" | "info" | "warning" | "error"

const Station: React.FC<StationProps> = ({ stationEntry, onUpdate }) => {
  const { station, monitorsEsd } = stationEntry
  const { t } = useTranslation()
  const navigate = useNavigate()

  const [modalText, setModalText] = useState<string>("-")
  const [modalTitleText, setModalTitleText] = useState<string>("-")
  const [modalIndexText, setModalIndexTitleText] = useState<number | null>(null)

  const [operatorStatuses, setOperatorStatuses] = useState<{
    [key: string]: boolean
  }>({})
  const [jigStatuses, setJigStatuses] = useState<{ [key: string]: boolean }>({})
  const [modalVisible, setModalVisible] = useState(false)
  const [openModal, setOpenModal] = useState(false)
  const [selectedMonitor, setSelectedMonitor] = useState<any | null>(null)

  const [error, setError] = useState<string | null>(null)
  const [iconToUse, seticonToUse] = useState<any | null>(null)
  const [iconTypeToUse, seticonTypeToUse] = useState<any | null>(null)
  const [loading, setLoading] = useState(true)
  const [logs, setLogs] = useState<LogData[]>([])
  const [globalColorOperador, setglobalColorOperador] = useState<any | null>(null)
  const [globalColorJig, setglobalColorJig] = useState<any | null>(null)
  const [monitorStatuses, setMonitorStatuses] = useState<{
    [key: string]: number
  }>({})

  const [connectionStatus, setConnectionStatus] = useState(true)

  const [alertState, setAlertState] = useState({
    message: "",
    type: "success" as AlertType,
    show: false,
  })

  const [groupLastUpdate, setGroupLastUpdate] = useState<{ [key: number]: number }>({})
  const [groupColors, setGroupColors] = useState<{ [key: number]: string }>({})
  const intervalRef = useRef<NodeJS.Timeout | null>(null)
  const [monitorLastUpdate, setMonitorLastUpdate] = useState<{ [key: string]: number }>({})

  const showAlert = useCallback((message: string, type: AlertType = "success") => {
    setAlertState({
      message,
      type,
      show: true,
    })
    setTimeout(() => {
      setAlertState((prev) => ({ ...prev, show: false }))
    }, 6000)
  }, [])

  const handleOpenModal = () => setOpenModal(true)
  const handleCloseModal = () => setOpenModal(false)
  const handleModalClose = () => setModalVisible(false)

  const handleCreateMonitor = async (monitor: any) => {
    try {
      const result = await createMonitor(monitor)
      const selectedCell = {
        monitorEsdId: result.id,
        linkStationAndLineId: selectedMonitor.stationInfo.linkStationAndLineID,
        positionSequence: selectedMonitor.index,
      }

      await createStationMapper(selectedCell)

      onUpdate()
      setOpenModal(false)
      showAlert(`Monitor ${result.serialNumberEsp} adicionado com sucesso!`, "success")
    } catch (error: any) {
      console.error("Erro ao criar monitor:", error)
      showAlert(`Monitor não foi adicionado!`, "error")
      if (error.message === "Request failed with status code 401") {
        localStorage.removeItem("token")
        navigate("/")
      }
    }
  }

  useEffect(() => {
    const connectToSignalR = async () => {
      try {
        await signalRService.startConnection()
        setError(null)
        setConnectionStatus(true)
        showAlert("Conexão com SignalR restabelecida!", "success")
      } catch (err) {
        console.error("Erro ao conectar ao SignalR", err)
        setError("Falha ao conectar ao SignalR")
        setConnectionStatus(false)
        showAlert("Conexão com SignalR perdida. Tentando reconectar...", "error")
      } finally {
        setLoading(false)
      }
    }

    connectToSignalR()

    const handleReceiveAlert = (log: LogData) => {
      const isConnected = log.status === 1
      setMonitorStatuses((prevStatuses) => ({
        ...prevStatuses,
        [log.serialNumberEsp]: isConnected ? 1 : 0,
      }))

      setLogs((prevLogs) => [log, ...prevLogs].slice(0, 100))
      if (log.status === 0) {
        showAlert(`Erro no monitor ${log.serialNumberEsp}: ${log.messageType}`, "error")
      }

      const monitorIndex = monitorsEsd.findIndex((m) => m.monitorsEsd.serialNumberEsp === log.serialNumberEsp)
      if (monitorIndex !== -1) {
        const groupIndex = Math.floor(monitorIndex / 4)
        setGroupLastUpdate((prev) => ({
          ...prev,
          [groupIndex]: Date.now(),
        }))
        // Não resetamos a cor do grupo aqui, isso será feito na função checkGroupUpdates
      }
      setMonitorLastUpdate((prev) => ({
        ...prev,
        [log.serialNumberEsp]: Date.now(),
      }))
    }

    signalRService.onReceiveAlert(handleReceiveAlert)

    return () => {
      signalRService.stopConnection()
    }
  }, [monitorsEsd, showAlert])

  const checkGroupUpdates = useCallback(() => {
    const now = Date.now()
    const newGroupColors = { ...groupColors }
    let hasChanges = false

    Object.entries(groupLastUpdate).forEach(([groupIndex, lastUpdate]) => {
      const groupMonitors = monitorsEsd.slice(Number(groupIndex) * 4, (Number(groupIndex) + 1) * 4)
      const allInactive = groupMonitors.every(
        (monitor) => now - (monitorLastUpdate[monitor.monitorsEsd.serialNumberEsp] || 0) > 1000,
      )

      if (allInactive) {
        newGroupColors[Number(groupIndex)] = "#f0f2f5" // Cor cinza para grupos sem atualização
        hasChanges = true
      } else {
        newGroupColors[Number(groupIndex)] = "" // Volta para a cor normal
        hasChanges = true
      }
    })

    if (hasChanges) {
      setGroupColors(newGroupColors)
    }
  }, [groupLastUpdate, groupColors, monitorsEsd, monitorLastUpdate])

  useEffect(() => {
    intervalRef.current = setInterval(checkGroupUpdates, 5000)

    return () => {
      if (intervalRef.current) {
        clearInterval(intervalRef.current)
      }
    }
  }, [checkGroupUpdates])

  const checkMonitorUpdates = useCallback(() => {
    const now = Date.now()
    const newMonitorStatuses = { ...monitorStatuses }
    let hasChanges = false

    Object.entries(monitorLastUpdate).forEach(([serialNumberEsp, lastUpdate]) => {
      if (now - lastUpdate > 30000) {
        newMonitorStatuses[serialNumberEsp] = 3 // 0 representa o estado "sem atualização"
        hasChanges = true
      }
    })

    if (hasChanges) {
      setMonitorStatuses(newMonitorStatuses)
    }
  }, [monitorLastUpdate, monitorStatuses])

  useEffect(() => {
    const intervalId = setInterval(checkMonitorUpdates, 5000)

    return () => clearInterval(intervalId)
  }, [checkMonitorUpdates])

  const cells = new Array(16).fill(null)

  monitorsEsd.forEach((monitor) => {
    cells[monitor.positionSequence] = monitor
  })

  const handleEditCellChange = async (params: any) => {
    try {
      const monitorView = await getMonitor(selectedMonitor.cell.serialNumberEsp)
      const updatedResult = {
        id: monitorView.id,
        serialNumberEsp: params.serialNumberEsp,
        description: params.description,
      }
      const result = await updateMonitor(updatedResult)
      showAlert(
        t("ESD_MONITOR.TOAST.UPDATE_SUCCESS", {
          appName: "App for Translations",
        }),
        "success",
      )
      onUpdate()
      return result
    } catch (error: any) {
      showAlert(t("ESD_MONITOR.TOAST.TOAST_ERROR", { appName: "App for Translations" }), "error")
      if (error.message === "Request failed with status code 401") {
        message.error("Sessão Expirada.")
        localStorage.removeItem("token")
        navigate("/")
      }
    }
  }

  const handleCellClick = (cell: any | null, index: number, stationInfo: any) => {
    const selectedCell = {
      cell: cell
        ? cell.monitorsEsd
        : {
            serialNumberEsp: "N/A",
            description: "Célula vazia",
            stationInfo,
          },
      index,
      stationInfo,
    }
    setSelectedMonitor(selectedCell)
    setModalText(selectedCell.cell.description)
    setModalTitleText(selectedCell.cell.serialNumberEsp)
    setModalIndexTitleText(index)
  }

  const handleDelete = () => {
    showAlert(
      t("ESD_MONITOR.TOAST.DELETE_SUCCESS", {
        appName: "App for Translations",
      }),
      "success",
    )
  }

  const getStatusIcon = (isConnected: boolean) => {
    return isConnected ? "#4caf50" : "#f44336"
  }

  const getIconType = (status: string, serialNumberEsp: string, groupIndex: number) => {
    const monitorStatus = monitorStatuses[serialNumberEsp]
    let color

    if (groupColors[groupIndex] === "#d9d9d9") {
      color = "#d9d9d9" // Cor cinza para o grupo inteiro se não houver atualização
    } else {
      color = monitorStatus === 1 ? "#4caf50" : monitorStatus === 0 ? "#f44336" : "#bfbfbf"
    }

    switch (status) {
      case "jig":
        return <LaptopOutlined style={{ color }} />
      case "operador":
        return <UserOutlined style={{ color }} />
      default:
        return <LaptopOutlined style={{ color: "#d9d9d9" }} />
    }
  }

  return (
    <>
      {!connectionStatus ? (
        <div className="no-connection-message" style={{ textAlign: "center", padding: "20px" }}>
          <Alert
            message="Sem Conexão"
            description="Aparentemente, você está desconectado. Verifique sua conexão de internet e tente novamente."
            type="error"
            showIcon
            icon={<ExclamationCircleOutlined style={{ fontSize: "24px", color: "#ffcc00" }} />}
            style={{ maxWidth: "600px", margin: "0 auto" }}
          />
        </div>
      ) : (
        <div className="card-grid">
          {Array.from({ length: Math.ceil(cells.length / 4) }).map((_, groupIndex) => {
            const group = cells.slice(groupIndex * 4, groupIndex * 4 + 4) // Agrupa 4 itens por vez

            return (
              <div
                key={groupIndex}
                className={`cell-group triangle-layout`}
              >
                {group.map((cell, index) => (
                  <div
                    key={index}
                    className={`icon-container ${
                      [0, 4].includes(groupIndex * 4 + index)
                        ? "top-cell-odd"
                        : [8, 12].includes(groupIndex * 4 + index)
                          ? "top-cell"
                          : "base-cell"
                    }`}
                    onClick={() => handleCellClick(cell, groupIndex * 4 + index, stationEntry)}
                  >
                    {cell ? (
                      <Tooltip title={cell.monitorsEsd.serialNumberEsp}>
                        <div className="computer-icon" onClick={() => setModalVisible(true)}>
                          <div className="status-indicators">
                            {getIconType(
                              index === 0 ? "operador" : "jig",
                              cell.monitorsEsd.serialNumberEsp,
                              groupIndex,
                            )}
                          </div>
                        </div>
                      </Tooltip>
                    ) : (
                      <div className="add-icon" onClick={handleOpenModal}>
                        <PlusCircleOutlined
                          className="cell-icon"
                          style={{
                            color: connectionStatus ? undefined : "#d9d9d9",
                          }}
                        />
                      </div>
                    )}
                  </div>
                ))}
              </div>
            )
          })}
        </div>
      )}

      {alertState.show && (
        <Alert
          message={alertState.message}
          type={alertState.type}
          showIcon
          closable
          onClose={() => setAlertState((prev) => ({ ...prev, show: false }))}
          style={{
            backgroundColor: "white",
            position: "fixed",
            top: 16,
            right: 16,
            zIndex: 1000,
          }}
        />
      )}

      <ReusableModal
        visible={modalVisible}
        onClose={handleModalClose}
        onEdit={() => console.log("Editar")}
        onDelete={handleDelete}
        onUpdate={onUpdate}
        title={modalTitleText}
        monitor={{
          positionSequence: selectedMonitor?.index,
          monitorsESD: {
            id: selectedMonitor?.index,
            serialNumberEsp: modalTitleText,
            description: selectedMonitor?.cell.description,
            statusJig: "TRUE",
            statusOperador: "FALSE",
          },
        }}
        onSubmit={handleEditCellChange}
      />

      <MonitorForm
        open={openModal}
        handleClose={handleCloseModal}
        onSubmit={handleCreateMonitor}
        type={[0, 4, 8, 12].includes(selectedMonitor?.index) ? "operador" : "jig"}
      />
    </>
  )
}

export default Station

