import React from "react"
import "./title-bar-dropdown.scss"
import { CalculatorSettings } from "../saving/saving"
import { capitalize } from "../utils"
import { ThemeData } from "../saving/themes"

type TitleBarDropdownProps = {
    show: boolean
    faceLeft: boolean
    availableThemes: ThemeData[]
    settings: CalculatorSettings
}

export function TitleBarDropdown(props: TitleBarDropdownProps) {
    function clickAngleToggle() {
        const unit =
            props.settings.angle_unit == "degrees" ? "radians" : "degrees"
        window.electronApi.updateAngleUnit(unit)
    }

    function clickTheme() {
        console.log(props.availableThemes)
        let currentIndex = props.availableThemes.findIndex(
            (x) => x.id == props.settings.theme
        )
        currentIndex = (currentIndex + 1) % props.availableThemes.length

        const theme = props.availableThemes[currentIndex].id
        window.electronApi.updateTheme(theme)
    }

    // Select theme name, or fallback to id if it's not found yet
    const themeStr =
        props.availableThemes.find((x) => x.id == props.settings.theme)?.name ??
        capitalize(props.settings.theme)

    return (
        <div
            style={{ visibility: props.show ? "visible" : "hidden" }}
            className={`titleBarDropdown ${props.faceLeft && "faceLeft"}`}>
            <div onClick={clickAngleToggle} className="labeledToggle">
                <label>Angle unit</label>
                <span>{capitalize(props.settings.angle_unit)}</span>
            </div>
            <div onClick={clickTheme} className="labeledToggle">
                <label>Theme</label>
                <span>{themeStr}</span>
            </div>
        </div>
    )
}
