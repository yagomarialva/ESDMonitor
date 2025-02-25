import React, { useState, ChangeEvent } from "react";
import { useTranslation } from "react-i18next";
import { Modal, Tooltip, Input, Table, Button } from "antd";
import type { ColumnsType } from "antd/es/table";
import { LaptopOutlined } from "@mui/icons-material";
import "./MonitorForm.css"; // CSS customizado

interface Monitor {
  serialNumberEsp: string;
  description: string;
  statusOperador: string;
  statusJig: string;
}

interface MonitorFormProps {
  open: boolean;
  handleClose: () => void;
  onSubmit: (monitor: Monitor) => Promise<void>;
  type: "operador" | "jig"; // Nova prop adicionada
}

const MonitorForm: React.FC<MonitorFormProps> = ({
  open,
  handleClose,
  onSubmit,
  type 
}) => {
  const { t } = useTranslation();

  // Estado do monitor
  const [monitor, setMonitor] = useState<Monitor>({
    serialNumberEsp: "",
    description: "",
    statusOperador: "",
    statusJig: "",
  });

  const [error, setError] = useState<string>("");
  const [errorName, setErrorName] = useState<string>("");

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setMonitor((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async () => {
    setError("");
    setErrorName("");

    try {
      await onSubmit(monitor);
      handleClose();
    } catch (error) {
      console.error("Error creating monitor:", error);
    }
  };

  const handleCloseModal = () => {
    setError("");
    setErrorName("");
    // Reseta os valores do monitor quando o modal é fechado
    setMonitor({
      serialNumberEsp: "",
      description: "",
      statusOperador: "",
      statusJig: "",
    });
    handleClose();
  };

  const monitorColumns: ColumnsType<Monitor> = [
    {
      title: t("Número de Série"),
      dataIndex: "serialNumberEsp",
      key: "serialNumberEsp",
      render: () => (
        <Tooltip title={monitor.serialNumberEsp}>
          <Input
            name="serialNumberEsp"
            value={monitor.serialNumberEsp}
            onChange={handleChange}
            required
          />
        </Tooltip>
      ),
    },
    {
      title: t("Descrição"),
      dataIndex: "description",
      key: "description",
      render: () => (
        <Tooltip title={monitor.description}>
          <Input
            name="description"
            value={monitor.description}
            onChange={handleChange}
            required
          />
        </Tooltip>
      ),
    },
  ];

  return (
    <Modal
      open={open}
      onCancel={handleCloseModal}
      footer={null}
      title={
        <div className="modal-title-container-add">
          <LaptopOutlined className="icon-add-monitor" />
          <span className="ellipsis-text">{type === "operador" ? "Adicionar Operador" : "Adicionar Jig"}</span>
        </div>
      }
      className="modal-container"
      closable={false} // Remove o botão X de fechar
      bodyStyle={{ border: "none" }} // Remove bordas do corpo do modal
      style={{ border: "none" }} // Remove bordas do modal
    >
      <Table
        columns={monitorColumns}
        dataSource={[monitor]} // Fonte de dados sincronizada
        pagination={false} // Desativa paginação
        rowKey={() => "monitor-row"}
        style={{ marginBottom: "16px" }} // Espaço entre tabela e botões
      />
      <div className="button-container-monitor-to-add">
        <Button
          onClick={handleCloseModal}
          className="submit-button-cancel-monitor"
        >
          {t("ESD_TEST.DIALOG.CLOSE", { appName: "App for Translations" })}
        </Button>
        <Button
          type="primary"
          onClick={handleSubmit}
          className="submit-button-add-monitor"
        >
          {t("ESD_TEST.DIALOG.SAVE", { appName: "App for Translations" })}
        </Button>
      </div>
    </Modal>
  );
};

export default MonitorForm;
