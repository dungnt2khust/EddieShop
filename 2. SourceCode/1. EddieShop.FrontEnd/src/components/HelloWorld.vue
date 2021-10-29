<template>
  <div class="hello">
    <h1>{{$t("Hello")}}</h1>
    <button v-for="entry in languages" :key="entry.title" @click="changeLocale(entry.language)">
      <flag :iso="entry.flag" v-bind:squared=false />
      {{entry.title}}
    </button>
  </div>
</template>

<script>
/* eslint-disable */
import i18n from "@/i18n/i18n.js"
export default {
  name: 'HelloWorld',
  data () {
    return {
      languages: [
        { flag: 'us', language: 'en', title: 'English' },
        { flag: 'vn', language: 'vi', title: 'Tiếng Việt' }
      ]
    }
  },
  methods: {
    changeLocale(locale) {
      i18n.locale = locale;
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
button {
  padding: 15px;
  border: 1px solid green;
  font-size: 18px;
  margin: 15px;
}
</style>
