import { render, screen, fireEvent, waitFor } from "@testing-library/react"
import ESDHomeForm from "./ESDHomeForm"
import { getAllMonitors, getMonitor } from "../../../../api/monitorApi"
import { getAllLinks, getLink } from "../../../../api/linkStationLine"
import React from "react"

// Mock the API calls
jest.mock("../../../../api/monitorApi", () => ({
  getAllMonitors: jest.fn(),
  getMonitor: jest.fn(),
}))

jest.mock("../../../../api/linkStationLine", () => ({
  getAllLinks: jest.fn(),
  getLink: jest.fn(),
}))

// Mock translations
jest.mock("react-i18next", () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: { [key: string]: string } = {
        "LINK_STATION_LINE.ADD_LINK_STATION_LINE": "Add Link Station Line",
        "LINK_STATION_LINE.TABLE.LINE": "Line",
        "LINK_STATION_LINE.DIALOG.SAVE": "Save",
      }
      return translations[key] || key
    },
  }),
}))

describe("ESDHomeForm", () => {
  const mockHandleClose = jest.fn()
  const mockOnSubmit = jest.fn()
  const defaultProps = {
    open: true,
    handleClose: mockHandleClose,
    onSubmit: mockOnSubmit,
  }

  const mockMonitors = [
    { id: 1, serialNumber: "MON001" },
    { id: 2, serialNumber: "MON002" },
  ]

  const mockLinks = [{ id: 1 }, { id: 2 }]

  beforeEach(() => {
    jest.clearAllMocks()
    ;(getAllMonitors as jest.Mock).mockResolvedValue(mockMonitors)
    ;(getAllLinks as jest.Mock).mockResolvedValue(mockLinks)
  })

  it("renders the form with correct initial state", async () => {
    render(<ESDHomeForm {...defaultProps} />)

    expect(screen.getByText("Add Link Station Line")).toBeInTheDocument()
    expect(screen.getByLabelText("Monitor ESD")).toBeInTheDocument()
    expect(screen.getByLabelText("Line")).toBeInTheDocument()
    expect(screen.getByText("Save")).toBeInTheDocument()

    await waitFor(() => {
      expect(getAllMonitors).toHaveBeenCalled()
      expect(getAllLinks).toHaveBeenCalled()
    })
  })

  it("loads and displays monitors and links", async () => {
    render(<ESDHomeForm {...defaultProps} />)

    await waitFor(() => {
      expect(getAllMonitors).toHaveBeenCalled()
      expect(getAllLinks).toHaveBeenCalled()
    })

    // Open monitor select
    fireEvent.mouseDown(screen.getByLabelText("Monitor ESD"))
    await waitFor(() => {
      expect(screen.getByText("MON001")).toBeInTheDocument()
      expect(screen.getByText("MON002")).toBeInTheDocument()
    })

    // Open link select
    fireEvent.mouseDown(screen.getByLabelText("Line"))
    await waitFor(() => {
      expect(screen.getByText("1")).toBeInTheDocument()
      expect(screen.getByText("2")).toBeInTheDocument()
    })
  })

  it("handles monitor selection", async () => {
    render(<ESDHomeForm {...defaultProps} />)

    await waitFor(() => {
      expect(getAllMonitors).toHaveBeenCalled()
    })

    fireEvent.mouseDown(screen.getByLabelText("Monitor ESD"))
    fireEvent.click(screen.getByText("MON001"))

    expect(getMonitor).toHaveBeenCalledWith(1)
  })

  it("handles link selection", async () => {
    render(<ESDHomeForm {...defaultProps} />)

    await waitFor(() => {
      expect(getAllLinks).toHaveBeenCalled()
    })

    fireEvent.mouseDown(screen.getByLabelText("Line"))
    fireEvent.click(screen.getByText("1"))

    expect(getLink).toHaveBeenCalledWith(1)
  })

  it("submits form with correct data", async () => {
    render(<ESDHomeForm {...defaultProps} />)

    await waitFor(() => {
      expect(getAllMonitors).toHaveBeenCalled()
      expect(getAllLinks).toHaveBeenCalled()
    })

    // Select monitor
    fireEvent.mouseDown(screen.getByLabelText("Monitor ESD"))
    fireEvent.click(screen.getByText("MON001"))

    // Select link
    fireEvent.mouseDown(screen.getByLabelText("Line"))
    fireEvent.click(screen.getByText("1"))

    // Submit form
    fireEvent.click(screen.getByText("Save"))

    expect(mockOnSubmit).toHaveBeenCalledWith({
      monitorEsdId: 1,
      linkStationAndLineId: 1,
    })
    expect(mockHandleClose).toHaveBeenCalled()
  })

  it("handles API errors gracefully", async () => {
    const consoleErrorSpy = jest.spyOn(console, "error").mockImplementation(() => {})
    ;(getAllMonitors as jest.Mock).mockRejectedValueOnce(new Error("API Error"))

    render(<ESDHomeForm {...defaultProps} />)

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith("Error fetching data:", expect.any(Error))
    })

    consoleErrorSpy.mockRestore()
  })

  it("closes modal when handleClose is called", () => {
    render(<ESDHomeForm {...defaultProps} />)

    fireEvent.click(screen.getByRole("button", { name: /save/i }))

    expect(mockHandleClose).toHaveBeenCalled()
  })

  it("validates required fields before submission", async () => {
    render(<ESDHomeForm {...defaultProps} />)

    // Try to submit without selecting values
    fireEvent.click(screen.getByText("Save"))

    // Check if form validation prevented submission
    expect(mockOnSubmit).not.toHaveBeenCalled()
  })
})

