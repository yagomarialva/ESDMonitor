import Monitor from "../support/pageObjects/Monitor";
import FactoryMapPage from "../support/pageObjects/FactoryMapPage";

describe("Monitor Tests", () => {
  beforeEach(() => {
    cy.fixture("users").then((users) => {
      cy.login(users.validUser.email, users.validUser.password);
    });
    Monitor.visit();
  });

  it("Deve exibir o título corretamente", () => {
    Monitor.getHeader().should("be.visible");
  });

  it("Deve criar uma nova linha", () => {
    FactoryMapPage.getEditButton().click();
    FactoryMapPage.getCreateButton().click();
    cy.contains("Linha criada com sucesso").should("be.visible");
  });

  it("Deve cancelar criação de um novo monitor", () => {
    Monitor.getEditButton().click();
    Monitor.getCreateButton().first().click();
    Monitor.getSerialNumberButton().type("123456");
    Monitor.getDescriptionButton().type("Monitor de teste");
    Monitor.getCancelMonitorButton().click();
    // cy.contains("Estação criada com sucesso").should("be.visible");
    // Monitor.getFinishEditButton().click();
  });

  it("Deve criar um novo monitor", () => {
    Monitor.getEditButton().click();
    Monitor.getCreateButton().first().click();
    Monitor.getSerialNumberButton().type("123456");
    Monitor.getDescriptionButton().type("Monitor de teste");
    Monitor.getSaveMonitorButton().click();
    // cy.contains("Estação criada com sucesso").should("be.visible");
    // Monitor.getFinishEditButton().click();
  });

  it("Deve excluir um monitor", () => {
    Monitor.getEditButton().click();
    Monitor.getCreateButton().first().click();
    Monitor.getDeleteButton().click();

    // Agora a confirmação do delete clica no botão "OK"
    Monitor.confirmDeletion();

    cy.contains("Monitor excluído com sucesso!").should("be.visible");
    Monitor.getFinishEditButton().click();
  });
  
    it("Deve mostrar um alerta quando estiver sem conexão", () => {
      cy.intercept("GET", "/api/StationView/factoryMap", {
        statusCode: 500,
      }).as("getFactoryMapFail");

      Monitor.visit();
      cy.wait("@getFactoryMapFail");
      Monitor.getNoConnectionAlert().should("be.visible");
    });
});
