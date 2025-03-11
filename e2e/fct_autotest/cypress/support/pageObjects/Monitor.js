class Monitor {
  visit() {
    cy.visit("/#/dashboard");
  }

  getHeader() {
    return cy.contains("Linha de produção");
  }

  getEditButton() {
    return cy.get("button").contains("Editar Estações");
  }

  getFinishEditButton() {
    return cy.get("button").contains("Finalizar Edição");
  }

  getDeleteButton() {
    return cy.get("svg#delete-icon");
  }

  getCreateButton() {
    return cy.get("div#cell");
  }

  
  getSerialNumberButton() {
    return cy.get("input#serialNumberEsp");
  }

  getDescriptionButton() {
    return cy.get("input#description");
  }

  getSaveMonitorButton() {
    return cy.get("button#submit-button-add-monitor");
  }

  getCancelMonitorButton() {
    return cy.get("button#submit-button-cancel-monitor");
  }

  getStationCheckbox() {
    return cy.get("input#radio-button-station");
  }

  getNoConnectionAlert() {
    return cy.contains("Não há linhas para serem exibidas");
  }

  confirmDeletion() {
    cy.contains("button", "OK").click(); // Garante que está clicando no botão correto
  }
}

export default new Monitor();
