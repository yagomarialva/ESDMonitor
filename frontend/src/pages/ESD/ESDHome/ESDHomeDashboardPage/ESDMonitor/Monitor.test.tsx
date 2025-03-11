import { render, screen, fireEvent, within } from "@testing-library/react"
import "@testing-library/jest-dom"
import Monitor from "./Monitor"
import React from "react"

const mockMonitor = {
  positionSequence: 1,
  monitorsESD: {
    id: "1",
    serialNumber: "SN123456789",
    description: "Test Monitor Description",
    statusJig: "Connected",
    statusOperador: "Active",
  },
}

describe("Monitor Component", () => {
  const mockOnMonitorTabActive = jest.fn()

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders monitor information correctly in view mode", () => {
    render(<Monitor monitor={mockMonitor} onMonitorTabActive={mockOnMonitorTabActive} isEditing={false} />)

    // Check if truncated values are displayed
    expect(screen.getByText("SN12345678...")).toBeInTheDocument()
    expect(screen.getByText("Test Monit...")).toBeInTheDocument()
    expect(screen.getByText("Connected")).toBeInTheDocument()
    expect(screen.getByText("Active")).toBeInTheDocument()

    // Check if full values are available in tooltips
    const tooltips = screen
      .getAllByRole("cell")
      .map((cell) => cell.querySelector(".ant-tooltip-open")?.getAttribute("title"))
    expect(tooltips).toContain("SN123456789")
    expect(tooltips).toContain("Test Monitor Description")
  })

  it("renders edit mode with input fields", () => {
    render(<Monitor monitor={mockMonitor} onMonitorTabActive={mockOnMonitorTabActive} isEditing={true} />)

    // Check if input fields are rendered with correct values
    const inputs = screen.getAllByRole("textbox")
    expect(inputs).toHaveLength(4)

    expect(inputs[0]).toHaveValue("SN123456789")
    expect(inputs[1]).toHaveValue("Test Monitor Description")
    expect(inputs[2]).toHaveValue("Connected")
    expect(inputs[3]).toHaveValue("Active")
  })

  it("handles input changes in edit mode", () => {
    render(<Monitor monitor={mockMonitor} onMonitorTabActive={mockOnMonitorTabActive} isEditing={true} />)

    const serialNumberInput = screen.getAllByRole("textbox")[0]
    fireEvent.change(serialNumberInput, { target: { value: "NewSerialNumber" } })
    expect(serialNumberInput).toHaveValue("NewSerialNumber")
  })

  it("switches between monitor and log tabs", () => {
    render(<Monitor monitor={mockMonitor} onMonitorTabActive={mockOnMonitorTabActive} isEditing={false} />)

    // Initially on Monitor tab
    expect(mockOnMonitorTabActive).not.toHaveBeenCalled()

    // Switch to Log tab
    const logTab = screen.getByText("Log")
    fireEvent.click(logTab)

    expect(mockOnMonitorTabActive).toHaveBeenCalledWith(false)

    // Check if log tables are rendered
    expect(screen.getByText("Logs de Operador")).toBeInTheDocument()
    expect(screen.getByText("Logs de Jigs")).toBeInTheDocument()
  })

  it("displays operator and jig logs correctly", () => {
    render(<Monitor monitor={mockMonitor} onMonitorTabActive={mockOnMonitorTabActive} isEditing={false} />)

    // Switch to Log tab
    const logTab = screen.getByText("Log")
    fireEvent.click(logTab)

    // Check operator logs
    expect(screen.getByText("Operador iniciou tarefa")).toBeInTheDocument()
    expect(screen.getByText("Operador finalizou tarefa")).toBeInTheDocument()

    // Check jig logs
    expect(screen.getByText("Jig conectado")).toBeInTheDocument()
    expect(screen.getByText("Jig desconectado")).toBeInTheDocument()
  })

  const truncateText = (text: string, maxLength: number) => {
    if (text.length <= maxLength) {
      return text
    }
    return text.substring(0, maxLength) + "..."
  }

  it("truncates text correctly", () => {
    const longText = "This is a very long text that should be truncated"
    const truncated = truncateText(longText, 10)
    expect(truncated).toBe("This is a ...")
  })

  it("maintains table structure in both tabs", () => {
    render(<Monitor monitor={mockMonitor} onMonitorTabActive={mockOnMonitorTabActive} isEditing={false} />)

    // Check Monitor tab table
    expect(screen.getAllByRole("columnheader")).toHaveLength(4)

    // Switch to Log tab
    const logTab = screen.getByText("Log")
    fireEvent.click(logTab)

    // Check Log tables
    const operatorTable = screen.getByText("Logs de Operador").closest("table")
    const jigTable = screen.getByText("Logs de Jigs").closest("table")

    expect(operatorTable).toBeInTheDocument()
    expect(jigTable).toBeInTheDocument()

    if (operatorTable) {
      const operatorHeaders = within(operatorTable).getAllByRole("columnheader")
      expect(operatorHeaders).toHaveLength(2) // Message and Date columns
    }

    if (jigTable) {
      const jigHeaders = within(jigTable).getAllByRole("columnheader")
      expect(jigHeaders).toHaveLength(2) // Message and Date columns
    }
  })
})

