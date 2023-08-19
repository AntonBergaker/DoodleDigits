export function mockElectronApi() {
    window.electronApi = {
        onSizeChanged: () => {},
        onUpdateSettings: () => {},
        saveState: () => {},
    }
}
