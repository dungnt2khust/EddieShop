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
      Route: "", 
      PreviousRoute: null,
      AccountData: {}
  },
  getters: {
    DisplayName(state) {
      return state.AccountData.DisplayName;
    }
  },
  mutations: {
    SET_LANG (state, payload) {
      app.$i18n.locale = payload;
      document.title = app.$t(app.$route.meta.Title);
      state.Language = payload;
      // this.Language = payload;
    }, 
    SET_ROUTE (state, payload) {
      if (localStorage.getItem("AccountType") == AccountType.ADMIN) {
        payload.shift();
      } 
      payload.shift();
      state.Route = payload;
    },
    SET_PREVIOUS_ROUTE(state, payload) {
      state.PreviousRoute = payload;
    },
    SET_ACCOUNT_DATA(state, payload) {
      state.AccountData = payload;
    }
  },
  actions: {
    setLang({commit}, payload) {
      commit('SET_LANG', payload);
    },
    setRoute({commit}, payload) {
      commit('SET_ROUTE', payload);
    },
    setPreviousRoute({commit}, payload) {
      commit('SET_PREVIOUS_ROUTE', payload)
    },
    setAccountData({commit}, payload) {
      commit('SET_ACCOUNT_DATA', payload)
    }
  }
});

export default store;