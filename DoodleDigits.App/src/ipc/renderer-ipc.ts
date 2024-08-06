import { IpcRendererEvent, ipcMain, ipcRenderer } from "electron"
import {
    rendererApi,
    RendererApi,
    RendererApiFunctionParameters,
} from "./renderer-api"
import { MainApi, MainApiFunctionParameters, mainApi } from "./main-api"

type Handler = (event: IpcRendererEvent, data: any) => void
const rendererEventIpc: { [key: string]: (handler: Handler) => void } = {}

type RendererEventIpc = {
    [TKey in keyof RendererApi as `on${Capitalize<TKey>}`]: (
        handler: (
            event: IpcRendererEvent,
            data: RendererApiFunctionParameters<TKey>
        ) => void
    ) => () => void
}

for (const key in rendererApi) {
    rendererEventIpc[`on${capitalizeFirst(key)}`] = function (
        handler: Handler
    ) {
        ipcRenderer.on(key, handler)
        return () => ipcRenderer.off(key, handler)
    }
}

function capitalizeFirst(string: string) {
    return string.charAt(0).toUpperCase() + string.slice(1)
}

const rendererCallIpc: { [key: string]: (data: any) => void } = {}
for (const key in mainApi) {
    rendererCallIpc[key] = function (data: any) {
        ipcRenderer.send(key, data)
    }
}

type RendererCallIpc = {
    [TKey in keyof MainApi]: (data: MainApiFunctionParameters<TKey>) => void
}

export type RendererIpc = RendererEventIpc & RendererCallIpc

const rendererIpc = {
    ...rendererEventIpc,
    ...rendererCallIpc,
} as RendererIpc

export { rendererIpc }
