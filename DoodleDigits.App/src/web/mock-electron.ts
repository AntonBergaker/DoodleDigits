import { getDefaultSettings, getDefaultState } from "../saving/saving-defaults"
import {
    CalculatorAngleUnit,
    CalculatorSettings,
    SaveStateData,
} from "../saving/saving"

let maybeSettings: CalculatorSettings | null = null
const settingsJson = localStorage.getItem("calculator_settings")
if (settingsJson) {
    maybeSettings = JSON.parse(settingsJson)
}
const settings =
    maybeSettings ??
    getDefaultSettings(
        () =>
            window.matchMedia &&
            window.matchMedia("(prefers-color-scheme: dark)").matches
    )

let maybeState: SaveStateData | null = null
const stateJson = localStorage.getItem("calculator_state")
if (stateJson) {
    maybeState = JSON.parse(stateJson)
}
let state = maybeState ?? getDefaultState()

export function mockElectronApi() {
    const eventMock = () => () => {}
    const ipcMock = () => {}

    const settingsEvent = new SimpleEvent<CalculatorSettings>()

    window.electronApi = {
        onSizeChanged: eventMock,
        onUpdateSettings: (func) =>
            settingsEvent.addListener((data) => func(null, data)),
        onFocusedChanged: eventMock,
        onUpdateAvailableThemes: eventMock,

        saveState: (newState: SaveStateData) => {
            state = newState
            saveState()
        },
        updateAngleUnit: (angleUnit: CalculatorAngleUnit) => {
            settings.angle_unit = angleUnit
            settingsEvent.invoke(settings)
            saveSettings()
        },
        updateTheme: (theme: string) => {
            settings.theme = theme
            settingsEvent.invoke(settings)
            saveSettings()
        },
    }
}

function saveSettings() {
    localStorage.setItem("calculator_settings", JSON.stringify(settings))
}
function saveState() {
    localStorage.setItem("calculator_state", JSON.stringify(state))
}

export function getStoredStateAndSettings(): [
    SaveStateData,
    CalculatorSettings,
] {
    return [state, settings]
}

class SimpleEvent<T> {
    listeners: ((args: T) => void)[]

    constructor() {
        this.listeners = []
    }

    addListener(func: (args: T) => void) {
        this.listeners.push(func)
        return () => {
            const index = this.listeners.indexOf((x) => func)
            if (index >= 0) {
                this.listeners.splice(index, 1)
            }
        }
    }

    invoke(args: T) {
        for (const listener of this.listeners) {
            listener(args)
        }
    }
}
