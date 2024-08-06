import { ThemeData } from "../saving/themes"
import { CalculatorSettings } from "../saving/saving"

/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable @typescript-eslint/no-unused-vars */
export const rendererApi = {
    updateSettings: (data: CalculatorSettings) => {},
    sizeChanged: (data: { x: number; y: number }) => {},
    focusedChanged: (data: { focused: boolean }) => {},
    updateAvailableThemes: (data: ThemeData[]) => {},
}
/* eslint-enable @typescript-eslint/no-empty-function */
/* eslint-enable @typescript-eslint/no-unused-vars */

export type RendererApi = typeof rendererApi
export type RendererApiKey = keyof RendererApi
export type RendererApiFunctionParameters<T extends RendererApiKey> =
    Parameters<RendererApi[T]>[0]
