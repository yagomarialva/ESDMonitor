// src/components/NotFoundPage/NotFoundPage.js
import React from "react";
import { Box, Typography, Button } from "@mui/material";
import { Link } from "react-router-dom";
import { Home as HomeIcon } from "@mui/icons-material";
import Logo from "./logo.png";

const NotFoundPage = () => {
  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        height: "100vh",
        textAlign: "center",
      }}
    >
      <img src={Logo} alt="" width="200px" style={{ marginTop: "20px" }} />
      <Typography variant="h1" component="h1" color="textSecondary">
        404
      </Typography>
      <Typography variant="h5" component="h2" color="textSecondary">
        Página Não Encontrada
      </Typography>
      <Button
        variant="contained"
        color="success"
        component={Link}
        to="/dashboard"
        sx={{ mt: 2 }}
        startIcon={<HomeIcon />}
      >
        Voltar para o Início
      </Button>
    </Box>
  );
};

export default NotFoundPage;
