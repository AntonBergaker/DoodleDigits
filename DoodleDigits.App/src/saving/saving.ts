import { promises as fs } from "fs"
import { getDefaultSettings, getDefaultState } from "./saving-defaults"
import { nativeTheme } from "electron"

export type SaveStateData = {
    content: string
    cursor_index: number
    window_dimensions: {
        x: number
        y: number
    }
}

export type CalculatorAngleUnit = "radians" | "degrees"

export type CalculatorSettings = {
    theme: string
    zoom: number
    always_on_top: boolean
    angle_unit: CalculatorAngleUnit
}

export const directory =
    (process.env.APPDATA ||
        (process.platform == "darwin"
            ? process.env.HOME + "/Library/Preferences"
            : process.env.HOME + "/.local/share")) + "/Doodle Digits"

async function loadFile(filename: string): Promise<string | null> {
    try {
        return await fs.readFile(`${directory}/${filename}`, "utf8")
    } catch (error) {
        return null
    }
}
async function writeFile(filename: string, content: string): Promise<void> {
    await fs.writeFile(`${directory}/${filename}`, content, "utf8")
}

export async function loadStateOrDefault(): Promise<SaveStateData> {
    let state = getDefaultState()

    const stateJson = await loadFile("state.json")
    if (stateJson != null) {
        const loadedState = JSON.parse(stateJson)
        fixLegacyState(loadedState)
        state = { ...state, ...loadedState }
    }
    return state
}

export async function loadSettingsOrDefault(): Promise<CalculatorSettings> {
    let settings = getDefaultSettings(() => nativeTheme.shouldUseDarkColors)

    const settingsJson = await loadFile("settings.json")
    if (settingsJson != null) {
        const loadedSettings = JSON.parse(settingsJson)
        fixLegacySettings(loadedSettings)
        settings = { ...settings, ...loadedSettings }
    }

    return settings
}

export function saveState(state: SaveStateData) {
    writeFile("state.json", JSON.stringify(state))
}

export function saveSettings(settings: CalculatorSettings) {
    writeFile("settings.json", JSON.stringify(settings))
}

/**
 * Cleans up the file and ports legacy settings to be standardized
 * @param state
 */
function fixLegacySettings(settings: CalculatorSettings) {
    const untypedSettings = settings as any
    if (typeof untypedSettings.dark_mode == "boolean") {
        settings.theme = untypedSettings.dark_mode ? "dark" : "default"
        delete untypedSettings.dark_mode
    }
    if ("force_on_top" in untypedSettings) {
        settings.always_on_top = untypedSettings.force_on_top
        delete untypedSettings.force_on_top
    }
}
/**
 * Cleans up the file and ports legacy settings to be standardized
 * @param state
 */
function fixLegacyState(state: SaveStateData) {
    // Fix capitalization on coordinates
    const dim = state.window_dimensions as any
    if ("X" in dim && typeof dim.X == "number") {
        dim.x = dim.X
        delete dim.X
    }
    if ("Y" in dim && typeof dim.Y == "number") {
        dim.y = dim.Y
        delete dim.Y
    }
}
