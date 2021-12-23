<template lang="">
  <div class="listgrid w-full fx-col" :class="{'default-scrollbar': autoScroll}">
    <div class="listgrid__header fx-row m-b-10">
      <EdSearch v-show="listChecked.length < 1" class="w-full"/>
      <div v-show="listChecked.length >= 1" class="listgrid__controls fx-row">
        <div>Đã chọn {{listChecked.length}}</div>
        <div>Bỏ chọn</div>
        <div>Xoá</div>
      </div>
      <div class="flex-1 fx-row jus-c-fend">
        <slot name="header"></slot>
      </div>
    </div>
    <div class="listgrid__content flex-1 default-scrollbar">
      <ul class="listgrid__list">
        <li
          v-for="(item, index) in listData"
          @dblclick="dblClick(item[itemID])"
          class="listgrid__item fx-row jus-c-sbtn p-v-16 p-h-16"
          tabindex="0"
          :key="index"
        >
          <div class="fx-row">
            <div class="listgrid__checkbox p-r-10">
              <ed-check-box v-model="value[index]" />
            </div>
            {{ query ? item[query] : item }}
          </div>

          <div class="listgrid__function fx-row">
            <div
              @click="editItem(item)"
              class="mi-edit scale-1.3 m-h-16"
              v-on="tooltipListeners('Chỉnh sửa')"
            ></div>
            <div
              @click="deleteItem(item)"
              class="mi-Delete scale-1.3"
              v-on="tooltipListeners('Xoá')"
            ></div>
          </div>
        </li>
      </ul>
      <slot name="content"></slot>
    </div>
    <div class="listgrid__footer">
      <slot name="footer"></slot>
    </div>
  </div>
</template>
<script>
export default {
  name: "ListGrid",
  props: {
    listData: {
      type: Array,
      default: () => []
    },
    query: {
      type: String,
      default: null
    },
    dblClick: {
      type: Function,
      default: () => {}
    },
    itemID: {
      type: String,
      default: null
    },
    editItem: {
      type: Function,
      default: () => {}
    },
    deleteItem: {
      type: Function,
      default: () => {}
    },
    autoScroll: {
      type: Boolean,
      default: true
    },
    value: {
      type: Array,
      default: () => []
    }
  },
  computed: {
    /**
     * Những bản ghi đang check
     * CreatedBy: NTDUNG (20/12/2021)
     */
    listChecked() {
      return this.value.filter(check => {
        return check;
      });
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/listgrid/listgrid.scss";
</style>
