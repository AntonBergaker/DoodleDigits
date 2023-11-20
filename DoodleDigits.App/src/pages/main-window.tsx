import React, { useEffect, useState } from "react"
import { TitleBar } from "../components/title-bar"
import { CalculatorPage } from "./calculator/calculator-page"
import { SaveStateData, CalculatorSettings } from "../saving/saving"

type MainWindowProps = {
    state: SaveStateData
    defaultSettings: CalculatorSettings
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
        <div className="container">
            <TitleBar angleUnit={settings.angle_unit} />
            <CalculatorPage
                state={props.state}
                settings={settings}
                onInput={props.onInput}
            />
        </div>
    )
}
