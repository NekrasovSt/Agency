"use strict";

const isDevBuild = process.argv.indexOf('--env.prod') < 0;
const devConfig = require("./build/webpack.dev.conf");
const prodConfig = require("./build/webpack.prod.conf");

module.exports = isDevBuild ? devConfig : prodConfig;
