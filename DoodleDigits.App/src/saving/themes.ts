export type ThemeData = {
    id: string
    name: string
    styleSheet: string | null
    isDark: boolean
}

export function themeFileToThemeData(id: string, fileObj: any): ThemeData {
    return {
        id: id,
        name: fileObj.name,
        styleSheet: id + "/" + fileObj.style_sheet,
        isDark: fileObj.is_dark,
    }
}

export function getDefaultTheme(): ThemeData {
    return {
        id: "default",
        name: "Default",
        styleSheet: null,
        isDark: false,
    }
}
export function getDarkTheme(): ThemeData {
    return {
        id: "dark",
        isDark: true,
        name: "Dark",
        styleSheet: "dark/dark.css",
    }
}
