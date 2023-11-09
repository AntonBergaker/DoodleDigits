import React from "react"
import { createRoot } from "react-dom/client"
import { getDefaultSettings, getDefaultState } from "../saving/saving-defaults"
import { SaveStateData, SaveSettingsData } from "../saving/saving"
import { StateSavingScheduler } from "../saving/state-saving-scheduler"
import { mockElectronApi } from "../web/mock-electron"
import { MainWindow } from "../pages/calculator/main-window"

const [state, settings] = readStateAndSettings()
let preTheme: string | undefined = undefined
let themeLink: HTMLLinkElement | undefined = undefined

applySettings(settings)

function render() {
    const root = createRoot(document.getElementById("document-root"))
    root.render(<MainWindow settings={settings} state={state} onInput={onCalculatorInput} />)
}


function applySettings(settings: SaveSettingsData) {

    if (preTheme != settings.theme) {
        preTheme = settings.theme
        // Insert dark mode css theme
        if (themeLink != undefined) {
            themeLink.remove()
            themeLink = undefined
        }

        if (settings.theme == "dark") {
            themeLink = document.createElement("link")
            themeLink.rel = "stylesheet"
            themeLink.href = getStaticPath("themes/dark/dark.css");
            document.head.appendChild(themeLink)
        }
    }
}

function getStaticPath(path: string): string {
    if (WEB) {
        return "./" + path
    } else if (window.developmentMode) {
        return "../../static/" + path
    } else {
        return "../../renderer/static/" + path
    }
}


render()

if (!window.electronApi) {
    mockElectronApi();
}

window.electronApi.onUpdateSettings((event, settings) =>
    applySettings(settings)
)

const stateScheduler = new StateSavingScheduler(state);

function onCalculatorInput(input: string) {
    state.content = input;
    stateScheduler.scheduleSave();
}

window.electronApi.onSizeChanged( (event, data) => {
    state.window_dimensions.x = data.x
    state.window_dimensions.y = data.y
    stateScheduler.scheduleSave()
} )

function readStateAndSettings(): [SaveStateData, SaveSettingsData] {
    const urlParams = new URLSearchParams(window.location.search)
    let state
    const stateQuery = urlParams.get("state")
    if (stateQuery) {
        state = JSON.parse(atob(stateQuery))
    } else {
        console.log("State was not sent to window. Creating defaults.")
        state = getDefaultState()
    }
    const settingsQuery = urlParams.get("settings")
    let settings
    if (settingsQuery) {
        settings = JSON.parse(atob(settingsQuery))
    } else {
        console.log("Settings was not sent to window. Creating defaults.")
        settings = getDefaultSettings(
            () =>
                window.matchMedia &&
                window.matchMedia("(prefers-color-scheme: dark)").matches
        )
    }
    return [state, settings]
}
