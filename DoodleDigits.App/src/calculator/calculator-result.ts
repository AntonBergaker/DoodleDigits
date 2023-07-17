export type CalculatorResult = {
    results: Result[]
}

export type Result = ValueResult | ErrorResult | ConversionResult
type BaseResult = {
    range: string
}

export type ValueResult = BaseResult&{
    type: "value"
    value: Value
}

export type ErrorResult = BaseResult&{
    type: "error"
    message: string
}

export type ConversionResult = BaseResult&{
    type: "conversion"
    previous: Value
    new: Value
}

export type Value = MatrixValue | RealValue | BooleanValue | TooBigValue | UndefinedValue
type BaseValue = {
    trivially_achieved: boolean
}

export type MatrixValue = BaseValue&{
    type: "matrix"
    matrix: MatrixDimension
}
export type MatrixDimension = MatrixDimension[] | Value[]

export type RealValue = BaseValue&{
    type: "real"
    value: string
}

export type BooleanValue = BaseValue&{
    type: "boolean"
    value: boolean
}

export type TooBigValue = BaseValue&{
    type: "too_big"
    value: "positive" | "positive_infinity" | "negative" | "negative_infinity"
}

export type UndefinedValue = BaseValue&{
    type: "undefined",
    undefined_type: "undefined" | "unset" | "error"
}


