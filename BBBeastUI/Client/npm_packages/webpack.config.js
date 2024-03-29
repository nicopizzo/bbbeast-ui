﻿const path = require("path");

module.exports = {
    entry: './src/index.js',
    output: {
        path: path.resolve(__dirname, '../wwwroot/js'),
        filename: 'index.bundle.js',
        libraryTarget: 'var',
        library: 'Jazzicon'
    }
};