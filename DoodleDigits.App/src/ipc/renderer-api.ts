import { SaveSettingsData } from "../saving/saving"

/* eslint-disable @typescript-eslint/no-empty-function */
export const rendererApi = {
    updateSettings: (data: SaveSettingsData) => {},
    sizeChanged: (data: { x: number; y: number }) => {},
    focusedChanged: (data: { focused: boolean }) => {},
}
/* eslint-enable @typescript-eslint/no-empty-function */

export type RendererApi = typeof rendererApi
export type RendererApiKey = keyof RendererApi
export type RendererApiFunctionParameters<T extends RendererApiKey> =
    Parameters<RendererApi[T]>[0]
