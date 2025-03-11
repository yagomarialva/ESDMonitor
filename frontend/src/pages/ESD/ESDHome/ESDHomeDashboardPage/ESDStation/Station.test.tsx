import { render, screen, fireEvent, waitFor } from "@testing-library/react"
import "@testing-library/jest-dom"
import Station from "./Station"
import signalRService from "../../../../../api/signalRService"
import { createMonitor } from "../../../../../api/monitorApi"
import { createStationMapper } from "../../../../../api/mapingAPI"
import React from "react"

// Mock the dependencies
jest.mock("../../../../../api/signalRService", () => ({
  startConnection: jest.fn(),
  stopConnection: jest.fn(),
  onReceiveAlert: jest.fn(),
}))

jest.mock("../../../../../api/monitorApi", () => ({
  createMonitor: jest.fn(),
  getMonitor: jest.fn(),
  updateMonitor: jest.fn(),
}))

jest.mock("../../../../../api/mapingAPI", () => ({
  createStationMapper: jest.fn(),
}))

// Mock react-router-dom
jest.mock("react-router-dom", () => ({
  useNavigate: () => jest.fn(),
}))

// Mock react-i18next
jest.mock("react-i18next", () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}))

const mockStationEntry = {
  station: {
    id: 1,
    linkStationAndLineID: 1,
    name: "Test Station",
    sizeX: 4,
    sizeY: 4,
  },
  linkStationAndLineID: 1,
  monitorsEsd: [
    {
      positionSequence: 0,
      monitorsEsd: {
        id: 1,
        serialNumberEsp: "TEST001",
        description: "Test Monitor 1",
        statusJig: "OK",
        statusOperador: "OK",
        linkStationAndLineID: 1,
      },
    },
  ],
}

describe("Station Component", () => {
  const mockOnUpdate = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
    ;(signalRService.startConnection as jest.Mock).mockResolvedValue(undefined)
  })

  it("renders without crashing", async () => {
    render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    await waitFor(() => {
      expect(signalRService.startConnection).toHaveBeenCalled()
    })
  })

  it("shows connection error alert when SignalR fails", async () => {
    ;(signalRService.startConnection as jest.Mock).mockRejectedValue(new Error("Connection failed"))

    render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    await waitFor(() => {
      expect(screen.getByText("Sem Conexão")).toBeInTheDocument()
    })
  })

  it("handles monitor creation successfully", async () => {
    const mockNewMonitor = {
      id: 2,
      serialNumberEsp: "TEST002",
      description: "New Monitor",
    }
    ;(createMonitor as jest.Mock).mockResolvedValue(mockNewMonitor)
    ;(createStationMapper as jest.Mock).mockResolvedValue({})

    render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    // Click on an empty cell to open creation modal
    const addIcon = screen.getByRole("img", { name: /plus-circle/i })
    fireEvent.click(addIcon)

    await waitFor(() => {
      expect(screen.getByRole("dialog")).toBeInTheDocument()
    })
  })

  it("updates monitor status when receiving SignalR alert", async () => {
    render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    await waitFor(() => {
      expect(signalRService.onReceiveAlert).toHaveBeenCalled()
    })

    const mockLog = {
      serialNumberEsp: "TEST001",
      status: 1,
      messageType: "operador",
      description: "Connected",
      timestamp: new Date().toISOString(),
    }

    // Simulate receiving a SignalR alert
    const onReceiveAlertCallback = (signalRService.onReceiveAlert as jest.Mock).mock.calls[0][0]
    onReceiveAlertCallback(mockLog)

    // Check if the status is updated (green icon should be present)
    const statusIcon = screen.getByRole("img", { name: /laptop|user/i })
    expect(statusIcon).toHaveStyle({ color: "#4caf50" })
  })

  it("handles monitor cell click and shows modal", async () => {
    render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    // Find and click the monitor cell
    const monitorCell = screen.getByText("TEST001")
    fireEvent.click(monitorCell)

    await waitFor(() => {
      expect(screen.getByRole("dialog")).toBeInTheDocument()
      expect(screen.getByText("TEST001")).toBeInTheDocument()
    })
  })

  it("updates group colors after inactivity period", async () => {
    jest.useFakeTimers()

    render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    // Fast-forward time by 5 seconds
    jest.advanceTimersByTime(5000)

    await waitFor(() => {
      const group = screen.getByTestId("cell-group-0")
      expect(group).toHaveStyle({ backgroundColor: "#f0f2f5" })
    })

    jest.useRealTimers()
  })

  it("cleans up SignalR connection on unmount", () => {
    const { unmount } = render(<Station stationEntry={mockStationEntry} onUpdate={mockOnUpdate} />)

    unmount()

    expect(signalRService.stopConnection).toHaveBeenCalled()
  })
})

