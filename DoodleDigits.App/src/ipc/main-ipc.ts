import { BrowserWindow, IpcMainEvent, ipcMain } from "electron"
import { MainApiFunctionParameters, MainApiKey } from "./main-api"
import { RendererApiFunctionParameters, RendererApiKey } from "./renderer-api"

export function onIpc<T extends MainApiKey>(
    event: T,
    handler: (event: IpcMainEvent, data: MainApiFunctionParameters<T>) => void
) {
    ipcMain.on(event, handler)
}

export function sendIpc<T extends RendererApiKey>(
    window: BrowserWindow,
    key: T,
    data: RendererApiFunctionParameters<T>
) {
    window.webContents.send(key, data)
}
