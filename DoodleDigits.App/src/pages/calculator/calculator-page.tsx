import React, { useEffect, useRef, useState } from "react"
import { calculate } from "../../calculator/calculator"
import "./calculator-page.css"
import { ResultView } from "../../components/result-view"
import { PageMeasurer } from "../../calculator/page-measure"
import { InputText } from "../../components/input-text"
import { CalculatorResult } from "../../calculator/calculator-result"
import { SaveStateData, SaveSettingsData } from "../../saving/saving"

type CalculatorPageSettings = {
    state: SaveStateData
    settings: SaveSettingsData
    onInput?: (string: string) => void
}

export function CalculatorPage(props: CalculatorPageSettings) {
    const [result, setResult] = useState(
        undefined as CalculatorResult | undefined
    )

    useEffect(() => {
        onInput(props.state.content)
    }, [])

    async function onInput(input: string) {
        const result = await calculate(input)
        setResult(result)
        if (props.onInput) {
            props.onInput(input);
        }
    }

    const ref = useRef<HTMLDivElement>()
    const pageMeasurer = new PageMeasurer(ref)

    return (
        <div className="container">
            <InputText
                ref={ref}
                onInput={onInput}
                defaultText={props.state.content}
            />
            <div className="annotations"></div>
            <div>
                {result != undefined && (
                    <ResultView
                        result={result}
                        pageMeasurer={pageMeasurer}></ResultView>
                )}
            </div>
        </div>
    )
}
