class Station {
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
    return cy.get("button#delete-button");
  }

  getEditLineButton() {
    return cy.get("button").contains("Editar Linhas");
  }

  getCreateLineButton() {
    return cy.get("button").contains("Adicionar");
  }

  getCreateButton() {
    return cy.get("button#add-button");
  }

  getStationCheckbox() {
    return cy.get("input#radio-button-station");
  }

  getNoConnectionAlert() {
    return cy.contains("Não há linhas para serem exibidas");
  }

  confirmDeletion() {
    cy.get(".ant-modal-confirm").contains("OK").click();
  }
}

export default new Station();
