import { CalculatorAngleUnit, SaveStateData } from "../saving/saving"

/* eslint-disable @typescript-eslint/no-empty-function */
/* eslint-disable @typescript-eslint/no-unused-vars */
export const mainApi = {
    saveState: (state: SaveStateData) => {},
    updateAngleUnit: (angleUnit: CalculatorAngleUnit) => {},
    updateTheme: (theme: string) => {},
}
/* eslint-enable @typescript-eslint/no-empty-function */
/* eslint-enable @typescript-eslint/no-unused-vars */

export type MainApi = typeof mainApi
export type MainApiKey = keyof MainApi
export type MainApiFunctionParameters<T extends MainApiKey> = Parameters<
    MainApi[T]
>[0]
