import type { Configuration } from "webpack"

import { rules } from "./webpack.rules"
import CopyPlugin from "copy-webpack-plugin"
import ForkTsCheckerWebpackPlugin from "fork-ts-checker-webpack-plugin"
import { DefinePlugin } from "webpack"

rules.push({
    test: /\.css$/,
    use: [{ loader: "style-loader" }, { loader: "css-loader" }],
})

const webConfig: Configuration = {
    /**
     * This is the main entry point for your application, it's the first file
     * that runs in the main process.
     */
    node: {
        __dirname: true,
    },

    devtool: "inline-source-map",
    entry: "./src/root/app.tsx",
    plugins: [
        new ForkTsCheckerWebpackPlugin({
            logger: "webpack-infrastructure",
        }),
        new CopyPlugin({
            patterns: [{ from: "./static" }],
        }),
        new DefinePlugin({
            WEB: true,
        }),
    ],
    // Put your normal webpack config below here
    module: {
        rules,
    },
    resolve: {
        extensions: [".js", ".ts", ".jsx", ".tsx", ".css", ".json"],
    },
}

export default webConfig
