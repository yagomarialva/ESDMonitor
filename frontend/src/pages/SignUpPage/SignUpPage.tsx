import React, { useState } from "react";
import {
  Alert,
  Button,
  Card,
  Container,
  Snackbar,
  TextField,
  Typography,
  FormControl,
  InputLabel,
  MenuItem,
  Select,
  SelectChangeEvent,
} from "@mui/material";
import { LoadingButton } from "@mui/lab";
import { useNavigate } from "react-router-dom";
import { createUser, UserData } from "../../api/userApi";
import Logo from "./logo.png";

interface SnackbarState {
  open: boolean;
  message: string;
  severity: "success" | "error" | "warning" | "info";
}

const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{6,12}$/;
const nonEmptyRegex = /^\S.*$/;

const SignUpPage: React.FC = () => {
  const navigate = useNavigate();
  const [username, setUsername] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [confirmPassword, setConfirmPassword] = useState<string>("");
  const [rolesName, setRolesName] = useState<string>("");
  const [badge, setBadge] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [snackbar, setSnackbar] = useState<SnackbarState>({
    open: false,
    message: "",
    severity: "success",
  });

  const handleSnackbarOpen = (message: string, severity: SnackbarState["severity"]) => {
    setSnackbar({ open: true, message, severity });
  };

  const handleSnackbarClose = () => {
    setSnackbar((prev) => ({ ...prev, open: false }));
  };

  const handleChangeRole = (event: SelectChangeEvent) => {
    setRolesName(event.target.value as string);
  };

  const handleSubmit = async () => {
    if (
      !nonEmptyRegex.test(username) ||
      !nonEmptyRegex.test(badge) ||
      !nonEmptyRegex.test(rolesName)
    ) {
      handleSnackbarOpen("Todos os campos devem ser preenchidos corretamente.", "error");
      return;
    }

    if (!passwordRegex.test(password)) {
      handleSnackbarOpen("A senha deve ter entre 6 e 12 caracteres, incluindo pelo menos uma letra e um número.", "error");
      return;
    }

    if (password !== confirmPassword) {
      handleSnackbarOpen("As senhas não coincidem.", "error");
      return;
    }

    setLoading(true);
    const userData: UserData = { username, password, rolesName, badge };
    const result = await createUser(userData);
    setLoading(false);

    if (result.success) {
      setUsername("");
      setPassword("");
      setConfirmPassword("");
      setRolesName("");
      setBadge("");
      handleSnackbarOpen("Usuário criado com sucesso!", "success");
    } else {
      handleSnackbarOpen(result.message || "Erro ao criar usuário.", "error");
    }
  };

  return (
    <Container maxWidth="xs" sx={{ display: "flex", flexDirection: "column", alignItems: "center", mt: 8 }}>
      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={handleSnackbarClose}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert onClose={handleSnackbarClose} severity={snackbar.severity} sx={{ width: "100%" }}>
          {snackbar.message}
        </Alert>
      </Snackbar>

      <Card sx={{ p: 4, width: "100%", display: "flex", flexDirection: "column", alignItems: "center" }}>
        <Typography variant="h4" component="h1" gutterBottom>Cadastro</Typography>
        <img src={Logo} alt="Logo" width="150px" style={{ marginBottom: "20px" }} />
        
        <TextField
          label="Nome"
          variant="outlined"
          fullWidth
          margin="normal"
          value={username}
          error={!!username && !nonEmptyRegex.test(username)}
          helperText={username && !nonEmptyRegex.test(username) ? "Nome não pode estar vazio." : ""}
          onChange={(e) => setUsername(e.target.value)}
        />
        
        <TextField
          label="Matricula"
          variant="outlined"
          fullWidth
          margin="normal"
          value={badge}
          error={!!badge && !nonEmptyRegex.test(badge)}
          helperText={badge && !nonEmptyRegex.test(badge) ? "Matricula não pode estar vazia." : ""}
          onChange={(e) => setBadge(e.target.value)}
        />
        
        <TextField
          label="Senha"
          type="password"
          variant="outlined"
          fullWidth
          margin="normal"
          value={password}
          error={!!password && !passwordRegex.test(password)}
          helperText={password && !passwordRegex.test(password) ? "A senha deve ter entre 6 e 12 caracteres, incluindo pelo menos uma letra e um número." : ""}
          onChange={(e) => setPassword(e.target.value)}
        />
        
        <TextField
          label="Confirmar Senha"
          type="password"
          variant="outlined"
          fullWidth
          margin="normal"
          value={confirmPassword}
          error={!!confirmPassword && password !== confirmPassword}
          helperText={confirmPassword && password !== confirmPassword ? "As senhas não coincidem." : ""}
          onChange={(e) => setConfirmPassword(e.target.value)}
        />
        
        <FormControl fullWidth variant="outlined" margin="normal" error={!nonEmptyRegex.test(rolesName)}>
          <InputLabel id="role-select-label">Função</InputLabel>
          <Select
            labelId="role-select-label"
            value={rolesName}
            onChange={handleChangeRole}
            label="Função"
          >
            <MenuItem value="">
              <em>Selecione uma Função</em>
            </MenuItem>
            <MenuItem value="administrador">Administrador</MenuItem>
            <MenuItem value="desenvolvedor">Desenvolvedor</MenuItem>
            <MenuItem value="tecnico">Técnico</MenuItem>
          </Select>
        </FormControl>

        <LoadingButton
          loading={loading}
          variant="contained"
          color="primary"
          fullWidth
          onClick={handleSubmit}
          sx={{ mt: 2 }}
        >
          Cadastrar
        </LoadingButton>
        
        <Button
          sx={{ mt: 2 }}
          fullWidth
          variant="outlined"
          onClick={() => navigate("/")}
        >
          Voltar para home
        </Button>
      </Card>
    </Container>
  );
};

export default SignUpPage;
