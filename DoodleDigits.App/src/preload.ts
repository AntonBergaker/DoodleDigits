import { contextBridge, ipcRenderer } from "electron"
import { mainApi } from "./ipc/main-api"
import { RendererIpc, rendererIpc } from "./ipc/renderer-ipc"

const ipcApi: { [key: string]: (arg: any) => void } = {}

for (const pair of Object.entries(mainApi)) {
    ipcApi[pair[0]] = (arg: any) => {
        ipcRenderer.send(pair[0], arg)
    }
}

const exposedApi = {
    ...ipcApi,
    ...rendererIpc,
}

contextBridge.exposeInMainWorld("electronApi", exposedApi)

contextBridge.exposeInMainWorld(
    "developmentMode",
    process.env.DEV_MODE == "true"
)

declare global {
    interface Window {
        electronApi: RendererIpc
        developmentMode: boolean
    }
}
