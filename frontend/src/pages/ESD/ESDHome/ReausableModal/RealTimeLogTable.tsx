import React, { useState, useEffect } from "react";
import { Table, Card, Badge, Alert, Spin, Tooltip } from "antd";
import {
  CheckCircleOutlined,
  WarningOutlined,
  CloseCircleOutlined,
  InfoCircleOutlined,
} from "@ant-design/icons";
import signalRService from "../../../../api/signalRService"; // Ajuste o caminho de importação conforme necessário
import "./RealTimeLogTable.css"; // Importando o CSS
import { Button, DatePicker } from "antd"; // Para o seletor de data e botão
import * as XLSX from "xlsx"; // Para exportar dados para Excel

interface LogData {
  serialNumberEsp: string;
  serialNumber: string;
  status: number;
  description: string;
  messageType: string;
  timestamp: string;
  lastUpdated: string;
}

interface RealTimeLogTableProps {
  serialNumberFilter?: string;
  statusFilter?: number;
  tipo?: "jig" | "operador";
}

export default function RealTimeLogTable({
  serialNumberFilter,
  statusFilter,
  tipo,
}: RealTimeLogTableProps) {
  const [logs, setLogs] = useState<LogData[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [dateRange, setDateRange] = useState<[string | null, string | null]>([
    null,
    null,
  ]);

  useEffect(() => {
    const connectToSignalR = async () => {
      try {
        await signalRService.startConnection();
        setError(null);
      } catch (err) {
        setError("Falha ao conectar ao SignalR");
      } finally {
        setLoading(false);
      }
    };

    connectToSignalR();

    // Load logs from localStorage on component mount
    const storedLogs = localStorage.getItem("realTimeLogs");
    if (storedLogs) {
      setLogs(JSON.parse(storedLogs));
    }

    signalRService.onReceiveAlert((log: LogData) => {
      const updatedLog = {
        ...log,
        status: log.status === 0 ? 0 : log.status === 1 ? 1 : -1,
        description:
          log.status === 0 ? "Desconectado" : log.status === 1 ? "Conectado" : "Status desconhecido",
        lastUpdated: new Date().toISOString(),
      }
      // setLogs((prevLogs) => [updatedLog, ...prevLogs].slice(0, 100));
      setLogs((prevLogs) => {
        const newLogs = [updatedLog, ...prevLogs].slice(0, 100)
        localStorage.setItem("realTimeLogs", JSON.stringify(newLogs))
        return newLogs
      })
    });

    return () => {
      signalRService.stopConnection();
    };
  }, []);

  const getStatusBadge = (status: number) => {
    switch (status) {
      case 0:
        return <CloseCircleOutlined style={{ color: "red" }} />
      case 1:
        return <CheckCircleOutlined style={{ color: "green" }} />
      case -1:
        return <WarningOutlined style={{ color: "gray" }} />
      default:
        return <Badge status="processing" text="Unknown" />
    }
  }

  const getStatusHeaderBadge = (status: number) => {
    switch (status) {
      case 0:
        return <Badge status="error" style={{ color: "red", marginLeft: "20px" }} />
      case 1:
        return <Badge status="success" style={{ color: "green", marginLeft: "20px" }} />
      // case -1:
      //   return <Badge status="warning" style={{ marginLeft: "20px" }} />
      default:
        return <Badge status="processing" style={{ marginLeft: "20px" }} />
    }
  }


  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    const day = String(date.getDate()).padStart(2, "0");
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const year = date.getFullYear();
    const hours = String(date.getHours()).padStart(2, "0");
    const minutes = String(date.getMinutes()).padStart(2, "0");

    return `${day}/${month}/${year} ${hours}:${minutes}`;
  };

  useEffect(() => {
    localStorage.setItem("realTimeLogs", JSON.stringify(logs))
  }, [logs])

  const columns = [
    {
      title: "Data",
      dataIndex: "lastUpdated",
      key: "lastUpdated",
      width: 5,
      render: (lastUpdated: string) => (
        <Tooltip title={formatDate(lastUpdated)}>
          <span>{formatDate(lastUpdated)}</span>
        </Tooltip>
      ),
      sorter: (a: LogData, b: LogData) =>
        new Date(a.lastUpdated).getTime() - new Date(b.lastUpdated).getTime(),
    },
    {
      title: "Descrição",
      dataIndex: "description",
      key: "description",
      width: 5,
      render: (description: string) => (
        <Tooltip title={description}>
          <span>{description}</span>
        </Tooltip>
      ),
      sorter: (a: LogData, b: LogData) =>
        a.description.localeCompare(b.description),
    },
    {
      title: "Status",
      dataIndex: "status",
      key: "status",
      width: 5,
      render: (status: number) => (
        // console.log('status', status),
        <Tooltip title={status === 1 ? "Conectado" : status === 0 ? "Desconectado" : "Status desconhecido"}>
        <span>{getStatusBadge(status)}</span>
      </Tooltip>
      ),
      sorter: (a: LogData, b: LogData) => a.status - b.status,
    },
  ];

  const filteredLogs = logs.filter((log) => {
    const withinDateRange =
      dateRange[0] && dateRange[1]
        ? new Date(log.lastUpdated) >= new Date(dateRange[0]) &&
          new Date(log.lastUpdated) <= new Date(dateRange[1])
        : true;

    return (
      (serialNumberFilter
        ? log.serialNumberEsp.includes(serialNumberFilter)
        : true) &&
      (statusFilter !== undefined ? log.status === statusFilter : true) &&
      withinDateRange
    );
  });

  const getLastLogStatus = () => {
    const filteredLogsByType = logs.filter(
      (log) => log.messageType === "jig" || "operador"
    );
    const lastLog = filteredLogsByType[0];
    if (lastLog) {
      return getStatusHeaderBadge(lastLog.status);
    }
    return <Badge status="processing" style={{ marginLeft: "20px" }} />;
  };

  if (loading) {
    return (
      <div className="loading-container">
        <Spin size="large" />
      </div>
    );
  }

  const exportToExcel = () => {
    const workbook = XLSX.utils.book_new();
    const worksheet = XLSX.utils.json_to_sheet(filteredLogs);
    XLSX.utils.book_append_sheet(workbook, worksheet, "Logs");
    XLSX.writeFile(workbook, "logs.xlsx");
  };

  const handleDateFilter = (dates: any) => {
    if (dates) {
      const [start, end] = dates;
      setDateRange([
        start ? start.startOf("day").toISOString() : null,
        end ? end.endOf("day").toISOString() : null,
      ]);
    } else {
      setDateRange([null, null]);
    }
  };

  return (
    <Card
      title={
        <div className="card-title-container">
          <span>Logs</span>
          {getLastLogStatus()}
        </div>
      }
      className="card-container"
    >
      <div className="table-controls">
        <DatePicker.RangePicker
          onChange={handleDateFilter}
          format="DD/MM/YYYY"
          style={{ marginRight: "10px" }}
        />
        {/* <Button onClick={exportToExcel}  style={{ backgroundColor: "#009B2D" }} type="primary">
          Exportar para Excel
        </Button> */}
      </div>
      {error && (
        <Alert
          message="Error"
          description={error}
          type="error"
          showIcon
          className="error-alert"
        />
      )}
      <Table
        columns={columns}
        dataSource={filteredLogs}
        rowKey={(record, index) => index!.toString()}
        pagination={{
          pageSize: 5,
          showSizeChanger: false,
          pageSizeOptions: [],
        }}
      />
    </Card>
  );
}
