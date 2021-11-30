import Vue from 'vue'
import Vuex from 'vuex'
import Guest from '@/router/menu/Guest'

Vue.use(Vuex)

const store = new Vuex.Store({
  state: { 
      navBar: Guest
  },
  mutations: {
    
  }
});

export default store;