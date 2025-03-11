import LoginPage from "../support/pageObjects/LoginPage";

describe("Login Page Tests", () => {
  beforeEach(() => {
    cy.fixture("users").then((users) => {
      cy.wrap(users).as("users");
    });
    LoginPage.visit();
  });

  it("Deve fazer login com sucesso", function () {
    const { validUser } = this.users;
    LoginPage.fillEmail(validUser.email);
    LoginPage.fillPassword(validUser.password);
    LoginPage.submit();

    cy.url().should("eq", Cypress.config("baseUrl") + "/login#/dashboard"); // Deve redirecionar para a página inicial
  });

  it("Deve mostrar erro para credenciais inválidas", function () {
    const { invalidUser } = this.users;
    LoginPage.fillEmail(invalidUser.email);
    LoginPage.fillPassword(invalidUser.password);
    LoginPage.submit();

    LoginPage.getSnackbar().should("contain", "Login ou senha inválidos");
  });
});
