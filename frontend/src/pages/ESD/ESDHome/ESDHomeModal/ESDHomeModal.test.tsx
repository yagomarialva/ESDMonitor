import { render, screen, fireEvent } from "@testing-library/react"
import ESDHomeModal from "./ESDHomeModal"
import React from "react"

// Mock translations
jest.mock("react-i18next", () => ({
  useTranslation: () => ({
    t: (key: string) => {
      if (key === "ESD_OPERATOR.DIALOG.CLOSE") {
        return "Close"
      }
      return key
    },
  }),
}))

describe("ESDHomeModal", () => {
  const mockHandleClose = jest.fn()
  const defaultProps = {
    open: true,
    handleClose: mockHandleClose,
  }

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders modal when open is true", () => {
    render(<ESDHomeModal {...defaultProps} />)

    expect(screen.getByRole("dialog")).toBeInTheDocument()
  })

  it("displays N/A for undefined produce values", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const textFields = screen.getAllByRole("textbox")
    textFields.forEach((field) => {
      expect(field).toHaveValue("N/A")
    })
  })

  it("displays correct produce values when provided", () => {
    const produce = {
      monitorsEsd: {
        stationName: "Station 1",
      },
      lineName: "Line A",
      status: "Active",
      statusJig: "Connected",
      statusOperador: "Online",
    }

    render(<ESDHomeModal {...defaultProps} produce={produce} />)

    expect(screen.getByLabelText("Data")).toHaveValue("Station 1")
    expect(screen.getByLabelText("Hora")).toHaveValue("Line A")
    expect(screen.getByLabelText("Operador")).toHaveValue("Active")
    expect(screen.getAllByLabelText("Logs")[0]).toHaveValue("Connected")
    expect(screen.getAllByLabelText("Logs")[1]).toHaveValue("Online")
  })

  it("calls handleClose when close button is clicked", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const closeButton = screen.getByText("Close")
    fireEvent.click(closeButton)

    expect(mockHandleClose).toHaveBeenCalledTimes(1)
  })

  it("calls handleClose when modal backdrop is clicked", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const modalBackdrop = screen.getByRole("dialog").parentElement
    fireEvent.click(modalBackdrop!)

    expect(mockHandleClose).toHaveBeenCalledTimes(1)
  })

  it("renders all text fields as disabled", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const textFields = screen.getAllByRole("textbox")
    textFields.forEach((field) => {
      expect(field).toBeDisabled()
    })
  })

  it("renders all text fields with required attribute", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const textFields = screen.getAllByRole("textbox")
    textFields.forEach((field) => {
      expect(field).toHaveAttribute("required")
    })
  })

  it("does not render when open is false", () => {
    render(<ESDHomeModal {...defaultProps} open={false} />)

    expect(screen.queryByRole("dialog")).not.toBeInTheDocument()
  })

  it("renders with correct aria labels", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const modal = screen.getByRole("dialog")
    expect(modal).toHaveAttribute("aria-labelledby", "modal-modal-title")
    expect(modal).toHaveAttribute("aria-describedby", "modal-modal-description")
  })

  it("renders success variant close button", () => {
    render(<ESDHomeModal {...defaultProps} />)

    const closeButton = screen.getByText("Close")
    expect(closeButton).toHaveClass("MuiButton-containedSuccess")
  })
})

