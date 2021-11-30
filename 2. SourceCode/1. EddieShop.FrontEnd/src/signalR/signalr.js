import Vue from 'vue'

const signalR = require("@aspnet/signalr");

Vue.prototype.$SignalR = new signalR.HubConnectionBuilder()
      .withUrl(`${process.env.BASE_URL}/hub/chat`)
      .configureLogging(signalR.LogLevel.Information)
      .build();

export default Vue;