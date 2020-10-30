const path = require('path');
//not sure if this is needed bc files arent .vue but have it if any are added
const VueLoaderPlugin = require("vue-loader/lib/plugin");

module.exports = {
    mode: "development", // "production" | "development" | "none"
    // Chosen mode tells webpack to use its built-in optimizations accordingly.
    entry: ["./wwwroot/assets/js/catfish.editFieldForm.js", "./wwwroot/assets/js/catfish.edititem.js"], // string | object | array
    // defaults to ./src
    // Here the application starts executing
    // and webpack starts bundling
    output: {
        // options related to how webpack emits results
        path: path.resolve(__dirname, "wwwroot/assets/dist"), // string
        // the target directory for all output files
        // must be an absolute path (use the Node.js path module)
        filename: "bundle.js", // string
        // the filename template for entry chunks
    },

    //because piranha is using their own version of vue, we need to alias it here
    //note that this... doesnt work :(
    resolve: {
        alias: {
            // If using the runtime only build
            vue$: 'vue/dist/vue.runtime.esm.js'
            // Or if using full build of Vue (runtime + compiler)
            // vue$: 'vue/dist/vue.esm.js'
        }
    },

    module: {
        // configuration regarding modules
        rules: [
            // rules for modules (configure loaders, parser options, etc.)
            {
                test: /\.vue$/,
                exclude: /node_modules/,
                use: {
                    loader: "vue-loader",
                    options: {
                        js: "babel-loader",
                        hotReload: true
                    }
                }
            },
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: [{
                    loader: "babel-loader",
                    options: {
                        presets: ["@babel/preset-env"]
                    }
                }]
            },
            {
                test: /\.(css|scss)$/,
                exclude: /node_modules/, //this line was commented out before
                use: ["vue-style-loader", "style-loader", "css-loader", "sass-loader"]
            }
        ]
    },
    plugins: [new VueLoaderPlugin()],
    optimization: {
        splitChunks: {
            cacheGroups: {
                commons: {
                    test: /[\\/]node_modules[\\/]/,
                    name: "vendors",
                    chunks: "all"
                }
            }
        }
    },
    devtool: "source-map"
}; 