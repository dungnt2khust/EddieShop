'use strict'
const merge = require('webpack-merge')
const prodEnv = require('./prod.env')

module.exports = merge(prodEnv, {
  NODE_ENV: '"development"',
  BASE_URL: '"https://localhost:44328"',
  CDNToken: 'Bearer 7QHhg-I1-qMay9f8pvTl1mycnkSBV8TBBPQh_gHt'
})
