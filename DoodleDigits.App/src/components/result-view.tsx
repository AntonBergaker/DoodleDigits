import React from "react";
import { PageMeasurer, Position } from "../calculator/page-measure";
import "./result-view.css";
import { CalculatorResult, Result, Value } from "../calculator/calculator-result";

export type ResultsViewProps = {
    result: CalculatorResult;
    pageMeasurer: PageMeasurer;
};

export function ResultView(props: ResultsViewProps) {
    const resultsPerLine: Map<number, string[]> = new Map();
    for (const result of props.result.results) {
        var text = getResultText(result);

        if (text == null) {
            continue;
        }

        var line = props.pageMeasurer.getCharacterLine(getRange(result.range)[1]);

        let array: string[];
        if (resultsPerLine.has(line)) {
            array = resultsPerLine.get(line);
        } else {
            array = [];
            resultsPerLine.set(line, array);
        }
        array.push(text);
    }

    let index = 0;
    const resultDoms: JSX.Element[] = [];
    for (const [line, results] of resultsPerLine) {
        const pos = props.pageMeasurer.getLinePosition(line);
        const resultDom = <p style={{left: `${pos[0]}px`, top: `${pos[1]}px`}} className="result" key={index++}>{results.join(", ")}</p>;
        resultDoms.push(resultDom);
    }

    return (<>
        {resultDoms}
    </>);
}

function getResultText(result: Result): string | null {
    if (result.type == "value") {
        if (result.value.trivially_achieved) {
            return null;
        }
        return getValueText(result.value, true);
    }
    if (result.type == "error") {
        return result.message;
    }
    if (result.type == "conversion") {
        return `converted ${getValueText(result.previous, false)} to ${getValueText(result.new, false)}`
    }
    return null;
}

function getRange(range: string): Position {
    const split = range.split('-').map(x => parseInt(x));
    return split as Position;
}

function getValueText(value: Value, includeEqualSign: boolean) : string | null {
    let equalSign = " = ";
    let leadsToSign = " → ";

    if (includeEqualSign == false) {
        equalSign = "";
        leadsToSign = "";
    }

    if (value.type == "real") {
        return equalSign + value.value;
    }
    if (value.type == "boolean") {
        return leadsToSign + (value.value ? "true" : "false");
    }
    if (value.type == "too_big") {
        if (value.value == "negative") {
            return leadsToSign + "a huge negative number"
        }
        if (value.value == "negative_infinity") {
            return equalSign + "-∞"
        }
        if (value.value == "positive") {
            return leadsToSign + "a huge number"
        }
        if (value.value == "positive_infinity") {
            return equalSign + "∞"
        }
    }
    if (value.type == "undefined") {
        if (value.undefined_type == "undefined") {
            return "undefined";
        }
        return null;
    }
    if (value.type == "matrix") {
        return "hmm";
    }

    return null;
}