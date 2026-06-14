import React from "react"
import { PageMeasurer, Position } from "../calculator/page-measure"
import "./result-view.css"
import {
    CalculatorResult,
    MatrixDimension,
    MatrixValue,
    Result,
    Value,
} from "../calculator/calculator-result"

export type ResultsViewProps = {
    result: CalculatorResult
    pageMeasurer: PageMeasurer
}

export function ResultView(props: ResultsViewProps) {
    const resultsPerLine: Map<number, Result[]> = new Map()
    for (const result of props.result.results) {
        const line = props.pageMeasurer.getCharacterLine(
            getRange(result.range)[1]
        )

        let array: Result[]
        if (resultsPerLine.has(line)) {
            array = resultsPerLine.get(line)
        } else {
            array = []
            resultsPerLine.set(line, array)
        }
        array.push(result)
    }

    let index = 0
    const resultDoms: JSX.Element[] = []
    for (let [line, results] of resultsPerLine) {
        const pos = props.pageMeasurer.getLinePosition(line)

        // If everything on the line is a trivial value, ignore everything
        if (results.every(x => x.type == "value" && (x.value.triviality == "trivial" || x.value.triviality == "trivial_side_effect"))) {
            continue;
        }
        // If not, hide side effects
        results = results.filter(x => x.type == "value" && x.value.triviality != "trivial_side_effect")

        const resultDom = (
            <p
                style={{ left: `${pos[0]}px`, top: `${pos[1]}px` }}
                className="result"
                key={index++}>
                {results.map(x => getResultText(x)).join(", ")}
            </p>
        )
        resultDoms.push(resultDom)
    }

    return <>{resultDoms}</>
}

function getResultText(result: Result): string | null {
    if (result.type == "value") {
        return getValueText(result.value, true)
    }
    if (result.type == "error") {
        return result.message
    }
    if (result.type == "conversion") {
        return `converted ${getValueText(
            result.previous,
            false
        )} to ${getValueText(result.new, false)}`
    }
    return null
}

function getRange(range: string): Position {
    const split = range.split("-").map((x) => parseInt(x))
    return split as Position
}

function getValueText(value: Value, includeEqualSign: boolean): string | null {
    let equalSign = " = "
    let leadsToSign = " → "

    if (includeEqualSign == false) {
        equalSign = ""
        leadsToSign = ""
    }

    if (value.type == "real") {
        return equalSign + value.value
    }
    if (value.type == "boolean") {
        return leadsToSign + (value.value ? "true" : "false")
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
            return "undefined"
        }
        return null
    }
    if (value.type == "matrix") {
        return getMatrixText(value)
    }

    return null
}

function getMatrixText(matrix: MatrixValue): string {
    const dimensions = getMatrixDimensionCount(matrix)

    const [start, end] = dimensions == 1 ? "()" : "[]"
    return getMatrixDimensionText(matrix.matrix, start, end)
}
function getMatrixDimensionText(
    matrixDimension: MatrixDimension | Value,
    start: string,
    end: string
): string {
    if (Array.isArray(matrixDimension)) {
        return `${start}${matrixDimension
            .map((x) => getMatrixDimensionText(x, start, end))
            .join(", ")}${end}`
    } else {
        return getValueText(matrixDimension, false)
    }
}
function getMatrixDimensionCount(matrix: MatrixValue): number {
    let dimensions = 0
    let currentDimension: Value | MatrixDimension = matrix.matrix
    while (Array.isArray(currentDimension)) {
        dimensions++
        currentDimension = currentDimension[0]
    }
    return dimensions
}
