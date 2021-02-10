const path = require("path");
const glob = require('glob');
//const MiniCssExtractPlugin = require('mini-css-extract-plugin');

module.exports = {
    entry: {
        app: glob.sync('./Scripts/**/*.js*'),
        styles: './Styles/styles.scss'
    },
    output: {
        path: path.join(__dirname, '/wwwroot/js'),
        filename: '[name].bundle.js',
        sourceMapFilename: '[name].map'
    },
    module: {
        rules: [
            {
                test: /\.(js|jsx)$/,
                exclude: /node_modules/,
                use: {
                    loader: "babel-loader"
                }
            },
            {
                test: /\.(scss|css)$/,
                use: ['style-loader', 'css-loader?url=false', 'sass-loader']
                //use: [MiniCssExtractPlugin.loader,'style-loader', 'css-loader?url=false', 'sass-loader']
            }
        ]
    },
    //plugins: [
    //    new MiniCssExtractPlugin()
    //]
};