import Station from "../support/pageObjects/Station";
import FactoryMapPage from "../support/pageObjects/FactoryMapPage";

describe("Station Tests", () => {
  beforeEach(() => {
    cy.fixture("users").then((users) => {
      cy.login(users.validUser.email, users.validUser.password);
    });
    Station.visit();
  });

  it("Deve criar uma nova linha", () => {
    FactoryMapPage.getEditButton().click();
    FactoryMapPage.getCreateButton().click();
    cy.contains("Linha criada com sucesso").should("be.visible");
  });

  it("Deve exibir o título corretamente", () => {
    Station.getHeader().should("be.visible");
    Station.getEditButton().click();
    Station.getCreateButton().click();
  });

  it("Deve entrar no modo de edição", () => {
    Station.getEditButton().click();
    Station.getDeleteButton().should("be.visible");
    Station.getCreateButton().should("be.visible");
  });

  it("Deve criar uma nova Estação", () => {
    Station.getEditButton().click();
    Station.getCreateButton().click();
    cy.contains("Estação criada com sucesso").should("be.visible");
    Station.getFinishEditButton().click();
  });

  it("Deve excluir uma Estação", () => {
    Station.getEditButton().click();
    Station.getStationCheckbox().first().click();
    Station.getDeleteButton().click();
    cy.contains("Estação excluída com sucesso").should("be.visible");
    Station.getFinishEditButton().click();
  });

  it("Deve mostrar um alerta quando estiver sem conexão", () => {
    cy.intercept("GET", "/api/StationView/factoryMap", {
      statusCode: 500,
    }).as("getFactoryMapFail");

    Station.visit();
    cy.wait("@getFactoryMapFail");
    Station.getNoConnectionAlert().should("be.visible");
  });
});
