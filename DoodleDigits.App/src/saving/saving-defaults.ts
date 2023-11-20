import { CalculatorSettings, SaveStateData } from "./saving"

export function getDefaultState(): SaveStateData {
    return {
        content: "",
        cursor_index: 0,
        window_dimensions: { x: 800, y: 600 },
    }
}

export function getDefaultSettings(
    isDarkModeFunction: () => boolean
): CalculatorSettings {
    return {
        theme: isDarkModeFunction() ? "dark" : "default",
        always_on_top: false,
        zoom: 0,
        angle_unit: "radians",
    }
}
