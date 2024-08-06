import React, { useEffect, useRef, useState } from "react"
import { CalculatorSettings } from "../saving/saving"
import { TitleBarDropdown } from "./title-bar-dropdown"
import { HamburgerIcon } from "./img/hamburger-icon"
import "./title-bar-button.scss"
import { ThemeData } from "../saving/themes"

type TitleBarButtonProps = {
    settings: CalculatorSettings
    availableThemes: ThemeData[]
}

export function TitleBarButton(props: TitleBarButtonProps) {
    const [dropdownVisible, setDropdownVisible] = useState(false)
    const ref = useRef<HTMLDivElement>()

    useEffect(() => {
        function onMousedown(event: MouseEvent) {
            if (ref.current && !ref.current.contains(event.target as any)) {
                setDropdownVisible(false)
            }
        }

        document.addEventListener("mousedown", onMousedown)
        return () => {
            document.removeEventListener("mousedown", onMousedown)
        }
    }, [ref])

    function showMenu() {
        setDropdownVisible(!dropdownVisible)
    }

    return (
        <div ref={ref} className="titleBarButton" onClick={showMenu}>
            <HamburgerIcon />
            <TitleBarDropdown
                availableThemes={props.availableThemes}
                faceLeft={true}
                show={dropdownVisible}
                settings={props.settings}
            />
        </div>
    )
}
