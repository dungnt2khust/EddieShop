<template lang="">
  <div class="loading">
    <FullScreen
      v-show="loadingState"
      bgColor="#fff"
      class="fx-row jus-c-center aln-i-center"
    >
      <div class="loading__icon mi-loading scale-1.5"></div>
      <span>{{ message }}</span>
    </FullScreen>
  </div>
</template>
<script>
export default {
  name: "Loading",
  data() {
    return {
      loadingState: false,
      message: "Loading ..."
    };
  },
  created() {
    this.$bus.$on("showLoading", (message, duration) => {
      this.loadingState = true;
      this.message = message;

      if (duration) {
        setTimeout(() => {
          this.loadingState = false;
        }, duration);
      }
    });
    this.$bus.$on("hideLoading", () => {
      this.loadingState = false;
    });
  },
  destroyed() {
    this.$bus.$off("showLoading");
    this.$bus.$off("hideLoading");
  }
};
</script>
<style lang="scss" scoped>
    .loading__icon {
        animation: rotate 0.8s linear infinite;
    }

    @keyframes rotate {
        0% {
            transform: rotate(0) scale(1.5);
        } 
        100% {
            transform: rotate(360deg) scale(1.5);
        }
    }
</style>
