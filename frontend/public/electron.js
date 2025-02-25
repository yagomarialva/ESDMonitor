const electron = require("electron");
const path = require("path");

const app = electron.app;
const BrowserWindow = electron.BrowserWindow;

let mainWindow;
let loadingWindow;

function createWindow() {
  // Create the loading window
  loadingWindow = new BrowserWindow({
    width: 400,
    height: 300,
    frame: false,
    transparent: true,
    alwaysOnTop: true,
    webPreferences: { nodeIntegration: true, contextIsolation: false },
  });

  // Load the loading.html file
  loadingWindow.loadFile(path.join(__dirname, "loading.html"));

  // Create the main browser window
  mainWindow = new BrowserWindow({
    width: 1400,
    height: 800,
    show: false, // Initially hide the main window
    webPreferences: { nodeIntegration: true, contextIsolation: false },
  });

  // Load the main content
  mainWindow.loadFile(path.join(__dirname, "../build/index.html"));

  // Send a message to update the span element's text
  loadingWindow.webContents.on("did-finish-load", () => {
    loadingWindow.webContents.send("onInitialized");
  });

  mainWindow.webContents.on("did-finish-load", () => {
    // When the main window is loaded, destroy the loading window and show the main window
    if (loadingWindow) {
      loadingWindow.webContents.send("onFinished",100);
      setTimeout(()=>{
      loadingWindow.close();
      mainWindow.show();
    }
      ,1000);
    }else mainWindow.show();
   
  });
}

app.on("ready", createWindow);
