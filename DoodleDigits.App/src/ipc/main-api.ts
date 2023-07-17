import { IpcMainEvent } from 'electron';
import { SaveStateData } from '../saving/saving';
import { SaveSettingsData } from '../saving/saving';

export const mainApi = {
    saveState: (event: IpcMainEvent, state: SaveStateData) => {

    },
    saveSettings: (event: IpcMainEvent, state: SaveSettingsData) => {

    },
}

export type MainApi = typeof mainApi;
export type MainApiKey = keyof MainApi;
export type MainApiFunctionParameters<T extends MainApiKey> = Parameters<MainApi[T]>[1]
