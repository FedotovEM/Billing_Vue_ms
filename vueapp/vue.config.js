const fs = require('fs')
const path = require('path')

const baseFolder =
    process.env.APPDATA !== undefined && process.env.APPDATA !== ''
        ? `${process.env.APPDATA}/ASP.NET/http`
        : `${process.env.HOME}/.aspnet/http`;

const certificateArg = process.argv.map(arg => arg.match(/--name=(?<value>.+)/i)).filter(Boolean)[0];
const certificateName = certificateArg ? certificateArg.groups.value : "vueapp";

if (!certificateName) {
    console.error('Invalid certificate name. Run this script in the context of an npm/yarn script or pass --name=<<app>> explicitly.')
    process.exit(-1);
}

const certFilePath = path.join(baseFolder, `${certificateName}.pem`);
const keyFilePath = path.join(baseFolder, `${certificateName}.key`);

const { defineConfig } = require('@vue/cli-service')
const webpack = require('webpack');

module.exports = {
    configureWebpack: {
        plugins: [
            new webpack.DefinePlugin({
                __VUE_PROD_HYDRATION_MISMATCH_DETAILS__: 'false',
            })
        ],
    },
    devServer: {
        proxy: {
            '^/weatherforecast': {
                target: 'http://localhost:7066/'
            }
        },
        port: 443,

        host: '0.0.0.0',
        port: 3000,
        client: {
            webSocketURL: 'ws://0.0.0.0:3000/ws',
        },
        headers: {
            'Access-Control-Allow-Origin': '*',
        }
    }
}