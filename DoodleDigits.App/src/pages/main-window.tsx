import React, { useEffect, useState } from "react"
import { TitleBar } from "../components/title-bar"
import { TitleBarButton } from "../components/title-bar-button"
import { CalculatorPage } from "./calculator/calculator-page"
import { SaveStateData, CalculatorSettings } from "../saving/saving"
import "./main-window.css"
import { ThemeData } from "../saving/themes"

type MainWindowProps = {
    state: SaveStateData
    defaultSettings: CalculatorSettings
    customTitlebar: boolean
    availableThemes: ThemeData[]
    onInput?: (string: string) => void
}

export function MainWindow(props: MainWindowProps) {
    const [settings, setSettings] = useState(props.defaultSettings)

    useEffect(() => {
        window.electronApi.onUpdateSettings((event, settings) =>
            setSettings(settings)
        )
    }, [])

    return (
        <div className="mainWindow">
            {props.customTitlebar && (
                <TitleBar
                    availableThemes={props.availableThemes}
                    settings={settings}
                />
            )}
            {!props.customTitlebar && (
                <TitleBarButton
                    availableThemes={props.availableThemes}
                    settings={settings}
                />
            )}
            <CalculatorPage
                state={props.state}
                settings={settings}
                showTitlebar={props.customTitlebar}
                onInput={props.onInput}
            />
        </div>
    )
}
