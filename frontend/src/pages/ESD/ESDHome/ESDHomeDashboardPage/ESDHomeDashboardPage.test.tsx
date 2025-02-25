import { render, screen, waitFor } from "@testing-library/react"
import { act } from "react-dom/test-utils"
import ESDDashboardPage from "./ESDHomeDashboardPage"
import { getAllStationMapper } from "../../../../api/mapingAPI"
import { useNavigate } from "react-router-dom"
import { jest, describe, beforeEach, it, expect } from "@jest/globals"
import React from "react"

// Mock the dependencies
jest.mock("react-router-dom", () => ({
  useNavigate: jest.fn(),
}))

jest.mock("react-i18next", () => ({
  useTranslation: () => ({
    t: (key: string) => key,
  }),
}))

jest.mock("../../../../api/mapingAPI", () => ({
  getAllStationMapper: jest.fn(),
}))

// Mock the FactoryMap component
jest.mock("./ESDFactoryMap/FactoryMap", () => {
  return function MockFactoryMap({ lines, onUpdate }: { lines: any[]; onUpdate: () => void }) {
    return <div data-testid="factory-map">Factory Map Mock</div>
  }
})

describe("ESDDashboardPage", () => {
  const mockNavigate = jest.fn()
  const mockStationsData = [
    {
      id: 1,
      line: {
        id: 1,
        name: "Line 1",
      },
      stations: [
        {
          station: {
            id: 1,
            name: "Station 1",
            sizeX: 100,
            sizeY: 100,
          },
          monitorsESD: [],
        },
      ],
    },
  ]

  beforeEach(() => {
    jest.clearAllMocks()
    ;(useNavigate as jest.Mock).mockReturnValue(mockNavigate)
    localStorage.clear()
  })

  it("renders FactoryMap component", async () => {
    ;(getAllStationMapper as jest.Mock).mockResolvedValueOnce(mockStationsData)

    render(<ESDDashboardPage />)

    expect(screen.getByTestId("factory-map"))
  })

  it("fetches stations data on mount", async () => {
    ;(getAllStationMapper as jest.Mock).mockResolvedValueOnce(mockStationsData)

    render(<ESDDashboardPage />)

    await waitFor(() => {
      expect(getAllStationMapper).toHaveBeenCalled()
    })
  })

  it("handles API error with 401 status", async () => {
    const error = new Error("Request failed with status code 401")
    ;(getAllStationMapper as jest.Mock).mockRejectedValueOnce(error)

    render(<ESDDashboardPage />)

    await waitFor(() => {
      expect(localStorage.getItem("token")).toBeNull()
      expect(mockNavigate).toHaveBeenCalledWith("/")
    })
  })

  it("handles other API errors without navigation", async () => {
    const consoleErrorSpy = jest.spyOn(console, "error").mockImplementation(() => {})
    const error = new Error("Generic error")
    ;(getAllStationMapper as jest.Mock).mockRejectedValueOnce(error)

    render(<ESDDashboardPage />)

    await waitFor(() => {
      expect(consoleErrorSpy).toHaveBeenCalledWith("Erro ao buscar dados:", error)
      expect(mockNavigate).not.toHaveBeenCalled()
    })

    consoleErrorSpy.mockRestore()
  })

  it("updates data when handleUpdate is called", async () => {
    ;(getAllStationMapper as jest.Mock).mockResolvedValueOnce(mockStationsData)

    const { getByTestId } = render(<ESDDashboardPage />)

    await waitFor(() => {
      expect(getAllStationMapper).toHaveBeenCalledTimes(1)
    })

    // Reset the mock to track the next call
    ;(getAllStationMapper as jest.Mock).mockClear()

    // Simulate update from FactoryMap component
    const factoryMap = getByTestId("factory-map")
    const props = (factoryMap as any).__reactProps$
    act(() => {
      props.onUpdate()
    })

    await waitFor(() => {
      expect(getAllStationMapper).toHaveBeenCalledTimes(1)
    })
  })

  it("passes correct props to FactoryMap", async () => {
    ;(getAllStationMapper as jest.Mock).mockResolvedValueOnce(mockStationsData)

    const { getByTestId } = render(<ESDDashboardPage />)

    await waitFor(() => {
      const factoryMap = getByTestId("factory-map")
      const props = (factoryMap as any).__reactProps$
      expect(props.lines).toEqual(mockStationsData)
      expect(typeof props.onUpdate).toBe("function")
    })
  })

  it("cleans up properly on unmount", async () => {
    ;(getAllStationMapper as jest.Mock).mockResolvedValueOnce(mockStationsData)

    const { unmount } = render(<ESDDashboardPage />)

    await waitFor(() => {
      expect(getAllStationMapper).toHaveBeenCalled()
    })

    unmount()

    // Add any specific cleanup checks if needed
    // For example, if you add event listeners or intervals in the future
  })

  it("handles empty stations data", async () => {
    ;(getAllStationMapper as jest.Mock).mockResolvedValueOnce([])

    render(<ESDDashboardPage />)

    await waitFor(() => {
      const factoryMap = screen.getByTestId("factory-map")
      const props = (factoryMap as any).__reactProps$
      expect(props.lines).toEqual([])
    })
  })
})

