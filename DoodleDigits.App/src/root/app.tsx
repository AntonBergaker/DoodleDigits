import React, { useEffect, useState } from "react"
import { createRoot } from "react-dom/client"
import { getDefaultSettings, getDefaultState } from "../saving/saving-defaults"
import { SaveStateData, CalculatorSettings } from "../saving/saving"
import { StateSavingScheduler } from "../saving/state-saving-scheduler"
import {
    getStoredStateAndSettings,
    mockElectronApi,
} from "../web/mock-electron"
import { MainWindow } from "../pages/main-window"
import { ThemeData, getDarkTheme, getDefaultTheme } from "../saving/themes"

const urlParams = new URLSearchParams(window.location.search)

let stateFunc = () => readStateAndSettings(urlParams)
if (WEB) {
    mockElectronApi()
    stateFunc = () => getStoredStateAndSettings()
}

const [state, defaultSettings] = stateFunc()
const customTitlebar = urlParams.get("titlebar") == "true"

let availableThemes: ThemeData[] = [getDefaultTheme(), getDarkTheme()]

// If passed an extra theme, add it
{
    const themeQuery = urlParams.get("theme")
    if (themeQuery) {
        availableThemes.push(JSON.parse(atob(themeQuery)))
    }
}

let preTheme: string | undefined = undefined
let themeLink: HTMLLinkElement | undefined = undefined

applySettings(defaultSettings)

function render() {
    const root = createRoot(document.getElementById("document-root"))

    root.render(<ReactRoot />)
}

function ReactRoot() {
    const [data, setAvailableThemes] = useState<ThemeData[]>([
        getDefaultTheme(),
        getDarkTheme(),
    ])
    availableThemes = data

    useEffect(() => {
        const unsub = window.electronApi.onUpdateAvailableThemes(
            (event, themes) => {
                setAvailableThemes(themes)
            }
        )
        return () => unsub()
    }, [])

    return (
        <MainWindow
            defaultSettings={defaultSettings}
            state={state}
            customTitlebar={customTitlebar}
            availableThemes={availableThemes}
            onInput={onCalculatorInput}
        />
    )
}

function applySettings(settings: CalculatorSettings) {
    const theme = availableThemes.find((x) => x.id == settings.theme)
    if (!theme) {
        return
    }
    if (preTheme != theme.id) {
        preTheme = theme.id

        // Remove previous css links
        if (themeLink != undefined) {
            themeLink.remove()
            themeLink = undefined
        }

        if (theme.styleSheet) {
            themeLink = document.createElement("link")
            themeLink.rel = "stylesheet"
            themeLink.href = getStaticFilesPath(`themes/${theme.styleSheet}`)
            document.head.appendChild(themeLink)
        }
    }
}

render()

window.electronApi.onUpdateSettings((event, settings) => {
    applySettings(settings)
})

const stateScheduler = new StateSavingScheduler(state)

function onCalculatorInput(input: string) {
    state.content = input
    stateScheduler.scheduleSave()
}

window.electronApi.onSizeChanged((event, data) => {
    state.window_dimensions.x = data.x
    state.window_dimensions.y = data.y
    stateScheduler.scheduleSave()
})

window.electronApi

function getStaticFilesPath(path: string): string {
    if (WEB) {
        return "./" + path
    } else if (window.developmentMode) {
        return "../../static/" + path
    } else {
        return window.process_resourcesPath + "/../resources/" + path
    }
}

function readStateAndSettings(
    urlParams: URLSearchParams
): [SaveStateData, CalculatorSettings] {
    let state
    const stateQuery = urlParams.get("state")
    if (stateQuery) {
        state = JSON.parse(atob(stateQuery))
    } else {
        state = getDefaultState()
    }

    let settings
    const settingsQuery = urlParams.get("settings")
    if (settingsQuery) {
        settings = JSON.parse(atob(settingsQuery))
    } else {
        settings = getDefaultSettings(
            () =>
                window.matchMedia &&
                window.matchMedia("(prefers-color-scheme: dark)").matches
        )
    }

    return [state, settings]
}
