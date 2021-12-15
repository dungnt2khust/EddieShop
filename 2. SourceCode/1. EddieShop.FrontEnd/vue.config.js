module.exports = {
  css: {
    loaderOptions: {
      sass: {
        data: '@import "@/assets/scss/common/common.scss";',
        prependData: '@import "@/assets/scss/common/common.scss";',
        additionalData: '@import "@/assets/scss/common/common.scss";',
        implementation: require('node-sass')
      },
    },
  }
};
