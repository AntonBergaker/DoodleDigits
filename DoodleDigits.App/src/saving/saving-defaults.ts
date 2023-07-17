import { SaveSettingsData, SaveStateData } from "./saving"

export function getDefaultState(): SaveStateData {
    return {
        content: "",
        cursor_index: 0,
        window_dimensions: {x: 800, y: 600}
    }
}

export function getDefaultSettings(): SaveSettingsData {
    return {
        dark_mode: "default",
        force_on_top: false,
        zoom: 0,
    }
}