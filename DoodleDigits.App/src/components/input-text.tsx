import React, { useState } from "react"
import "./input-text.css"

export type InputTextProps = {
    onInput: (input: string) => void
    defaultText: string
}

export const InputText = React.forwardRef<HTMLDivElement, InputTextProps>(
    (props, ref) => {
        const [text, setText] = useState(props.defaultText ?? "")

        async function onInput(target: HTMLTextAreaElement) {
            setText(target.value)
            props.onInput(target.value)
        }

        return (
            <div ref={ref} className="input-container">
                <InputMagic text={text} />
                <textarea
                    onInput={(a) =>
                        a.target instanceof HTMLTextAreaElement &&
                        onInput(a.target)
                    }
                    className="input"
                    autoCapitalize="off"
                    spellCheck="false"
                    defaultValue={props.defaultText}></textarea>
            </div>
        )
    }
)

type InputMagicProps = {
    text: string
}
export const InputMagic = (props: InputMagicProps) => {
    let index = 0
    const array = props.text.split("\n")
    const chars = []
    for (const char of array) {
        chars.push(<span key={index}>{char}</span>)
        chars.push(<br key={`nl-${index}`}></br>)
        index++
    }
    return <div className="input-magic">{chars}</div>
}
