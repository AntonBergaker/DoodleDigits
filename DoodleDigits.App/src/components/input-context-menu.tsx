import React from "react"

export function InputContextMenu() {
    return (
        <div>
            <ContextMenuDivider></ContextMenuDivider>
        </div>
    )
}

type ContextMenuItemProps = {}

function ContextMenuItem(props: ContextMenuItemProps) {
    return (
        <div>
            <h2>Text</h2>
        </div>
    )
}

function ContextMenuDivider() {
    return <div></div>
}
