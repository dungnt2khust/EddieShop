<template>
  <!-- eslint-disable  -->
  <div class="pagination">
    <ed-select-box
      :options="options"
      v-model="currOption"
      :up="true"
      width="200px"
    />
    <paginate
      :page-count="totalPage"
      :page-range="1"
      :margin-pages="2"
      :click-handler="clickCallback"
      :container-class="'pagination'"
      :page-class="'page-item'"
      :active-class="'active-class'"
      :next-class="'next-class'"
      :prev-class="'prev-class'"
      next-text=">"
      prev-text="<"
      first-button-text="<<"
      last-button-text=">>"
      :first-last-button="true"
    >
      <template v-slot:breakViewContent>
        <svg width="16" height="4" viewBox="0 0 16 4">
          <circle fill="#000" cx="2" cy="1" r="1" />
          <circle fill="#000" cx="8" cy="1" r="1" />
          <circle fill="#000" cx="14" cy="1" r="1" />
        </svg>
      </template>
    </paginate>
  </div>
</template>

<script>
/* eslint-disable */
export default {
  name: "Pagination",
  props: {
    value: {
      type: [String, Number],
      default: 1
    },
    options: {
      type: Array,
      default: () => [10, 20, 50, 100]
    },
    totalPage: {
      type: [Number, String],
      default: 0
    }
  },
  data() {
    return {
      currOption: 0,
      pageNum: 1
    };
  },
  methods: {
    clickCallback(pageNum) {
      this.pageNum = pageNum;
      this.$emit("changePaging", pageNum, this.options[this.currOption]);
    }
  },
  watch: {
    currOption: function(val) {
      this.$emit("changePaging", this.pageNum, this.options[val]);
    }
  }
};
</script>

<style lang="scss">
@import "@/assets/scss/components/pagination/pagination.scss";
</style>
