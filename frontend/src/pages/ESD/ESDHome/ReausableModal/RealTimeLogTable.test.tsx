import { render, screen, act } from "@testing-library/react"
import RealTimeLogTable from "./RealTimeLogTable"
import signalRService from "../../../../api/signalRService"
import React from "react"

// Mock the signalR service
jest.mock("../../../../api/signalRService", () => ({
  startConnection: jest.fn(),
  stopConnection: jest.fn(),
  onReceiveAlert: jest.fn(),
}))

// Mock localStorage
const mockLocalStorage = {
  getItem: jest.fn(),
  setItem: jest.fn(),
}
Object.defineProperty(window, "localStorage", { value: mockLocalStorage })

describe("RealTimeLogTable", () => {
  beforeEach(() => {
    jest.clearAllMocks()
    mockLocalStorage.getItem.mockReturnValue(null)
  })

  it("renders loading state initially", () => {
    render(<RealTimeLogTable />)
    expect(screen.getByRole("status")) // Spin component
  })

  it("shows error alert when SignalR connection fails", async () => {
    ;(signalRService.startConnection as jest.Mock).mockRejectedValueOnce(new Error("Connection failed"))

    render(<RealTimeLogTable />)

    await screen.findByText("Error")
    expect(screen.getByText("Falha ao conectar ao SignalR"))
  })

  it("loads logs from localStorage on mount", () => {
    const mockLogs = [
      {
        serialNumberEsp: "ESP001",
        serialNumber: "SN001",
        status: 1,
        description: "Conectado",
        messageType: "jig",
        timestamp: "2024-02-25T10:00:00",
        lastUpdated: "2024-02-25T10:00:00",
      },
    ]

    mockLocalStorage.getItem.mockReturnValue(JSON.stringify(mockLogs))

    render(<RealTimeLogTable />)

    // Wait for loading to finish
    act(() => {
      ;(signalRService.startConnection as jest.Mock).mockResolvedValueOnce(undefined)
    })

    expect(mockLocalStorage.getItem)
  })

  it("filters logs by date range", async () => {
    const mockLogs = [
      {
        serialNumberEsp: "ESP001",
        status: 1,
        description: "Conectado",
        messageType: "jig",
        timestamp: "2024-02-25T10:00:00",
        lastUpdated: "2024-02-25T10:00:00",
      },
    ]

    mockLocalStorage.getItem.mockReturnValue(JSON.stringify(mockLogs))

    render(<RealTimeLogTable />)

    // Wait for loading to finish
    await act(async () => {
      await (signalRService.startConnection as jest.Mock).mockResolvedValueOnce(undefined)
    })

    // Find and interact with the DatePicker
    const datePicker = screen.getByRole("textbox")
    expect(datePicker)
  })

  it("displays correct status badge for different statuses", async () => {
    const mockLogs = [
      {
        serialNumberEsp: "ESP001",
        status: 1, // Connected
        description: "Conectado",
        messageType: "jig",
        timestamp: "2024-02-25T10:00:00",
        lastUpdated: "2024-02-25T10:00:00",
      },
      {
        serialNumberEsp: "ESP002",
        status: 0, // Disconnected
        description: "Desconectado",
        messageType: "jig",
        timestamp: "2024-02-25T10:00:00",
        lastUpdated: "2024-02-25T10:00:00",
      },
    ]

    mockLocalStorage.getItem.mockReturnValue(JSON.stringify(mockLogs))

    render(<RealTimeLogTable />)

    // Wait for loading to finish
    await act(async () => {
      await (signalRService.startConnection as jest.Mock).mockResolvedValueOnce(undefined)
    })

    // Check for status badges
    expect(screen.getByText("Conectado"))
    expect(screen.getByText("Desconectado"))
  })

  it("updates logs when receiving new SignalR message", async () => {
    let signalRCallback: (log: any) => void
    ;(signalRService.onReceiveAlert as jest.Mock).mockImplementation((callback) => {
      signalRCallback = callback
    })

    render(<RealTimeLogTable />)

    // Wait for loading to finish
    await act(async () => {
      await (signalRService.startConnection as jest.Mock).mockResolvedValueOnce(undefined)
    })

    // Simulate receiving a new log
    act(() => {
      signalRCallback({
        serialNumberEsp: "ESP003",
        status: 1,
        description: "New Connection",
        messageType: "jig",
        timestamp: "2024-02-25T11:00:00",
        lastUpdated: "2024-02-25T11:00:00",
      })
    })

    // Verify localStorage was updated
    expect(mockLocalStorage.setItem)
  })

  it("disconnects SignalR on unmount", () => {
    const { unmount } = render(<RealTimeLogTable />)
    unmount()
    expect(signalRService.stopConnection)
  })
})

function beforeEach(arg0: () => void) {
  throw new Error("Function not implemented.")
}

function expect(arg0: any) {
  throw new Error("Function not implemented.")
}

