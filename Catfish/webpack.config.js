const path = require('path');

module.exports = {
    mode: 'development',
    entry: {
        associationsView: './Scripts/es6/pages/associationsView.js'
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