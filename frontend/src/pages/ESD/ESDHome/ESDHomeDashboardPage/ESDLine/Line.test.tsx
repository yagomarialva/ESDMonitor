import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import Line from "./Line";
import { createStation, deleteStation, getAllStations } from "../../../../../api/stationApi";
import { createLink, deleteLink } from "../../../../../api/linkStationLine";
import { BrowserRouter } from "react-router-dom";

jest.mock("../../../../../api/stationApi", () => ({
  createStation: jest.fn(),
  deleteStation: jest.fn(),
  getAllStations: jest.fn(),
  getStationByName: jest.fn(),
}));

jest.mock("../../../../../api/linkStationLine", () => ({
  createLink: jest.fn(),
  deleteLink: jest.fn(),
}));

const mockLineData = {
  id: 1,
  line: { id: 1, name: "Linha 1" },
  stations: [
    {
      station: { id: 1, name: "Estação 1", sizeX: 6, sizeY: 6, linkStationAndLineID: 1 },
      linkStationAndLineID: 1,
      monitorsEsd: [],
    },
  ],
};

describe("Line Component", () => {
  it("deve renderizar corretamente", () => {
    render(
      <BrowserRouter>
        <Line lineData={mockLineData} onUpdate={jest.fn()} />
      </BrowserRouter>
    );
    expect(screen.getByText("Linha 1")).toBeInTheDocument();
  });

  it("deve alternar o modo de edição corretamente", () => {
    render(
      <BrowserRouter>
        <Line lineData={mockLineData} onUpdate={jest.fn()} />
      </BrowserRouter>
    );
    const editButton = screen.getByText("Editar Estações");
    fireEvent.click(editButton);
    expect(screen.getByText("Finalizar Edição")).toBeInTheDocument();
  });

  it("deve chamar a função de criação de estação corretamente", async () => {
    (createStation as jest.Mock).mockResolvedValueOnce({ name: "Nova Estação", sizeX: 6, sizeY: 6 });
    (getAllStations as jest.Mock).mockResolvedValueOnce([...mockLineData.stations]);

    render(
      <BrowserRouter>
        <Line lineData={mockLineData} onUpdate={jest.fn()} />
      </BrowserRouter>
    );

    const editButton = screen.getByText("Editar Estações");
    fireEvent.click(editButton);

    const addButton = screen.getByRole("button", { name: /add/i });
    fireEvent.click(addButton);

    await waitFor(() => expect(createStation).toHaveBeenCalled());
  });

  it("deve chamar a função de exclusão de estação corretamente", async () => {
    (deleteStation as jest.Mock).mockResolvedValueOnce({});
    (deleteLink as jest.Mock).mockResolvedValueOnce({});
    
    render(
      <BrowserRouter>
        <Line lineData={mockLineData} onUpdate={jest.fn()} />
      </BrowserRouter>
    );

    const editButton = screen.getByText("Editar Estações");
    fireEvent.click(editButton);

    const removeButton = screen.getByRole("button", { name: /remove/i });
    fireEvent.click(removeButton);

    await waitFor(() => expect(deleteStation).toHaveBeenCalled());
  });

  it("deve exibir a Snackbar corretamente ao criar estação", async () => {
    (createStation as jest.Mock).mockResolvedValueOnce({ name: "Nova Estação", sizeX: 6, sizeY: 6 });
    (getAllStations as jest.Mock).mockResolvedValueOnce([...mockLineData.stations]);

    render(
      <BrowserRouter>
        <Line lineData={mockLineData} onUpdate={jest.fn()} />
      </BrowserRouter>
    );

    const editButton = screen.getByText("Editar Estações");
    fireEvent.click(editButton);

    const addButton = screen.getByRole("button", { name: /add/i });
    fireEvent.click(addButton);

    await waitFor(() => expect(screen.getByText("Estação criada com sucesso!")).toBeInTheDocument());
  });
});
