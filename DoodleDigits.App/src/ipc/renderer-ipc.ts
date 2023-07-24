import { IpcRendererEvent, ipcRenderer } from "electron"
import {
    rendererApi,
    RendererApi,
    RendererApiFunctionParameters,
} from "./renderer-api"

type Handler = (event: IpcRendererEvent, data: any) => void
const rendererIpc: { [key: string]: (handler: Handler) => void } = {}

for (const key in rendererApi) {
    rendererIpc[`on${capitlizeFirst(key)}`] = function (handler: Handler) {
        ipcRenderer.on(key, handler)
    }
}

function capitlizeFirst(string: string) {
    return string.charAt(0).toUpperCase() + string.slice(1)
}

export type OnRendererApi = {
    [TKey in keyof RendererApi as `on${Capitalize<TKey>}`]: (
        handler: (
            event: IpcRendererEvent,
            data: RendererApiFunctionParameters<TKey>
        ) => void
    ) => void
}

export { rendererIpc }
