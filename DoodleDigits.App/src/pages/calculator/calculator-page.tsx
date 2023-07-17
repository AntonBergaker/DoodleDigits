import React, { useRef, useState } from "react";
import { calculate } from "../../calculator/calculator";
import "./calculator-page.css";
import { ResultView } from "../../components/result-view";
import { PageMeasurer } from "../../calculator/page-measure";
import { InputText } from "../../components/input-text";
import { CalculatorResult } from "../../calculator/calculator-result";
import { getDefaultSettings, getDefaultState } from "../../saving/saving-defaults";
import { SaveStateData, SaveSettingsData } from "../../saving/saving";


export function CalculatorPage() {

    const [result, setResult] = useState(undefined as CalculatorResult | undefined);

    async function onInput(input: string) {
        const result = await calculate(input);
        setResult(result);
    }

    const ref = useRef<HTMLDivElement>();
    const pageMeasurer = new PageMeasurer(ref);
    const [state, settings] = readStateAndSettings();

    return (
        <div className="container">
            <InputText ref={ref} onInput={onInput} defaultText={state.content}/>
            <div className="annotations"></div>
            <div>
                {result != undefined && <ResultView result={result} pageMeasurer={pageMeasurer}></ResultView>}
            </div>
        </div>
    )
}

function readStateAndSettings(): [SaveStateData, SaveSettingsData] {
    const urlParams = new URLSearchParams(window.location.search);
    let state;
    const stateQuery = urlParams.get('state');
    if (stateQuery) {
        state = JSON.parse(atob(stateQuery));
    } else {
        console.log("State was not sent to window. Creating defaults.")
        state = getDefaultState();
    }
    const settingsQuery = urlParams.get('settings');
    let settings;
    if (settingsQuery) {
        settings = JSON.parse(atob(settingsQuery));
    } else {
        console.log("Settings was not sent to window. Creating defaults.")
        settings = getDefaultSettings();
    }
    return [state, settings];

}