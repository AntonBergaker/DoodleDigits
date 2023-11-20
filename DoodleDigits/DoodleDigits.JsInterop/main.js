import { dotnet } from './_framework/dotnet.js'

// Expose dotnet to the rest of the app. It's a kinda ugly hack to add it to the window, but trying to reference
// it directly using bootstrap is leading to a tons of issues, so in the end this is the approach chosen.
window.dotnetGlobalReference = dotnet