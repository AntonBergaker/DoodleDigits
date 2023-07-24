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

export async function calculate(input: string): Promise<CalculatorResult> {
    if (dotnetExports == undefined) {
        const anyWindow = window as any
        // If the script hasn't loaded yet, give it time
        let tries = 0
        while (!anyWindow.dotnetLmao) {
            if (tries > 20) {
                throw Error("Failed to find dotnet inside the window class.")
            }
            await new Promise((r) => setTimeout(r, 100))
            tries++
        }

        const dotnet = (window as any).dotnetLmao
        const { getAssemblyExports, getConfig } = await dotnet
            .withDiagnosticTracing(false)
            .withApplicationArgumentsFromQuery()
            .create()

        const config = getConfig()
        dotnetExports = await getAssemblyExports(config.mainAssemblyName)
    }

    const result = JSON.parse(
        dotnetExports.DoodleDigitsJsInterop.CalculatorInterop.Calculate(input)
    ) as CalculatorResult
    return result
}
/*
onmessage = async (e) => {
    const { input, index } = e.data as WorkerMessageInput;
    const result = await calculate(input);
    postMessage( { result, index } );
}*/
