<template>
  <div id="app">
    <Header/>
    <Main/>
    <Tooltip/>
    <Loading/>
    <Dialog/>
    <Toast/>
  </div>
</template>

<script>
// Components
import Header from "@/layouts/header/Header.vue"
import Main from "@/layouts/main/Main.vue"
import Tooltip from "@/components/tooltip/Tooltip.vue"
import Loading from "@/layouts/loading/Loading.vue"
import Dialog from "@/components/dialog/Dialog.vue"
import Toast from "@/components/toast/Toast.vue"
// Plugins

export default {
  name: "App",
  components: {
    Header,
    Main,
    Tooltip,
    Loading,
    Dialog,
    Toast
  } 
};

</script>
<style>
  @import url('./assets/css/main.css');
</style>
<style lang="scss">
  @import "@/assets/scss/common/common.scss";
</style>
