import type { Configuration } from "webpack"

import { rules } from "./webpack.rules"
import { plugins } from "./webpack.plugins"

rules.push({
    test: /\.(png|jp(e*)g|svg|gif)$/,
    type: "asset/resource",
})
rules.push({
    test: /\.(scss|css)$/,
    use: ["style-loader", "css-loader", "sass-loader"],
})

export const rendererConfig: Configuration = {
    module: {
        rules,
    },
    plugins,
    resolve: {
        extensions: [".js", ".ts", ".jsx", ".tsx", ".css"],
    },
}
