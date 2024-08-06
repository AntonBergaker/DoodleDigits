import React, { useEffect, useRef, useState } from "react"
import "./title-bar.scss"
import { HamburgerIcon } from "./img/hamburger-icon"
import { TitleBarDropdown } from "./title-bar-dropdown"
import { CalculatorSettings } from "../saving/saving"
import { ThemeData } from "../saving/themes"

type TitleBarProps = {
    settings: CalculatorSettings
    availableThemes: ThemeData[]
}

export function TitleBar(props: TitleBarProps) {
    const [focus, setFocus] = useState(true)
    const [dropdownVisible, setDropdownVisible] = useState(false)
    const ref = useRef<HTMLDivElement>()

    useEffect(() => {
        function onMousedown(event: MouseEvent) {
            if (ref.current && !ref.current.contains(event.target as any)) {
                setDropdownVisible(false)
            }
        }

        const focusUnsub = window.electronApi.onFocusedChanged(
            (sender, data) => {
                setFocus(data.focused)
            }
        )

        document.addEventListener("mousedown", onMousedown)
        return () => {
            document.removeEventListener("mousedown", onMousedown)
            focusUnsub()
        }
    }, [ref])

    function showMenu() {
        setDropdownVisible(!dropdownVisible)
    }

    return (
        <div ref={ref} className={`titleBar ${focus || "unfocused"}`}>
            <div className="dropdownButton">
                <HamburgerIcon onClick={showMenu} />
            </div>
            <TitleBarDropdown
                availableThemes={props.availableThemes}
                faceLeft={false}
                show={dropdownVisible}
                settings={props.settings}
            />
            <svg className="seperator draggable" width="2px">
                <rect fill="currentColor" width="1.4px" height="25px" />
            </svg>
            <h1 className="draggable">doodle digits</h1>
            <div className="padding draggable" />
        </div>
    )
}
