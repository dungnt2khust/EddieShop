import Vue from 'vue' 
import Router from 'vue-router'

// Router
import User from './route/User'
import Admin from './route/Admin'
import Authen from './route/Authen'

Vue.use(Router)

export default new Router({
    mode: "history",
    routes: [
        ...Authen,
        ...User,
        ...Admin,
        {path: "/*", redirect: "/not-found"}
    ]
})