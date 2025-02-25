import React, { useEffect, useState } from "react";
import { Table, Tabs, Tooltip, Input, Button } from "antd";
import type { ColumnsType } from "antd/es/table";

const { TabPane } = Tabs;

interface monitorsESD {
  id: string;
  serialNumber: string;
  description: string;
  statusJig: string;
  statusOperador: string;
}

interface MonitorProps {
  monitor: {
    positionSequence: number;
    monitorsESD: monitorsESD;
  };
  onMonitorTabActive: (active: boolean) => void; // Nova prop
  isEditing: boolean; // Nova prop para edição
}

interface LogData {
  key: string;
  message: string;
  date: string;
}

interface DataType {
  key: string;
  serialNumber: string;
  description: string;
  statusJig: string;
  statusOperador: string;
}

const truncateText = (text: string, maxLength: number) =>
  text.length > maxLength ? `${text.slice(0, maxLength)}...` : text;

const Monitor: React.FC<MonitorProps> = ({ monitor, onMonitorTabActive, isEditing }) => {
  const [editableData, setEditableData] = useState<monitorsESD>(monitor.monitorsESD);
  const monitorColumns: ColumnsType<DataType> = [
    {
      title: "Serial Number",
      dataIndex: "serialNumber",
      key: "serialNumber",
      render: (text: string) => (
        <Tooltip title={text}>
          {isEditing ? (
            <Input
              value={editableData.serialNumber}
              onChange={(e) => setEditableData({ ...editableData, serialNumber: e.target.value })}
            />
          ) : (
            <span className="ellipsis-text">{truncateText(text, 10)}</span>
          )}
        </Tooltip>
      ),
    },
    {
      title: "Description",
      dataIndex: "description",
      key: "description",
      render: (text: string) => (
        <Tooltip title={text}>
          {isEditing ? (
            <Input
              value={editableData.description}
              onChange={(e) => setEditableData({ ...editableData, description: e.target.value })}
            />
          ) : (
            <span className="ellipsis-text">{truncateText(text, 10)}</span>
          )}
        </Tooltip>
      ),
    },
    {
      title: "Status Jig",
      dataIndex: "statusJig",
      key: "statusJig",
      render: (text: string) => (
        <Tooltip title={text}>
          {isEditing ? (
            <Input
              value={editableData.statusJig}
              onChange={(e) => setEditableData({ ...editableData, statusJig: e.target.value })}
            />
          ) : (
            <span className="ellipsis-text">{truncateText(text, 10)}</span>
          )}
        </Tooltip>
      ),
    },
    {
      title: "Status Operador",
      dataIndex: "statusOperador",
      key: "statusOperador",
      render: (text: string) => (
        <Tooltip title={text}>
          {isEditing ? (
            <Input
              value={editableData.statusOperador}
              onChange={(e) => setEditableData({ ...editableData, statusOperador: e.target.value })}
            />
          ) : (
            <span className="ellipsis-text">{truncateText(text, 10)}</span>
          )}
        </Tooltip>
      ),
    },
  ];

  const monitorData: DataType[] = [
    {
      key: monitor.monitorsESD.id.toString(),
      serialNumber: monitor.monitorsESD.serialNumber,
      description: monitor.monitorsESD.description,
      statusJig: monitor.monitorsESD.statusJig,
      statusOperador: monitor.monitorsESD.statusOperador,
    },
  ];

  const logColumns: ColumnsType<LogData> = [
    {
      title: "Message",
      dataIndex: "message",
      key: "message",
      render: (text: string) => (
        <Tooltip title={text}>
          <span className="ellipsis-text">{truncateText(text, 10)}</span>
        </Tooltip>
      ),
    },
    {
      title: "Date",
      dataIndex: "date",
      key: "date",
    },
  ];

  const operatorLogs: LogData[] = [
    { key: "1", message: "Operador iniciou tarefa", date: "2024-10-09" },
    { key: "2", message: "Operador finalizou tarefa", date: "2024-10-09" },
  ];

  const jigLogs: LogData[] = [
    { key: "1", message: "Jig conectado", date: "2024-10-08" },
    { key: "2", message: "Jig desconectado", date: "2024-10-09" },
  ];

  const handleTabChange = (activeKey: string) => {
    onMonitorTabActive(activeKey === "1");
  };


  return (
    <div>
      <Tabs defaultActiveKey="1" onChange={handleTabChange}>
        <TabPane tab="Monitor" key="1">
          <Table columns={monitorColumns} dataSource={monitorData} pagination={false} />
        </TabPane>
        <TabPane tab="Log" key="2">
          <div style={{ display: "flex", gap: "16px" }}>
            <Table
              title={() => "Logs de Operador"}
              columns={logColumns}
              dataSource={operatorLogs}
              pagination={false}
              style={{ width: "50%" }}
            />
            <Table
              title={() => "Logs de Jigs"}
              columns={logColumns}
              dataSource={jigLogs}
              pagination={false}
              style={{ width: "50%" }}
            />
          </div>
        </TabPane>
      </Tabs>
    </div>
  );
};

export default Monitor;
