<template lang="">
  <div class="listgrid w-full fx-col">
    <div class="listgrid__header fx-row m-b-10">
      <EdSearch
        v-show="value.length < 1"
        class="h-full"
        style="width: 300px;"
        v-model="filterString"
      />
      <div v-show="value.length >= 1" class="listgrid__controls fx-row">
        <div class="listgrid__checked">Đã chọn {{ value.length }}</div>
        <div @click="uncheckAll" class="listgrid__uncheck m-r-10">Bỏ chọn</div>
        <div @click="$emit('deleteMulti')" class="listgrid__delete">Xoá</div>
      </div>
      <div class="flex-1 fx-row jus-c-fend">
        <slot name="header"></slot>
      </div>
    </div>
    <div
      class="listgrid__content flex-1 fx-col w-full default-scrollbar pos-relative"
    >
      <!-- <div class="listgrid__table fx-col h-full default-scrollbar">
        <div class="listgrid__table-header fx-row">
          <div class="listgrid__table-th" style="width: 50px">
            <div class="listgrid__checkbox fx-row jus-c-center aln-i-center">
              <ed-check-box v-model="checkAll" />
            </div>
          </div>
          <div
            class="listgrid__table-th"
            v-for="(header, index) in listHeader"
            :key="index"
            :style="{
              'min-width': header.width ? header.width : header.minWidth,
              'max-width': header.width ? header.width : ''
            }"
            :class="{ 'flex-1': header.minWidth}"
          >
            {{ header.title }}
          </div>
          <div
            class="listgrid__table-th"
            style="min-width: 100px;max-width: 100px"
          ></div>
        </div>
        <div class="listgrid__table-body flex-1">
          <div
            class="listgrid__table-tr fx-row"
            v-for="(row, index) in listData"
            @dblclick="dblClick(row)"
          >
            <div class="listgrid__table-td" style="width: 50px">
              <div class="fx-row jus-c-center aln-i-center">
                <ed-check-box
                  :value="value.findIndex(item => item == row[itemID]) != -1"
                  @changeState="checkItem($event, row[itemID])"
                />
              </div>
            </div>
            <div
              class="listgrid__table-td"
              v-for="(header, index) in listHeader"
              v-html="tableData(row, header)"
              :style="{
                'min-width': header.width ? header.width : header.minWidth,
                'max-width': header.width ? header.width : ''
              }"
              :class="{ 'flex-1': header.minWidth}"
            ></div>
            <div
              class="listgrid__table-td"
              style="min-width: 100px;max-width: 100px"
            >
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
            </div>
          </div>
        </div>
        <div class="listgrid__table-footer"></div>
      </div> -->
      <table
        class="listgrid__table"
        :class="{ 'flex-1': listData.length == 0 }"
        v-columns-resizable
        style="min-width: 100%;"
      >
        <thead class="listgrid__table-header">
          <tr>
            <th
              :style="
                `min-width: ${checkboxWidth}px; max-width: ${checkboxWidth}px`
              "
            >
              <div class="listgrid__checkbox fx-row jus-c-center aln-i-center">
                <ed-check-box
                  :value="isCheckAll"
                  @changeState="toggleCheckAll"
                />
              </div>
            </th>
            <th
              v-for="(header, index) in listHeader"
              :key="index"
              :style="[
                {
                  'text-align': header.headerPos,
                  'min-width': header.width + 'px'
                },
              ]"
            >
                <!-- header.pin ? headerStylePin(index) : '' -->
              {{ header.title }}
            </th>
            <th
              :style="
                `min-width: ${functionWidth}px; max-width: ${functionWidth}px`
              "
            ></th>
          </tr>
        </thead>
        <tbody class="listgrid__table-body">
          <tr v-for="(row, index) in listData" @dblclick="dblClick(row)">
            <td>
              <div class="fx-row jus-c-center aln-i-center">
                <ed-check-box
                  :value="value.findIndex(item => item == row[itemID]) != -1"
                  @changeState="checkItem($event, row[itemID])"
                />
              </div>
            </td>
            <td
              v-for="(header, index) in listHeader"
              :style="[{
                'text-align': header.dataPos,
                'white-space': header.wrap ? 'wrap' : 'nowrap'
              }, 
              ]"
              v-html="tableData(row, header)"

            ></td>
              <!-- header.pin ? headerStylePin(index) : '' -->
            <td>
              <div class="listgrid__function fx-row">
                <div
                  @click="$emit('edit', row)"
                  class="mi-edit scale-1.3 m-h-16"
                  v-on="tooltipListeners('Chỉnh sửa')"
                ></div>
                <div
                  @click="$emit('delete', row)"
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
          Tổng số bản ghi: {{ pagingRecordInfo }}
        </span>
        <slot name="footer"></slot>
      </div>
      <EdPagination
        @changePaging="changePaging"
        :totalPage="pagingInfo.totalPage"
        :totalRecord="pagingInfo.totalRecord"
      />
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
    itemID: {
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
    },
    pagingInfo: {
      type: Object,
      default: () => {}
    }
  },
  data() {
    return {
      checkAll: false,
      filterString: "",
      checkboxWidth: 50,
      functionWidth: 100
    };
  },
  computed: {
    /**
     * Thông tin bản ghi của danh sách
     * CreatedBy: NTDUNG (21/12/2021)
     */
    pagingRecordInfo() {
      return `${(this.pagingInfo.pageNum - 1) * this.pagingInfo.pageSize + 1}-
            ${
              this.listData.length < this.pagingInfo.pageSize
                ? (this.pagingInfo.pageNum - 1) * this.pagingInfo.pageSize +
                  this.listData.length
                : this.pagingInfo.pageNum * this.pagingInfo.pageSize
            }/${this.pagingInfo.totalRecord}`;
    },
    /**
     * Xác định có phải đang checkall
     * CreatedBy: NTDUNG (21/12/2021)
     */
    isCheckAll() {
      var checkAll = true;
      if (this.listData.length == 0) checkAll = false;
      else
        this.listData.forEach(data => {
          if (!this.value.find(id => data[this.itemID] == id)) checkAll = false;
        });
      return checkAll;
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
          td = this.formatNumber(row[header.field]);
          break;
        case "money":
          td = this.formatMoney(row[header.field]);
          break;
        case "image":
          td = `<div class="fx-row jus-c-center aln-i-center">
            <img style="max-height: ${header.height}; min-height: ${
            header.height
          }" src="data:image/gif;base64,${row[header.field]}" atl="Image" />
            </div>
            `;
          break;
        default:
          td = row[header.field];
          break;
      }
      return td;
    },
    /**
     * Paging thay đổi
     * CreatedBy: NTDUNG (21/12/2021)
     */
    changePaging(pageNum, pageSize) {
      this.$emit("changePagingFilter", pageNum, pageSize, this.filterString);
    },
    /**
     * Check item
     * CreatedBy: NTDUNG (21/12/2021)
     */
    checkItem(state, itemID) {
      var tempArr = [...this.value];
      if (state) {
        tempArr.push(itemID);
      } else {
        tempArr = tempArr.filter(item => {
          return item != itemID;
        });
      }
      this.$emit("input", tempArr);
    },
    /**
     * Bỏ chọn tất cả
     * CreatedBy: NTDUNG (21/12/2021)
     */
    uncheckAll() {
      this.$emit("input", []);
    },
    /**
     * Bật tắt checkall
     * CreatedBy: NTDUNG (21/12/2021)
     */
    toggleCheckAll(state) {
      var newCheck = [...this.value];
      if (state) {
        this.listData.forEach(item => {
          if (!this.value.find(id => item[this.itemID] == id))
            newCheck.push(item[this.itemID]);
        });
      } else {
        newCheck = this.value.filter(id => {
          return !(
            this.listData.findIndex(data => data[this.itemID] == id) != -1
          );
        });
      }
      this.$emit("input", newCheck);
    },
    /**
     * Pin header
     * CreatedBy: NTDUNG (21/12/2021)
     */
    headerStylePin(index) {
      var leftPos = this.checkboxWidth;
      for (var i = 0; i < index; i++) {
        if (
          this.listHeader[index].width &&
          !Number.isNaN(this.listHeader[index].width)
        ) {
          leftPos += Number(this.listHeader[index].width);
          console.log(this.listHeader[index].width);
        }
      }
      return { position: "sticky", left: leftPos + "px" };
    }
  },
  watch: {
    filterString: function(val) {
      this.$emit(
        "changePagingFilter",
        this.pagingInfo.pageNum,
        this.pagingInfo.pageSize,
        this.filterString
      );
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/listgrid/listgrid.scss";
</style>
