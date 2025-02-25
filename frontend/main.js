// const { app, BrowserWindow } = require('electron');
// const path = require('path');

// function createWindow() {
//   const mainWindow = new BrowserWindow({
//     width: 800,
//     height: 600,
//     webPreferences: {
//       nodeIntegration: true, // Removemos preload aqui
//     },
//   });

//   const startUrl = process.env.ELECTRON_START_URL || `file://${path.join(__dirname, 'build', 'index.html')}`;
//   console.log(`Loading URL: ${startUrl}`);
//   mainWindow.loadURL(startUrl);

//   // Abrir DevTools automaticamente
//   mainWindow.webContents.openDevTools();

//   mainWindow.on('closed', function () {
//     mainWindow = null;
//   });
// }

// app.on('ready', createWindow);

// app.on('window-all-closed', function () {
//   if (process.platform !== 'darwin') {
//     app.quit();
//   }
// });

// app.on('activate', function () {
//   if (mainWindow === null) {
//     createWindow();
//   }
// });
