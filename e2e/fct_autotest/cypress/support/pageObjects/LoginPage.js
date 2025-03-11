class LoginPage {
    visit() {
      cy.visit("/login");
    }
  
    fillEmail(email) {
      cy.get("input[id='Nome']").type(email);
    }
  
    fillPassword(password) {
      cy.get("input[id='Senha']").type(password);
    }
  
    submit() {
      cy.get("button").contains("Fazer Login").click();
    }
  
    getSnackbar() {
      return cy.get(".snackbar-content");
    }
  }
  
  export default new LoginPage();
  