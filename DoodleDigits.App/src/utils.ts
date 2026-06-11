export function capitalize(input: string): string {
    return input.charAt(0).toUpperCase() + input.slice(1)
}

export function stringToBase64(input: string): string {
    const byteString = btoa(String.fromCharCode(...new TextEncoder().encode(input)))
    return byteString
        .replace(/\+/g, '-')
        .replace(/\//g, '_')
        .replace(/=+$/, '')
}

export function jsonToBase64(input: any): string {
    return stringToBase64(JSON.stringify(input))
}

export function stringFromBase64(base64url: string): string {
    const base64 = base64url
        .replace(/-/g, '+')
        .replace(/_/g, '/')
        .padEnd(Math.ceil(base64url.length / 4) * 4, '=')

    return new TextDecoder().decode(
        Uint8Array.from(atob(base64), c => c.charCodeAt(0))
    )
}

export function jsonFromBase64(base64: string): any {
    return JSON.parse(stringFromBase64(base64))
}