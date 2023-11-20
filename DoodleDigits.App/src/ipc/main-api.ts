import { IpcMainEvent } from "electron"
import { CalculatorAngleUnit, SaveStateData } from "../saving/saving"
import { CalculatorSettings } from "../saving/saving"

/* eslint-disable @typescript-eslint/no-empty-function */
export const mainApi = {
    saveState: (state: SaveStateData) => {},
    updateAngleUnit: (angleUnit: CalculatorAngleUnit) => {},
}
/* eslint-enable @typescript-eslint/no-empty-function */

export type MainApi = typeof mainApi
export type MainApiKey = keyof MainApi
export type MainApiFunctionParameters<T extends MainApiKey> = Parameters<
    MainApi[T]
>[0]
