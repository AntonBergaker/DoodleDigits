import { SaveSettingsData } from "../saving/saving"

function api<T>(data: T) {
    console.log("heh")
}

export const rendererApi = {
    updateSettings: (data: SaveSettingsData) => {},
}

export type RendererApi = typeof rendererApi
export type RendererApiKey = keyof RendererApi
export type RendererApiFunctionParameters<T extends RendererApiKey> =
    Parameters<RendererApi[T]>[0]
