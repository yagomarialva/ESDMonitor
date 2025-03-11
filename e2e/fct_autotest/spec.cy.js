describe('Meu Primeiro Teste', () => {
    it('Visita o site e verifica o título', () => {
      cy.visit('https://example.cypress.io');
      cy.contains('Kitchen Sink');
    });
  });
  