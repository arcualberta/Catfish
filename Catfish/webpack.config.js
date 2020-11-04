const path = require('path');
var webpack = require('webpack');
//not sure if this is needed bc files arent .vue but have it if any are added
const VueLoaderPlugin = require("vue-loader/lib/plugin");
const publicFacingDirectory = "_public-facing";

/*
 Include any javascript files that require the use of Vue and/or libraries here.
 They are separated by manager-side and public-facing side.
 This is important! Manager-side already includes a copy of Vue, which Piranha provides.
 The manager-side will not have Vue added to its pages or it will be duplicated
 (note manager side is any page you need to log into to reach).

 Any public-facing js MUST be put into the assets/js/_public-facing folder.
 There is a check below to include Vue with anything in that folder.

The name on the left will be what the bundle will be called when created in assets/dist
 */

const managerSideEntryPaths = {
    editFieldFormBundle: "./wwwroot/assets/js/catfish.editFieldForm.js",
    editItemBundle: "./wwwroot/assets/js/catfish.edititem.js",
};

const publicFacingEntryPaths = {
    keywordsSearchBlock: "./wwwroot/assets/js/_public-facing/test.js",
    testBlock: "./wwwroot/assets/js/_public-facing/test2.js"
};

module.exports = [
    {
    mode: "development", // "production" | "development" | "none"
    // Chosen mode tells webpack to use its built-in optimizations accordingly.
    entry: managerSideEntryPaths,
        // defaults to ./src
    // Here the application starts executing
    // and webpack starts bundling
    output: {
        // options related to how webpack emits results
        path: path.resolve(__dirname, "wwwroot/assets/dist"), // string
        // the target directory for all output files
        // must be an absolute path (use the Node.js path module)
        filename: '[name].js', //"bundle.js", // string
        // the filename template for entry chunks
    },

    //because piranha is using their own version of vue, we need to alias it here
    //note that this... doesnt work :(
    /*resolve: {
        alias: {
            // If using the runtime only build
            vue$: 'vue/dist/vue.runtime.esm.js'
            // Or if using full build of Vue (runtime + compiler)
            // vue$: 'vue/dist/vue.esm.js'
        }
    },*/

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
    plugins: [new VueLoaderPlugin(),
        
    ],
    optimization: {
        splitChunks: {
            cacheGroups: {
                commons: {
                    test: /[\\/]node_modules[\\/]/,
                    name: "vendorsManagerSide",
                    chunks: "all"
                }
            }
        }
    },
    devtool: "source-map"
    },

    {
        mode: "development", // "production" | "development" | "none"
        // Chosen mode tells webpack to use its built-in optimizations accordingly.
        entry: publicFacingEntryPaths,
        // defaults to ./src
        // Here the application starts executing
        // and webpack starts bundling
        output: {
            // options related to how webpack emits results
            path: path.resolve(__dirname, "wwwroot/assets/dist"), // string
            // the target directory for all output files
            // must be an absolute path (use the Node.js path module)
            filename: '[name].js', //"bundle.js", // string
            // the filename template for entry chunks
        },

        //because piranha is using their own version of vue, we need to alias it here
        //note that this... doesnt work :(
        /*resolve: {
            alias: {
                // If using the runtime only build
                vue$: 'vue/dist/vue.runtime.esm.js'
                // Or if using full build of Vue (runtime + compiler)
                // vue$: 'vue/dist/vue.esm.js'
            }
        },*/

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
        plugins: [new VueLoaderPlugin(),
            new webpack.ProvidePlugin({
                Vue: ['vue/dist/vue.esm.js', 'default']
            })
        ],
        optimization: {
            splitChunks: {
                cacheGroups: {
                    commons: {
                        test: /[\\/]node_modules[\\/]/,
                        name: "vendorsPublicFacingSide",
                        chunks: "all"
                    }
                }
            }
        },
        devtool: "source-map"
    }
]; 