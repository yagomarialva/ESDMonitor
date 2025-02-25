const path = require('path');

module.exports = {
  entry: './src/index.js', // Seu ponto de entrada do React
  output: {
    path: path.resolve(__dirname, 'dist'),
    filename: 'bundle.js',
    publicPath: '/', // Necessário para o HMR e roteamento no React
  },
  module: {
    rules: [
      {
        test: /\.js$/,
        exclude: /node_modules/,
        use: {
          loader: 'babel-loader',
          options: {
            presets: ['@babel/preset-env', '@babel/preset-react'],
          },
        },
      },
      {
        test: /\.css$/, // Adiciona suporte para CSS
        use: ['style-loader', 'css-loader'],
      },
      {
        test: /\.(png|jpg|gif|svg|ico)$/, // Suporte para imagens e ícones
        type: 'asset/resource',
      },
    ],
  },
  externals: {
    react: 'React',
    'react-dom': 'ReactDOM',
  },
  resolve: {
    extensions: ['.js', '.jsx'],
  },
  mode: 'development', // Troque para 'production' em produção
  devServer: {
    static: {
      directory: path.join(__dirname, 'dist'),
    },
    historyApiFallback: true, // Necessário para trabalhar com React Router
    compress: true, // Habilita compressão gzip
    port: 3000, // Porta do servidor de desenvolvimento
    hot: true, // Habilita Hot Module Replacement (HMR)
    proxy: {
      '/api': {
        target: 'http://192.168.0.102:5051', // Primeira opção de servidor
        changeOrigin: true,
        secure: false,
        timeout: 10000, // Ajusta o tempo limite do proxy
        pathRewrite: { '^/api': '' }, // Remove '/api' do caminho de URL
      },
      // '/api': {
      //   target: 'http://192.168.0.103:7080', // Primeira opção de servidor
      //   changeOrigin: true,
      //   secure: false,
      //   timeout: 10000, // Ajusta o tempo limite do proxy
      //   pathRewrite: { '^/api': '' }, // Remove '/api' do caminho de URL
      // },
      // '/api': {
      //   target: 'http://localhost:5051', // Segunda opção de servidor
      //   changeOrigin: true,
      //   secure: false,
      //   timeout: 10000, // Ajusta o tempo limite do proxy
      //   pathRewrite: { '^/api': '' }, // Remove '/api' do caminho de URL
      // },
      // '/api': {
      //   target: 'http://localhost:7080', // Segunda opção de servidor
      //   changeOrigin: true,
      //   secure: false,
      //   timeout: 10000, // Ajusta o tempo limite do proxy
      //   pathRewrite: { '^/api': '' }, // Remove '/api' do caminho de URL
      // },
    },
    client: {
      overlay: {
        warnings: true, // Mostra avisos no navegador
        errors: true,   // Mostra erros no navegador
      },
    },
  },
};
