<template lang="">
  <div class="listgrid fx-col">
    <div class="listgrid__header fx-row m-b-10">
      <EdSearch v-show="listChecked.length < 1" class="w-full" />
      <div v-show="listChecked.length >= 1" class="listgrid__controls fx-row">
        <div>Đã chọn {{ listChecked.length }}</div>
        <div>Bỏ chọn</div>
        <div>Xoá</div>
      </div>
      <div class="flex-1 fx-row jus-c-fend">
        <slot name="header"></slot>
      </div>
    </div>
    <div class="listgrid__content flex-1 default-scrollbar">
      <table class="listgrid__table h-full w-full">
        <thead class="listgrid__table-header">
          <th style="width: 30px">
            <div class="listgrid__checkbox fx-row jus-c-center aln-i-center">
              <ed-check-box v-model="checkAll" />
            </div>
          </th>
          <th v-for="(header, index) in listHeader" :key="index">
            {{ header.title }}
          </th>
          <th></th>
        </thead>
        <tbody class="listgrid__table-body default-scrollbar">
          <tr v-for="(row, index) in listData" @dblclick="dblClick(row)">
            <td style="width: 30px">
              <div class="fx-row jus-c-center aln-i-center">
                <ed-check-box v-model="value[index]" />
              </div>
            </td>
            <td
              v-for="(header, index) in listHeader"
              v-html="tableData(row, header)"
              :style="{ width: header.width }"
            ></td>
            <td style="width: 100px">
              <div class="listgrid__function fx-row">
                <div
                  @click="editItem(row)"
                  class="mi-edit scale-1.3 m-h-16"
                  v-on="tooltipListeners('Chỉnh sửa')"
                ></div>
                <div
                  @click="deleteItem(row)"
                  class="mi-delete scale-1.3"
                  v-on="tooltipListeners('Xoá')"
                ></div>
              </div>
            </td>
          </tr>
        </tbody>
        <tfoot class="listgrid__table-footer"></tfoot>
      </table>
    </div>
    <div class="listgrid__footer fx-row p-t-10">
      <div class="flex-1">
        <span class="txt-smb-1 txt-s-14">
          Tổng số bản ghi: {{ listData.length }}
        </span>
        <slot name="footer"></slot>
      </div>
      <EdPagination />
    </div>
  </div>
</template>
<script>
export default {
  name: "ListGridTable",
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
    editItem: {
      type: Function,
      default: () => {}
    },
    deleteItem: {
      type: Function,
      default: () => {}
    },
    value: {
      type: Array,
      default: () => []
    },
    listHeader: {
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      checkAll: false
    };
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
  },
  methods: {
    /**
     * Giá trị tại ô dữ liệu
     * CreatedBy: NTDUNG (20/12/2021)
     */
    tableData(row, header) {
      var td;
      switch (header.type) {
        case "number":
          break;
        case "image":
          td = `<div class="fx-row jus-c-center aln-i-center">
            <img style="max-height: 100px; min-height: 100px" src="data:image/gif;base64,${
              row[header.field]
            }" atl="Image" />
            </div>
            `;
          break;
        default:
          td = row[header.field];
          break;
      }
      return td;
    }
  },
  watch: {
    /**
     * Chọn và bỏ chọn tất cả
     * CreatedBy: NTDUNG (20/12/2021)
     */
    checkAll: function(val) {
      var newListCheck = [...this.listData];
      if (val) {
        newListCheck = newListCheck.map(check => true);
      } else {
        newListCheck = newListCheck.map(check => false);
      }
      this.$emit("input", newListCheck);
    },
    "listChecked.length": function(val) {
      if (val == 0) {
        this.checkAll = false;
      }
      if (val == this.listData.length) {
        this.checkAll = true;
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/listgrid/listgrid.scss";
</style>
