import React from "react"
import "./title-bar-dropdown.scss"
import { CalculatorAngleUnit } from "../saving/saving"
import { capitalize } from "../utils"

type TitleBarDropdownProps = {
    show: boolean
    angleUnit: CalculatorAngleUnit
}

export function TitleBarDropdown(props: TitleBarDropdownProps) {
    function clickAngleToggle() {
        const unit = props.angleUnit == "degrees" ? "radians" : "degrees"
        window.electronApi.updateAngleUnit(unit)
    }

    return (
        <div
            style={{ visibility: props.show ? "visible" : "hidden" }}
            className="titleBarDropdown">
            <div onClick={clickAngleToggle} className="labeledToggle">
                <label>Angle unit</label>
                <span>{capitalize(props.angleUnit)}</span>
            </div>
        </div>
    )
}
