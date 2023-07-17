import {promises as fs} from 'fs';
import { getDefaultSettings, getDefaultState } from './saving-defaults';

export type SaveStateData = {
    content: string,
    cursor_index: number,
    window_dimensions: {
        x: number,
        y: number
    }
}

export type SaveSettingsData = {
    dark_mode: "dark" | "light" | "default",
    zoom: number,
    force_on_top: boolean
}

const directory = 
    (process.env.APPDATA || (process.platform == 'darwin' ? process.env.HOME + '/Library/Preferences' : process.env.HOME + "/.local/share")) + 
    "/Doodle Digits";

async function loadFile(filename: string): Promise<string | null> {
    try {
        return await fs.readFile(`${directory}/${filename}`, 'utf8');
    } catch (error) {
        return null;
    }
}
async function writeFile(filename: string, content: string): Promise<void> {
    await fs.writeFile(`${directory}/${filename}`, content, 'utf8');
}

export async function loadStateOrDefault(): Promise<SaveStateData> {
    const stateJson = await loadFile("state.json");
    console.log(stateJson);
    let state: SaveStateData;
    if (stateJson == null) {
        state = getDefaultState();
    } else {
        state = JSON.parse(stateJson);
    }
    fixLegacyState(state);
    return state;
}

export async function loadSettingsOrDefault(): Promise<SaveSettingsData> {
    const settingsJson = await loadFile("settings.json");
    let settings: SaveSettingsData;
    if (settingsJson == null) {
        settings = getDefaultSettings();
    } else {
        settings = JSON.parse(settingsJson);
    }
    fixLegacySettings(settings);
    return settings;
}

/**
 * Cleans up the file and ports legacy settings to be standardized
 * @param state 
 */
function fixLegacySettings(settings: SaveSettingsData) {
    if (typeof settings.dark_mode == "boolean") {
        settings.dark_mode = settings.dark_mode ? "dark" : "light"
    }
}
/**
 * Cleans up the file and ports legacy settings to be standardized
 * @param state 
 */
function fixLegacyState(state: SaveStateData) {
    // Fix capitalization on coordinates
    const dim = state.window_dimensions as any;
    if ("X" in dim && typeof dim.X == "number") {
        dim.x = dim.X;
        delete dim.X;
    }
    if ("Y" in dim && typeof dim.Y == "number") {
        dim.y = dim.Y;
        delete dim.Y;
    }
}