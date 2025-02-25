// src/App.test.js
import React from "react";
import { render, screen} from "@testing-library/react";
import { BrowserRouter as Router } from "react-router-dom";
import App from "./App";

describe("App Component", () => {
  it("renders AppBar with navigation links", () => {
    render(
        <Router>
          <App />
        </Router>
    );  
  });

});
