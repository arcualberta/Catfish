/*
 Config file for bundling Vue with frontend and other libraries if applicable.
 First run 'npm install', then run Webpack bundling using 'npm run build' at root directory of Catfish.
*/

const path = require('path');
var webpack = require('webpack');
//not sure if this is needed bc files arent .vue but have it if any are added
const VueLoaderPlugin = require("vue-loader/lib/plugin");

/*
 Include any javascript files that require the use of Vue and/or libraries below.
 They are separated by manager-side and public-facing side.
 This is important! Manager-side already includes a copy of Vue, which Piranha provides.
 The manager-side will not have Vue added to its pages or Vue will be duplicated
 (note manager side is any page you need to log into to reach).

 Any public-facing js should be put into the assets/js/_public-facing folder.

The key's name will be what the bundle will be called when created in assets/dist.
Put a script tag into the page's cshtml for it to run there.
Do not put a specific page's script into the _Layout or you will get multiple Vue instances running!
Bad stuff happens when there's duplicate Vue instances, don't do it.
 */

const managerSideEntryPaths = {
    editFieldFormBundle: "./wwwroot/assets/js/catfish.editFieldForm.js",
    editItemBundle: "./wwwroot/assets/js/catfish.edititem.js",
};

const publicFacingEntryPaths = {
    //keywordsSearchBundle: "./wwwroot/assets/js/_public-facing/keywords-search-block-public.js"
    'calendar-block-vue': "./wwwroot/assets/js/_public-facing/calendar-block.js"
};

///////////////////////////////////////////////////////////////////////////////////

/*
    Next is common stuff between the manager-side and public-facing side to avoid code duplication.
    Please note that I left the comments from the example Webpack in the first config object for referencing,
    I found it useful to look at them when problems arose.
    For more info about different attributes in Webpack see the official documentation.

    The main differences between the two configs are:
    -In attribute 'entry', entry objects (the constants created above)
    -In attribute 'plugins', the ProvidePlugin for Vue with every js file on the public-facing side
    -In 'optimization', the name of the vendors chunk. 

    TODO: set up production builds for these
*/

let devMode = "development";
let productionMode = "productionMode";

let outputPath = path.resolve(__dirname, "wwwroot/assets/dist");
let filenamePath = '[name].js';

let moduleRules = [
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
];

let devTool = "source-map";

///////////////////////////////////////////////////////////////////////////////////

/*
 Below are two config objects, one for manager-side, one for public-facing side.
*/

module.exports = [
    //manager-side config
    {
        mode: devMode, // "production" | "development" | "none"
        // Chosen mode tells webpack to use its built-in optimizations accordingly.
        entry: managerSideEntryPaths,
        // defaults to ./src
        // Here the application starts executing
        // and webpack starts bundling
        output: {
            // options related to how webpack emits results
            path: outputPath, // string
            // the target directory for all output files
            // must be an absolute path (use the Node.js path module)
            filename: filenamePath, //"bundle.js", // string
            // the filename template for entry chunks
        },

        module: {
            // configuration regarding modules
            rules: moduleRules
        },
        plugins: [new VueLoaderPlugin()],
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
        devtool: devTool
    },
    //public-facing entry config
    {
        mode: devMode,
        entry: publicFacingEntryPaths,
        output: {
            path: outputPath, 
            filename: filenamePath
        },

        module: {
            rules: moduleRules
        },
        //plugins: [new VueLoaderPlugin(),
        //    new webpack.ProvidePlugin({
        //        Vue: ['vue/dist/vue.esm.js', 'default']
        //    })
        //],
        //optimization: {
        //    splitChunks: {
        //        cacheGroups: {
        //            commons: {
        //                test: /[\\/]node_modules[\\/]/,
        //                name: "vendorsPublicFacingSide",
        //                chunks: "all"
        //            }
        //        }
        //    }
        //},
        devtool: devTool
    }
]; 