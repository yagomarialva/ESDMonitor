import { render, screen, fireEvent, waitFor } from "@testing-library/react"
import "@testing-library/jest-dom"
import FactoryMap from "./FactoryMap"
import signalRService from "../../../../../api/signalRService"
import { createLine, deleteLine } from "../../../../../api/linerApi"
import { createStation, deleteStation, getStationByName } from "../../../../../api/stationApi"
import { createLink, deleteLink } from "../../../../../api/linkStationLine"
import React from "react"

// Mock all the required dependencies
jest.mock("../../../../../api/signalRService")
jest.mock("../../../../../api/linerApi")
jest.mock("../../../../../api/stationApi")
jest.mock("../../../../../api/linkStationLine")
jest.mock("react-i18next", () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}))

jest.mock("react-router-dom", () => ({
  useNavigate: () => jest.fn(),
}))

const mockLines = [
  {
    id: 1,
    line: { id: 1, name: "Line 1" },
    stations: [
      {
        station: {
          id: 1,
          linkStationAndLineID: 1,
          name: "Station 1",
          sizeX: 6,
          sizeY: 6,
        },
        linkStationAndLineID: 1,
        monitorsEsd: [],
      },
    ],
  },
]

describe("FactoryMap Component", () => {
  const mockOnUpdate = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
    ;(signalRService.startConnection as jest.Mock).mockResolvedValue(undefined)
    ;(signalRService.getConnectionState as jest.Mock).mockReturnValue("Connected")
  })

  it("renders without crashing and initializes SignalR connection", async () => {
    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    await waitFor(() => {
      expect(signalRService.startConnection).toHaveBeenCalled()
    })

    expect(screen.getByText("LINE.TABLE_HEADER")).toBeInTheDocument()
  })

  it("handles connection loss correctly", async () => {
    ;(signalRService.getConnectionState as jest.Mock).mockReturnValue("Disconnected")

    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    await waitFor(() => {
      expect(screen.getByText("Sem Conexão")).toBeInTheDocument()
    })
  })

  it("enables editing mode", () => {
    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    const editButton = screen.getByText("Editar Linhas")
    fireEvent.click(editButton)

    expect(screen.getByText("Finalizar Edição")).toBeInTheDocument()
    expect(screen.getByText("LINE.ADD_LINE")).toBeInTheDocument()
  })

  it("creates a new line successfully", async () => {
    ;(createLine as jest.Mock).mockResolvedValue({ id: 2, name: "New Line" })
    ;(createStation as jest.Mock).mockResolvedValue({ id: 2, name: "New Line" })
    ;(getStationByName as jest.Mock).mockResolvedValue({ id: 2 })
    ;(createLink as jest.Mock).mockResolvedValue({})

    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    // Enable editing mode
    fireEvent.click(screen.getByText("Editar Linhas"))

    // Click add line button
    const addButton = screen.getByText("LINE.ADD_LINE")
    fireEvent.click(addButton)

    await waitFor(() => {
      expect(createLine).toHaveBeenCalled()
      expect(createStation).toHaveBeenCalled()
      expect(createLink).toHaveBeenCalled()
      expect(mockOnUpdate).toHaveBeenCalled()
    })
  })

  it("handles line deletion", async () => {
    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    // Enable editing mode
    fireEvent.click(screen.getByText("Editar Linhas"))

    // Select a line
    const checkbox = screen.getByRole("checkbox")
    fireEvent.click(checkbox)

    // Click delete button
    const deleteButton = screen.getByText("LINE.CONFIRM_DIALOG.DELETE_LINE")
    fireEvent.click(deleteButton)

    // Confirm deletion in modal
    const confirmButton = await screen.findByText("OK")
    fireEvent.click(confirmButton)

    await waitFor(() => {
      expect(deleteLink).toHaveBeenCalledWith(1)
      expect(deleteLine).toHaveBeenCalledWith(1)
      expect(deleteStation).toHaveBeenCalledWith(1)
      expect(mockOnUpdate).toHaveBeenCalled()
    })
  })

  it("handles real-time updates via SignalR", async () => {
    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    const mockLog = {
      serialNumber: "TEST001",
      status: 1,
      description: "Connected",
    }

    await waitFor(() => {
      expect(signalRService.onReceiveAlert).toHaveBeenCalled()
    })

    // Simulate receiving a SignalR message
    const onReceiveAlertCallback = (signalRService.onReceiveAlert as jest.Mock).mock.calls[0][0]
    onReceiveAlertCallback(mockLog)

    // Verify the log is processed (you might need to adjust this based on how logs are displayed)
    expect(screen.getByText("Connected")).toBeInTheDocument()
  })

  it("displays empty state when no lines exist", () => {
    render(<FactoryMap lines={[]} onUpdate={mockOnUpdate} />)

    expect(screen.getByText("Não há linhas para serem exibidas")).toBeInTheDocument()
  })

  it("prevents deletion of lines with multiple stations", () => {
    const linesWithMultipleStations = [
      {
        ...mockLines[0],
        stations: [
          ...mockLines[0].stations,
          {
            station: {
              id: 2,
              linkStationAndLineID: 2,
              name: "Station 2",
              sizeX: 6,
              sizeY: 6,
            },
            linkStationAndLineID: 2,
            monitorsEsd: [],
          },
        ],
      },
    ]

    render(<FactoryMap lines={linesWithMultipleStations} onUpdate={mockOnUpdate} />)

    // Enable editing mode
    fireEvent.click(screen.getByText("Editar Linhas"))

    const checkbox = screen.getByRole("checkbox")
    expect(checkbox).toBeDisabled()
  })

  it("handles error cases gracefully", async () => {
    ;(createLine as jest.Mock).mockRejectedValue(new Error("Failed to create line"))

    render(<FactoryMap lines={mockLines} onUpdate={mockOnUpdate} />)

    // Enable editing mode
    fireEvent.click(screen.getByText("Editar Linhas"))

    // Attempt to create a new line
    const addButton = screen.getByText("LINE.ADD_LINE")
    fireEvent.click(addButton)

    await waitFor(() => {
      expect(screen.getByText("Erro ao criar estação")).toBeInTheDocument()
    })
  })
})

