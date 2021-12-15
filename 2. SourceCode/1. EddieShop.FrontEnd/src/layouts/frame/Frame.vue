<template lang="">
  <div
    class="frame fx-wrap flex-1"
    :class="[
      gutH ? 'gut-h-' + gutH : '',
      gutV ? 'gut-v-' + gutV : ''
    ]"
    :style="customizeStyle(styleCustom)"
  >
    <slot></slot>
  </div>
</template>
<script>
export default {
  name: "Frame",
  props: {
    gutH: {
      type: [String, Number],
      default: null
    },
    gutV: {
      type: [String, Number],
      default: null
    },
    height: {
      type: [String, Number],
      default: null
    },
    fullWidth: {
      type: Boolean,
      default: false
    },
    autoScroll: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      styleCustom: {
        height: this.height
      }
    };
  }
};
</script>
<style lang=""></style>
