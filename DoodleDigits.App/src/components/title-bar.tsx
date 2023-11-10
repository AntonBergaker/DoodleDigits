import React, { useState } from "react";
import "./title-bar.css"

export function TitleBar() {
    const [focus, setFocus] = useState(true);

    window.electronApi.onFocusedChanged((sender, data) => {
        setFocus(data.focused);
    });

    return <div className={`titleBar ${focus || "unfocused"}`}>
        <p>doodle digits</p>
    </div>
}