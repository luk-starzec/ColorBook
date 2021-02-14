const path = require("path");
const glob = require('glob');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const CssMinimizerPlugin = require('css-minimizer-webpack-plugin');

module.exports = {
    entry: {
        'js/app': glob.sync('./Scripts/**/*.js*'),
        'css/styles': glob.sync('./Styles/**/*.*css')
    },
    output: {
        path: path.join(__dirname, '/wwwroot'),
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
                //use: [MiniCssExtractPlugin.loader, 'css-loader?url=false', 'sass-loader']
            }
        ]
    },
    plugins: [
        new MiniCssExtractPlugin()
    ],
    optimization: {
        minimizer: [
            new CssMinimizerPlugin(),
        ],
    },
};