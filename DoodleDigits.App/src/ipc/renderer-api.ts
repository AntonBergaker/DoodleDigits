import { IpcRendererEvent } from 'electron';
import { SaveStateData, SaveSettingsData } from '../saving/saving';

function api<T>(data: T) {
    
}

export const rendererApi = {
    
};

export type RendererApi = typeof rendererApi;
export type RendererApiKey = keyof RendererApi;
export type RendererApiFunctionParameters<T extends RendererApiKey> = Parameters<RendererApi[T]>[0]
