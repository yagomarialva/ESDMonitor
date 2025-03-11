Cypress.Commands.add("login", (email, password) => {
    cy.request("POST", "/api/Authentication", {
      username: email,
      password: password,
    }).then((response) => {
      expect(response.status).to.eq(200);
      localStorage.setItem("token", response.body.token);
      localStorage.setItem("role", response.body.role);
      localStorage.setItem("name", response.body.name);
    });
  });
  