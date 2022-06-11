var path = require('path')
var webpack = require('webpack')

module.exports = {
  entry: './src/main.js',
  output: {
    path: path.resolve(__dirname, './dist'),
    publicPath: '/dist/',
    filename: 'build.js'
  },
  module: {
    rules: [
      {
        test: /\.css$/,
        use: [
          'vue-style-loader',
          'css-loader'
        ],
      },      {
        test: /\.vue$/,
        loader: 'vue-loader',
        options: {
          loaders: {
          }
          // other vue-loader options go here
        }
      },
      {
        test: /\.js$/,
        loader: 'babel-loader',
        exclude: /node_modules/
      },
      {
        test: /\.(png|jpg|gif|svg)$/,
        loader: 'file-loader',
        options: {
          name: '[name].[ext]?[hash]'
        }
      }
    ]
  },
  resolve: {
    alias: {
      'vue$': 'vue/dist/vue.esm.js'
    },
    extensions: ['*', '.js', '.vue', '.json']
  },
  devServer: {
    historyApiFallback: true,
    noInfo: true,
    overlay: true,
    proxy: {
      '/api/payments' : {
        //target: 'http://localhost:8081',
        target: 'http://payment-service:8080',
        changeOrigin: true,
        secure: false
      },
      '/api/customers' : {
        //target: 'http://localhost:8082',
        target: 'http://customer-service:8080',
        changeOrigin: true,
        secure: false
      },
      '/api/offers' : {
        //target: 'http://localhost:8083',
        target: 'http://offer-service:80',
        changeOrigin: true,
        secure: false
      },
      '/api/orders' : {
        //target: 'http://localhost:8084',
        target: 'http://order-service:80',
        changeOrigin: true,
        secure: false
      },
      '/offerHub' : {
        //target: 'http://localhost:8083',
        target: 'http://offer-service:80',
        changeOrigin: true,
        secure: false
      },
      '/offerHub' : {
        //target: 'ws://localhost:8083',
        target: 'ws://offer-service:80',
        changeOrigin: true,
        ws: true
      }
    },
    headers: {
      "Access-Control-Allow-Origin": "*",
    },
    port: 8080
  },
  performance: {
    hints: false
  },
  devtool: '#eval-source-map'
}

if (process.env.NODE_ENV === 'production') {
  const UglifyJsPlugin = require('uglifyjs-webpack-plugin')

  module.exports.devtool = '#source-map'
  // http://vue-loader.vuejs.org/en/workflow/production.html
  module.exports.plugins = (module.exports.plugins || []).concat([
    new webpack.DefinePlugin({
      'process.env': {
        NODE_ENV: '"production"'
      }
    }),
    new UglifyJsPlugin({
      "uglifyOptions":
          {
              compress: {
                  warnings: false
              },
              sourceMap: true
          }
    }),
    new webpack.LoaderOptionsPlugin({
      minimize: true
    })
  ])
}
