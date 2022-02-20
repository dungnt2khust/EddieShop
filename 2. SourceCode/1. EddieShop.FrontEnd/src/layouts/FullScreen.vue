<template lang="">
  <div
    class="full-screen default-scrollbar fx-row"
    :style="[
      {
        'background-image': image ? `url(${image})` : 'unset',
        'background-repeat': `no-repeat`,
        'background-size': `cover`
      },
      customizeStyle(styleCustom)
    ]"
  >
    <slot></slot>
  </div>
</template>
<script>
export default {
  name: "full-screen",
  props: {
    bgColor: {
      type: String,
      default: null
    },
    image: {
      type: String,
      default: null
    }
  },
  data() {
    return {
      styleCustom: {
        "background-color": this.bgColor
      }
    };
  },
  computed: {}
};
</script>
<style lang="scss">
@import "@/assets/scss/layouts/fullscreen.scss";
</style>
