import { render, screen, fireEvent, waitFor } from "@testing-library/react"
import "@testing-library/jest-dom"
import ReusableModal from "./ReusableModal"
import { BrowserRouter } from "react-router-dom"
import * as monitorApi from "../../../../api/monitorApi"
import React from "react"

// Mock the API calls
jest.mock("../../../../api/monitorApi", () => ({
  getMonitor: jest.fn(),
  deleteMonitor: jest.fn(),
  getMonitorLogs: jest.fn(),
}))

// Mock the react-i18next hook
jest.mock("react-i18next", () => ({
  useTranslation: () => ({ t: (key: string) => key }),
}))

const mockMonitor = {
  positionSequence: "1",
  monitorsESD: {
    id: 1,
    description: "Test Monitor",
    serialNumberEsp: "SN123",
    statusOperador: "OK",
    statusJig: "OK",
  },
}

const defaultProps = {
  visible: true,
  onClose: jest.fn(),
  onEdit: jest.fn(),
  onDelete: jest.fn(),
  onSubmit: jest.fn(),
  title: "Test Modal",
  monitor: mockMonitor,
  onUpdate: jest.fn(),
}

describe("ReusableModal", () => {
  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders the modal with correct title", () => {
    render(
      <BrowserRouter>
        <ReusableModal {...defaultProps} />
      </BrowserRouter>,
    )
    expect(screen.getByText("Test Modal")).toBeInTheDocument()
  })

  it("displays monitor information correctly", () => {
    render(
      <BrowserRouter>
        <ReusableModal {...defaultProps} />
      </BrowserRouter>,
    )
    expect(screen.getByText("SN123")).toBeInTheDocument()
    expect(screen.getByText("Test Monitor")).toBeInTheDocument()
  })

  it("handles edit mode correctly", async () => {
    render(
      <BrowserRouter>
        <ReusableModal {...defaultProps} />
      </BrowserRouter>,
    )

    const editButton = screen.getByRole("img", { name: "edit" })
    fireEvent.click(editButton)

    await waitFor(() => {
      expect(screen.getByDisplayValue("SN123")).toBeInTheDocument()
      expect(screen.getByDisplayValue("Test Monitor")).toBeInTheDocument()
    })
  })

  it("handles delete confirmation", async () => {
    ;(monitorApi.getMonitor as jest.Mock).mockResolvedValue({ id: 1 })
    ;(monitorApi.deleteMonitor as jest.Mock).mockResolvedValue({})

    render(
      <BrowserRouter>
        <ReusableModal {...defaultProps} />
      </BrowserRouter>,
    )

    const deleteButton = screen.getByRole("img", { name: "delete" })
    fireEvent.click(deleteButton)

    // Confirm deletion
    const okButton = await screen.findByText("OK")
    fireEvent.click(okButton)

    await waitFor(() => {
      expect(monitorApi.deleteMonitor).toHaveBeenCalledWith(1)
      expect(defaultProps.onUpdate).toHaveBeenCalled()
      expect(defaultProps.onDelete).toHaveBeenCalled()
      expect(defaultProps.onClose).toHaveBeenCalled()
    })
  })

  it("switches to log tab and fetches logs", async () => {
    ;(monitorApi.getMonitorLogs as jest.Mock).mockResolvedValue([
      { messageType: "operator", message: "Operator log" },
      { messageType: "jig", message: "Jig log" },
    ])

    render(
      <BrowserRouter>
        <ReusableModal {...defaultProps} />
      </BrowserRouter>,
    )

    const logTab = screen.getByText("Log")
    fireEvent.click(logTab)

    await waitFor(() => {
      expect(monitorApi.getMonitorLogs).toHaveBeenCalledWith("SN123")
    })
  })
})

