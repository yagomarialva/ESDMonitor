const { defineConfig } = require("cypress");

module.exports = defineConfig({
  e2e: {
    baseUrl: "http://localhost:3000", // Ou "http://192.168.0.101:3000"
    setupNodeEvents(on, config) {
      // Configurações adicionais podem ser feitas aqui
    },
  },
});
