import { SaveSettingsData, SaveStateData } from "./saving"

export function getDefaultState(): SaveStateData {
    return {
        content: "",
        cursor_index: 0,
        window_dimensions: { x: 800, y: 600 },
    }
}

export function getDefaultSettings(
    isDarkModeFunction: () => boolean
): SaveSettingsData {
    return {
        theme: isDarkModeFunction() ? "dark" : "default",
        always_on_top: false,
        zoom: 0,
    }
}
