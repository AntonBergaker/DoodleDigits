import React from "react"
import { createRoot } from "react-dom/client"
import { CalculatorPage } from "../pages/calculator/calculator-page"
import { getDefaultSettings, getDefaultState } from "../saving/saving-defaults"
import { SaveStateData, SaveSettingsData } from "../saving/saving"

function render() {
    const root = createRoot(document.getElementById("document-root"))
    const [state, settings] = readStateAndSettings()

    applySettings(settings)

    root.render(<CalculatorPage settings={settings} state={state} />)
}

let themeLink: HTMLLinkElement | undefined = undefined

function applySettings(settings: SaveSettingsData) {
    // Insert dark mode css theme
    if (themeLink != undefined) {
        themeLink.remove()
        themeLink = undefined
    }

    if (settings.theme == "dark") {
        themeLink = document.createElement("link")
        themeLink.rel = "stylesheet"
        if (window.developmentMode) {
            themeLink.href = "../../static/themes/dark/dark.css"
        } else {
            themeLink.href = "../../renderer/static/themes/dark/dark.css"
        }
        document.head.appendChild(themeLink)
    }
}

render()

window.electronApi.onUpdateSettings((event, settings) =>
    applySettings(settings)
)

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
