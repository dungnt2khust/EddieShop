<template lang="">
  <div class="search">
    <EdInput v-model="filterString" :placeholder="placeholder" :duration="500"/>
  </div>
</template>
<script>
export default {
  name: "Search",
  props: {
    value: {
      type: [Number, String],
      default: ""
    },
    placeholder: {
      type: [Number, String],
      default: "Tìm kiếm"
    }
  },
  data() {
    return {
      filterString: this.value
    };
  },
  watch: {
    filterString: function(val) {
      this.$emit("input", val);
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/search/search.scss";
</style>
