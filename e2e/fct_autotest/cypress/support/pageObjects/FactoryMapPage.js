class FactoryMapPage {
    visit() {
      cy.visit("/#/dashboard");
    }
  
    getHeader() {
      return cy.contains("Linha de produção");
    }
  
    getDeleteButton() {
      return cy.get("button").contains("Excluir Linha");
    }

    getEditButton() {
      return cy.get("button").contains("Editar Linhas");
    }
  
    getCreateButton() {
      return cy.get("button").contains("Adicionar");
    }
    
    getLineCheckbox() {
      return cy.get(".ant-checkbox-input");
    }
  
    getNoConnectionAlert() {
      return cy.contains("Não há linhas para serem exibidas");
    }
  
    confirmDeletion() {
      cy.get(".ant-modal-confirm").contains("OK").click();
    }
  }
  
  export default new FactoryMapPage();
  