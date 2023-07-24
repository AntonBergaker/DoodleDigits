import { SaveStateData } from "./saving"

export class StateSavingScheduler {
    private state: SaveStateData
    private lastStateSave = Date.now()
    private savePromise: Promise<void> | null = null

    constructor(state: SaveStateData) {
        this.state = state
    }

    public scheduleSave() {
        // Saving already scheduled
        if (this.savePromise != null) {
            return
        }

        const timeLeft = this.lastStateSave + 1000 * 5 - Date.now()

        // Time is less than 0, save instantly
        if (timeLeft < 0) {
            this.transferSave()
        }
        // Time is larger than 0, wait till 5 seconds has passed since last save before saving again
        else {
            this.savePromise = (async () => {
                await new Promise((r) => setTimeout(r, timeLeft))
                this.savePromise = null
                this.transferSave()
            })()
        }
    }

    private transferSave() {
        window.electronApi.saveState(this.state)
        this.lastStateSave = Date.now()
    }
}
