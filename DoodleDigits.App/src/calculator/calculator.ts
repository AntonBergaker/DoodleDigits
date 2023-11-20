import { CalculatorAngleUnit, CalculatorSettings } from "../saving/saving"
import { CalculatorResult } from "./calculator-result"

let dotnetExports: any = undefined

export type WorkerMessageInput = {
    input: string
    index: number
}
export type WorkerMessageOutput = {
    result: CalculatorResult
    index: number
}

export type CalculatorInputSettings = {
    angleUnit: CalculatorAngleUnit
}

export async function calculate(
    input: string,
    settings: CalculatorSettings
): Promise<CalculatorResult> {
    if (dotnetExports == undefined) {
        const anyWindow = window as any
        // If the script hasn't loaded yet, give it time
        let tries = 0
        while (!anyWindow.dotnetGlobalReference) {
            if (tries > 100) {
                throw Error("Failed to find dotnet inside the window class.")
            }
            await new Promise((r) => setTimeout(r, 100))
            tries++
        }

        const dotnet = anyWindow.dotnetGlobalReference
        const { getAssemblyExports, getConfig } = await dotnet
            .withDiagnosticTracing(false)
            .withApplicationArgumentsFromQuery()
            .create()

        const config = getConfig()
        dotnetExports = await getAssemblyExports(config.mainAssemblyName)
    }

    const inputSettings: CalculatorInputSettings = {
        angleUnit: settings.angle_unit,
    }

    console.log(inputSettings)

    const result = JSON.parse(
        dotnetExports.DoodleDigits.JsInterop.CalculatorInterop.Calculate(
            input,
            JSON.stringify(inputSettings)
        )
    ) as CalculatorResult
    return result
}
/*
onmessage = async (e) => {
    const { input, index } = e.data as WorkerMessageInput;
    const result = await calculate(input);
    postMessage( { result, index } );
}*/
