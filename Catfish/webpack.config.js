const path = require('path');

module.exports = {
    entry: {
        main: './Scripts/es6/main.js',
        file2: './Scripts/es6/file2.js'
    },
    output: {
        path: path.resolve(__dirname, './Scripts/dist'),
        filename: "[name].js"
    },
    module: {
        rules: [{
            loader: 'babel-loader',
            test: /\.js$/,
            exclude: /node_modules/
        }]
    }
};