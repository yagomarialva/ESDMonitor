Cypress.Commands.add("login", (email, password) => {
    cy.visit("/login");
    cy.get("input[id='Nome']").type(email);
    cy.get("input[id='Senha']").type(password);
    cy.get("button").contains("Fazer Login").click();
  });
  