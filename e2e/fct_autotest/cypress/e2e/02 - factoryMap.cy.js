import FactoryMapPage from "../support/pageObjects/FactoryMapPage";

describe("Factory Map Tests", () => {
  beforeEach(() => {
    cy.fixture("users").then((users) => {
        cy.login(users.validUser.email, users.validUser.password);
      });
    FactoryMapPage.visit();
  });

  it("Deve exibir o título corretamente", () => {
    FactoryMapPage.getHeader().should("be.visible");
  });

  it("Deve entrar no modo de edição", () => {
    FactoryMapPage.getEditButton().click();
    FactoryMapPage.getDeleteButton().should("be.visible");
    FactoryMapPage.getCreateButton().should("be.visible");
  });

  it("Deve criar uma nova linha", () => {
    FactoryMapPage.getEditButton().click();
    FactoryMapPage.getCreateButton().click();
    cy.contains("Linha criada com sucesso").should("be.visible");
  });

  it("Deve excluir uma linha", () => {
    FactoryMapPage.getEditButton().click();
    FactoryMapPage.getLineCheckbox().last().click(); // Seleciona sempre a primeira linha
    FactoryMapPage.getDeleteButton().click();
    FactoryMapPage.confirmDeletion();
    cy.contains("Linha Teste").should("not.exist");
});

  it("Deve mostrar um alerta quando estiver sem conexão", () => {
    cy.intercept("GET", "/api/StationView/factoryMap", {
      statusCode: 500,
    }).as("getFactoryMapFail");

    FactoryMapPage.visit();
    cy.wait("@getFactoryMapFail");
    FactoryMapPage.getNoConnectionAlert().should("be.visible");
  });
});
