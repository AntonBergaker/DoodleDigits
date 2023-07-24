export type Position = [x: number, y: number]

type LineLength = {
    start: number
    end: number
}
export class PageMeasurer {
    private textContainerRef: React.MutableRefObject<HTMLDivElement>

    private lastRef: HTMLDivElement | undefined = undefined
    private lastInput = ""

    private lines: LineLength[] = []
    private textArea: HTMLTextAreaElement
    private textAreaMagic: HTMLDivElement

    constructor(textContainerRef: React.MutableRefObject<HTMLDivElement>) {
        this.textContainerRef = textContainerRef
    }

    private onRefChanged(newRef: HTMLDivElement) {
        this.textArea = newRef.getElementsByClassName(
            "input"
        )[0] as HTMLTextAreaElement
        this.textAreaMagic = newRef.getElementsByClassName(
            "input-magic"
        )[0] as HTMLDivElement

        this.lastRef = newRef
    }

    private onInputChanged(newValue: string) {
        this.lines.length = 0

        let start = 0
        while (start < newValue.length) {
            const end = newValue.indexOf("\n", start + 1)
            if (end == -1) {
                this.lines.push({ start, end: newValue.length })
                break
            }
            this.lines.push({ start, end })
            start = end
        }

        this.lastInput = newValue
    }

    private checkRefs() {
        if (this.lastRef != this.textContainerRef.current) {
            this.onRefChanged(this.textContainerRef.current)
        }
        if (this.lastInput != this.textArea.value) {
            this.onInputChanged(this.textArea.value ?? "")
        }
    }

    public getCharacterLine(character: number): number {
        this.checkRefs()

        for (let i = 0; i < this.lines.length; i++) {
            const line = this.lines[i]
            if (character >= line.start && character <= line.end) {
                return i
            }
        }
        return this.lines.length - 1
    }

    public getLinePosition(line: number): Position {
        this.checkRefs()

        const len = this.textAreaMagic.children.length

        if (len == 0) {
            return [0, 0]
        }

        const child = this.textAreaMagic.children[
            clamp(line * 2, 0, len - 1)
        ] as HTMLElement

        const pos: Position = [
            child.offsetLeft + child.offsetWidth,
            child.offsetTop,
        ]
        return pos
    }
}

function clamp(num: number, min: number, max: number): number {
    return num <= min ? min : num >= max ? max : num
}
