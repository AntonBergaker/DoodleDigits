import { contextBridge, ipcRenderer } from "electron";
import { mainApi } from "./ipc/main-api";
import { rendererIpc } from "./ipc/renderer-ipc";

const ipcApi: { [key: string]: (arg: any) => void } = {};

for (const pair of Object.entries(mainApi)) {
    ipcApi[pair[0]] = (arg: any) => {
        ipcRenderer.send(pair[0], arg);
    }
}

const exposedApi = {
    ...ipcApi,
    ...rendererIpc
}

declare global {
    interface Window {
        electronApi: any
    }
}
window.electronApi = window.electronApi || {};

contextBridge.exposeInMainWorld('electronApi', exposedApi);