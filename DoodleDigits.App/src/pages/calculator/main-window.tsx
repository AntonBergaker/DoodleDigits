import React from "react";
import { TitleBar } from "../../components/title-bar";
import { CalculatorPage } from "./calculator-page";
import { SaveStateData, SaveSettingsData } from "../../saving/saving";

type MainWindowProps = {
    state: SaveStateData
    settings: SaveSettingsData
    onInput?: (string: string) => void
}

export function MainWindow(props: MainWindowProps) {
    return (
        <div className="container">
            <TitleBar/>
            <CalculatorPage state={props.state} settings={props.settings} onInput={props.onInput}/>
        </div>
    )
}