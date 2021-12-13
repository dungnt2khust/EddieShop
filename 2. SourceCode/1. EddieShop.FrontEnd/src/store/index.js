import Vue from 'vue'
import Vuex from 'vuex'
import Guest from '@/router/menu/Guest'
import {AccountType} from "@/models/enum/AccountType.js"
import app from '../main'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: { 
      navBar: Guest,
      Language: 'vi', 
      Route: ""
  },
  mutations: {
    SET_LANG (state, payload) {
      app.$i18n.locale = payload;
      document.title = app.$t(app.$route.meta.Title);
      this.Language = payload;
    }, 
    SET_ROUTE (state, payload) {
      if (localStorage.getItem("AccountType") == AccountType.ADMIN) {
        payload.shift();
      } 
      payload.shift();
      this.Route = payload;
    }
  },
  actions: {
    setLang({commit}, payload) {
      commit('SET_LANG', payload);
    },
    setRoute({commit}, payload) {
      commit('SET_ROUTE', payload);
    }
  }
});

export default store;