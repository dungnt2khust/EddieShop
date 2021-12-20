<template>
  <!-- eslint-disable  -->
  <div class="pagination">
    <paginate
      :page-count="20"
      :page-range="1"
      :margin-pages="2"
      :click-handler="clickCallback"
      :next-text="'>'"
      :container-class="'pagination'"
      :page-class="'page-item'"
      :active-class="'active-class'"
      :first-last-button="true"
    >
      <template v-slot:prevContent>
        <div class="prevButton">PREV</div>
      </template>
      <template v-slot:nextContent>
        <div class="nextButton">NEXT</div>
      </template>
      <template v-slot:breakViewContent>
        <svg width="16" height="4" viewBox="0 0 16 4">
          <circle fill="#999999" cx="2" cy="2" r="2" />
          <circle fill="#999999" cx="8" cy="2" r="2" />
          <circle fill="#999999" cx="14" cy="2" r="2" />
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
      default: 1,
    },
  },
  methods: {
    clickCallback(pageNum) {
      this.$emit("changePage", pageNum);
    },
  },
};
</script>

<style lang="scss">
.pagination {
  height: 30px;
  background-color: aqua;
}
.pagination {
  display: flex;
  list-style: none;
  height: 30px;
}
.page-item {
  padding: 0;
  height: 100%;
  display: block;
  & > a {
    padding: 4px 10px;
    user-select: none;
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100%;
    &:hover {
      background-color: #ccc;
      cursor: pointer;
    }
  }
}
.prevButton {
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  padding: 0 4px;
  user-select: none;
}
.disabled .prevButton,
.disabled .nextButton {
  opacity: 0.5;
}
.nextButton {
  cursor: pointer;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  padding: 0 4px;
  user-select: none;
}
.active-class {
  background-color: green;
  color: #fff;
}
</style>