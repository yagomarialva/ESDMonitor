import { render, screen, fireEvent, waitFor } from "@testing-library/react"
import ESDHomeEditForm from "./ESDHomeEditForm"
import React from "react"

// Mock translations
jest.mock("react-i18next", () => ({
  useTranslation: () => ({
    t: (key: string) => {
      const translations: { [key: string]: string } = {
        "ESD_MONITOR.DIALOG.EDIT_MONITOR": "Edit Monitor",
        "ESD_MONITOR.TABLE.NAME": "Name",
        "ESD_MONITOR.DIALOG.CLOSE": "Close",
        "ESD_MONITOR.DIALOG.SAVE": "Save",
      }
      return translations[key] || key
    },
    i18n: {
      changeLanguage: jest.fn(),
      language: "en",
    },
  }),
}))

describe("ESDHomeEditForm", () => {
  const mockHandleClose = jest.fn()
  const mockOnSubmit = jest.fn()
  const defaultProps = {
    open: true,
    handleClose: mockHandleClose,
    onSubmit: mockOnSubmit,
  }

  const mockInitialData = {
    id: "1",
    description: "Initial description",
    serialNumber: "SN001",
    status: "ACTIVE",
    statusOperador: "PASS",
    statusJig: "PASS",
  }

  beforeEach(() => {
    jest.clearAllMocks()
  })

  it("renders form with initial empty state when no data provided", () => {
    render(<ESDHomeEditForm {...defaultProps} />)

    expect(screen.getByText("Edit Monitor")).toBeInTheDocument()
    expect(screen.getByLabelText("Name")).toHaveValue("")
    expect(screen.getByLabelText("Status do Operador")).toHaveValue("")
    expect(screen.getByLabelText("Status do Jig")).toHaveValue("")
    expect(screen.getByLabelText("Descrição")).toBeDisabled()
  })

  it("renders form with initial data when provided", () => {
    render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} />)

    expect(screen.getByLabelText("Name")).toHaveValue(mockInitialData.serialNumber)
    expect(screen.getByLabelText("Status do Operador")).toHaveValue(mockInitialData.statusOperador)
    expect(screen.getByLabelText("Status do Jig")).toHaveValue(mockInitialData.statusJig)
    expect(screen.getByLabelText("Descrição")).toHaveValue(mockInitialData.description)
  })

  it("enables description field when status is changed", async () => {
    render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} />)

    const operadorSelect = screen.getByLabelText("Status do Operador")
    const descriptionField = screen.getByLabelText("Descrição")

    expect(descriptionField).toBeDisabled()

    fireEvent.mouseDown(operadorSelect)
    const failOption = screen.getByText("FAIL")
    fireEvent.click(failOption)

    expect(descriptionField).toBeEnabled()
    expect(screen.getByText("Para alterar o status é necessário justificar o motivo.")).toBeInTheDocument()
  })

  it("enables save button only when description is modified", async () => {
    render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} />)

    const saveButton = screen.getByText("Save")
    const operadorSelect = screen.getByLabelText("Status do Operador")
    const descriptionField = screen.getByLabelText("Descrição")

    expect(saveButton).toBeDisabled()

    // Change status to enable description field
    fireEvent.mouseDown(operadorSelect)
    fireEvent.click(screen.getByText("FAIL"))

    // Modify description
    fireEvent.change(descriptionField, { target: { value: "New description" } })

    expect(saveButton).toBeEnabled()
  })

  it("submits form with updated data", async () => {
    render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} />)

    // Change status
    fireEvent.mouseDown(screen.getByLabelText("Status do Operador"))
    fireEvent.click(screen.getByText("FAIL"))

    // Update description
    const descriptionField = screen.getByLabelText("Descrição")
    fireEvent.change(descriptionField, { target: { value: "New description" } })

    // Submit form
    const saveButton = screen.getByText("Save")
    fireEvent.click(saveButton)

    expect(mockOnSubmit).toHaveBeenCalledWith({
      ...mockInitialData,
      statusOperador: "FAIL",
      description: "New description",
    })
    expect(mockHandleClose).toHaveBeenCalled()
  })

  it("handles form submission error", async () => {
    const consoleErrorSpy = jest.spyOn(console, "error").mockImplementation(() => {})
    const mockError = new Error("Submission failed")
    const mockOnSubmitError = jest.fn().mockRejectedValue(mockError)

    render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} onSubmit={mockOnSubmitError} />)

    // Change status and description to enable submit
    fireEvent.mouseDown(screen.getByLabelText("Status do Operador"))
    fireEvent.click(screen.getByText("FAIL"))
    fireEvent.change(screen.getByLabelText("Descrição"), {
      target: { value: "New description" },
    })

    // Submit form
    fireEvent.click(screen.getByText("Save"))

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith("Error creating or updating monitor:", mockError)
    })

    consoleErrorSpy.mockRestore()
  })

  it("resets form state when modal is closed and reopened", () => {
    const { rerender } = render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} />)

    // Change status and description
    fireEvent.mouseDown(screen.getByLabelText("Status do Operador"))
    fireEvent.click(screen.getByText("FAIL"))
    fireEvent.change(screen.getByLabelText("Descrição"), {
      target: { value: "New description" },
    })

    // Close and reopen modal
    rerender(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} open={false} />)
    rerender(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} open={true} />)

    // Check if form is reset
    expect(screen.getByLabelText("Status do Operador")).toHaveValue(mockInitialData.statusOperador)
    expect(screen.getByLabelText("Descrição")).toHaveValue(mockInitialData.description)
    expect(screen.getByLabelText("Descrição")).toBeDisabled()
  })

  it("validates required fields before submission", async () => {
    render(<ESDHomeEditForm {...defaultProps} initialData={mockInitialData} />)

    // Change status but don't update description
    fireEvent.mouseDown(screen.getByLabelText("Status do Operador"))
    fireEvent.click(screen.getByText("FAIL"))

    const saveButton = screen.getByText("Save")
    expect(saveButton).toBeDisabled()
  })
})

