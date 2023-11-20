import React, { useEffect, useRef, useState } from "react"
import { calculate } from "../../calculator/calculator"
import "./calculator-page.css"
import { ResultView } from "../../components/result-view"
import { PageMeasurer } from "../../calculator/page-measure"
import { InputText } from "../../components/input-text"
import { CalculatorResult } from "../../calculator/calculator-result"
import { SaveStateData, CalculatorSettings } from "../../saving/saving"

type CalculatorPageProps = {
    state: SaveStateData
    settings: CalculatorSettings
    onInput?: (string: string) => void
}

export function CalculatorPage(props: CalculatorPageProps) {
    const [result, setResult] = useState(
        undefined as CalculatorResult | undefined
    )
    const [input, setInput] = useState(props.state.content)

    useEffect(() => {
        const a = async () => {
            const result = await calculate(input, props.settings)
            setResult(result)
            if (props.onInput) {
                props.onInput(input)
            }
        }
        a()
    }, [input, props.settings.angle_unit])

    const ref = useRef<HTMLDivElement>()
    const pageMeasurer = new PageMeasurer(ref)

    return (
        <div className="container">
            <InputText
                ref={ref}
                onInput={setInput}
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
