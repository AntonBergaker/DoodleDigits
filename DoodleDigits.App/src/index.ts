import { app, BrowserWindow, ipcMain } from "electron"
import {
    directory,
    loadSettingsOrDefault,
    loadStateOrDefault,
    saveSettings,
    SaveSettingsData,
    saveState,
    SaveStateData,
} from "./saving/saving"
import contextMenu from "electron-context-menu"
import { onIpc, sendIpc } from "./ipc/main-ipc"

process.env.DEV_MODE = process.argv.includes("--dev") ? "true" : "false"

// This allows TypeScript to pick up the magic constants that's auto-generated by Forge's Webpack
// plugin that tells the Electron app where to look for the Webpack-bundled app code (depending on
// whether you're running in development or production).
declare const MAIN_WINDOW_WEBPACK_ENTRY: string
declare const MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY: string

// Handle creating/removing shortcuts on Windows when installing/uninstalling.
if (require("electron-squirrel-startup")) {
    app.quit()
}

const savedStatePromise = loadStateOrDefault()
let savedState: SaveStateData
const savedSettingsPromise = loadSettingsOrDefault()
let savedSettings: SaveSettingsData

contextMenu({
    showSelectAll: true,
    showSearchWithGoogle: false,
    showInspectElement: false,

    append: (defaultActions, parameters, browserWindow) => [
        defaultActions.separator(),
        {
            label: "Dark Mode",
            type: "checkbox",
            checked: savedSettings.theme == "dark",
            click: (item, window, event) => {
                if (savedSettings.theme == "dark") {
                    savedSettings.theme = "default"
                } else {
                    savedSettings.theme = "dark"
                }
                item.checked = savedSettings.theme == "dark"
                sendIpc(window, "updateSettings", savedSettings)
                saveSettings(savedSettings)
            },
        },
    ],
})

const createWindow = async () => {
    savedState = await savedStatePromise
    savedSettings = await savedSettingsPromise

    const size = savedState.window_dimensions

    // Create the browser window.
    const mainWindow = new BrowserWindow({
        height: size.y,
        width: size.x,
        webPreferences: {
            preload: MAIN_WINDOW_PRELOAD_WEBPACK_ENTRY,
        },
        autoHideMenuBar: true,
        darkTheme: savedSettings.theme.includes("dark"),
        icon: "./root/icon.ico",
    })

    mainWindow.on("resize", () => {
        const [x, y] = mainWindow.getSize()
        sendIpc(mainWindow, "sizeChanged", { x, y })
    })

    const query = `?state=${btoa(JSON.stringify(savedState))}&settings=${btoa(
        JSON.stringify(savedSettings)
    )}`
    mainWindow.loadURL(MAIN_WINDOW_WEBPACK_ENTRY + query)
}

onIpc("saveState", (event, state) => {
    saveState(state)
})

app.setPath("userData", directory + "/electron")

// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
// Some APIs can only be used after this event occurs.
app.on("ready", createWindow)

// Quit when all windows are closed, except on macOS. There, it's common
// for applications and their menu bar to stay active until the user quits
// explicitly with Cmd + Q.
app.on("window-all-closed", () => {
    if (process.platform !== "darwin") {
        app.quit()
    }
})

app.on("activate", () => {
    // On OS X it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if (BrowserWindow.getAllWindows().length === 0) {
        createWindow()
    }
})
