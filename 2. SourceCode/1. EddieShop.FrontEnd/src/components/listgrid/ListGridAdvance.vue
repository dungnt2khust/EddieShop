<template lang="">
  <div class="listgrid w-full fx-col" :style="customizeStyle(styleCustom)">
    <div class="listgrid__header fx-row m-b-10">
      <EdSearch
        v-show="listCheck.length < 1 && showSearch"
        class="h-full"
        style="width: 300px;"
        v-model="filterString"
      />
      <div v-show="listCheck.length >= 1" class="listgrid__controls fx-row">
        <div class="listgrid__checked">Đã chọn {{ listCheck.length }}</div>
        <div @click="uncheckAll" class="listgrid__uncheck m-r-10">Bỏ chọn</div>
        <div @click="$emit('DeleteMulti')" class="listgrid__delete">Xoá</div>
      </div>
      <div class="flex-1 fx-row jus-c-fend">
        <slot name="header"></slot>
      </div>
    </div>
    <div
      ref="listgridContent"
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
                  class="mi-Delete scale-1.3"
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
        :class="{ 'flex-1': value.length == 0 }"
        v-columns-resizable
        ref="listgridTable"
        style="min-width: 100%"
      >
        <!-- style="min-width: 100%;max-width: 100%" -->
        <thead class="listgrid__table-header" ref="thead">
          <tr>
            <th
              :width="checkboxWidth"
              :style="
                `min-width: ${checkboxWidth}px !important;max-width: ${checkboxWidth}px !important`
              "
            >
              <div v-show="showCheck" class="listgrid__checkbox fx-row jus-c-center aln-i-center">
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
                { 'max-width': header.pin ? header.width + 'px' : '' },
                header.pin ? pin(index) : {}
              ]"
            >
              {{ header.title }}
            </th>
            <th
              :style="
                `min-width: ${functionWidth}px; max-width: ${functionWidth}px`
              "
            >
              <div
                class="listgrid__edit fx-row jus-c-center aln-i-center cur-p"
                v-on="tooltipListeners('Chỉnh sửa')"
                v-if="editAll"
              >
                <div
                  @click="editAllState = true"
                  v-if="!editAllState"
                  class="mi-edit scale-1.5"
                ></div>
                <ed-button
                  :method="saveTable"
                  v-else
                  label="Lưu"
                  :styleBtn="1"
                  bgColor="#5DCE00"
                />
              </div>
            </th>
          </tr>
        </thead>
        <tbody class="listgrid__table-body">
          <tr v-if="!value.length">
            <div class="fx-row jus-c-center aln-i-center">
              Không có dữ liệu
            </div> 
          </tr>
          <tr v-for="(row, index) in value" @dblclick="$emit('dblClick', row)">
            <td>
              <div v-show="showCheck" class="fx-row jus-c-center aln-i-center">
                <ed-check-box
                  :value="
                    listCheck.findIndex(item => item == row[itemID]) != -1
                  "
                  @changeState="checkItem($event, row[itemID])"
                />
              </div>
            </td>
            <td
              v-for="(header, index) in listHeader"
              :style="[
                {
                  'white-space': header.wrap || header.pin ? 'wrap' : 'nowrap',
                  'min-width': header.width + 'px',
                  'max-width': header.pin ? header.width + 'px' : ''
                },
                { padding: header.type == 'image' ? 0 : '' },
                header.pin ? pin(index) : {}
              ]"
            >
              <div class="fx-row" :style="posData(header.dataPos)">
                <EdNumber
                  v-if="header.type == 'number' && editAll && editAllState"
                  :min="header.min"
                  :max="header.max"
                  v-model="row[header.field]"
                />
              </div>
              <div
                class="td-wrapper fx-row"
                :style="posData(header.dataPos)"
                v-if="header.type != 'custom'"
                v-html="tableData(row, header, index)"
              ></div>
              <slot v-else :name="header.field" v-bind="{ row }"></slot>
            </td>
            <!-- header.pin ? headerStylePin(index) : '' -->
            <td>
              <div class="listgrid__function fx-row">
                <div
                  v-show="editRow"
                  @click="$emit('edit', row)"
                  class="mi-edit scale-1.3 m-h-16"
                  v-on="tooltipListeners('Chỉnh sửa')"
                ></div>
                <div
                  v-show="deleteRow"
                  @click="$emit('Delete', row)"
                  class="mi-Delete scale-1.3"
                  v-on="tooltipListeners('Xoá')"
                ></div>
              </div>
            </td>
          </tr>
        </tbody>
        <tfoot class="listgrid__table-footer" v-if="haveFooter">
          <tr>
            <td></td>
            <td
              v-for="(header, index) in listHeader"
              :style="header.pin ? pin(index) : {}"
            >
              <div v-if="index == 0" class="w-full">Tổng</div> 
              <div v-if="header.total">
                <div class="m-v-4" :style="{ 'text-align': header.dataPos }">
                  {{
                    header.total &&
                      totalInPage(header)
                  }}
                </div>

                <div v-if="pagingInfo.totalPage > 1">
                  <div
                    class="line"
                    style="width: 100%;height:1px;background-color:#888;"
                  ></div>

                  <div class="m-v-4" :style="{ 'text-align': header.dataPos }">
                    {{
                      header.total &&
                      pagingInfo.totalData && pagingInfo.totalData[header.field]
                        ? footerData(pagingInfo.totalData[header.field].All, header)
                        : ""
                    }}
                  </div>
                </div>
              </div>
            </td>
            <td></td>
          </tr>
        </tfoot>
      </table>
    </div>
    <div v-show="showPaging" class="listgrid__footer fx-row p-t-10">
      <div class="flex-1">
        <span class="txt-smb-1 txt-s-14">
          Tổng số bản ghi: {{ pagingRecordInfo }}
        </span>
        <slot name="footer"></slot>
      </div>
      <EdPagination
        @changePaging="changePaging"
        :currPage="pagingInfo.pageNum"
        :totalPage="pagingInfo.totalPage"
        :totalRecord="pagingInfo.totalRecord"
      />
    </div>
  </div>
</template>
<script>
export default {
  name: "ListGridAdvance",
  props: {
    height: {
      type: String,
      default: null
    },
    width: {
      type: String,
      default: null
    },
    minHeight: {
      type: String,
      default: null
    },
    minWidth: {
      type: String,
      default: null
    },
    maxHeight: {
      type: String,
      default: null
    },
    maxWidth: {
      type: String,
      default: null
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
      default: () => ({})
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
      default: () => ({})
    },
    editRow: {
      type: Boolean,
      default: true
    },
    deleteRow: {
      type: Boolean,
      default: true
    },
    editAll: {
      type: Boolean,
      default: false
    },
    listCheck: {
      type: Array,
      default: () => []
    },
    totalOnSelect: {
      type: Boolean,
      default: false
    },
    showCheck: {
      type: Boolean,
      default: true
    },
    showSearch: {
      type: Boolean,
      default: true
    },
    showHeader: {
      type: Boolean,
      default: true
    },
    showPaging: {
      type: Boolean,
      default: true
    }
  },
  data() {
    return {
      checkAll: false,
      filterString: "",
      checkboxWidth: 60,
      functionWidth: 100,
      editAllState: false,
      styleCustom: {
        "height": this.height,
        "width": this.width,
        "min-height": this.minHeight,
        "min-width": this.minWidth,
        "max-height": this.maxHeight,
        "max-width": this.minWidth
      }
    };
  },
  // created() {
  //   window.addEventListener('resize', this.handleResize);
  // },
  // destroyed() {
  //   window.removeEventListener('resize', this.handleResize, true)
  // },
  computed: {
    /**
     * Thông tin bản ghi của danh sách
     * CreatedBy: NTDUNG (21/12/2021)
     */
    pagingRecordInfo() {
      return `${(this.pagingInfo.pageNum - 1) * this.pagingInfo.pageSize + 1}-
            ${
              this.value.length < this.pagingInfo.pageSize
                ? (this.pagingInfo.pageNum - 1) * this.pagingInfo.pageSize +
                  this.value.length
                : this.pagingInfo.pageNum * this.pagingInfo.pageSize
            }/${this.pagingInfo.totalRecord}`;
    },
    /**
     * Xác định có phải đang checkall
     * CreatedBy: NTDUNG (21/12/2021)
     */
    isCheckAll() {
      var checkAll = true;
      if (this.value.length == 0) checkAll = false;
      else
        this.value.forEach(data => {
          if (!this.listCheck.find(id => data[this.itemID] == id))
            checkAll = false;
        });
      return checkAll;
    },
    /**
     * Có footer hay không
     * CreatedBy: NTDUNG (23/12/2021)
     */
    haveFooter() {
      var totalFields = this.listHeader.filter(header => header.total);
      return totalFields.length;
    }
  },
  methods: {
    /**
     * Tổng trong trang
     * CreatedBy: NTDUNG (28/12/2021)
     */
    totalInPage(header) {
      if (!this.totalOnSelect) {
        var sum = 0
        this.value.forEach(row => {
          sum += row[header.field];
        });
        return this.footerData(sum, header);
      } else {
        if (this.listCheck.length == 0) {
          var sum = 0
          this.value.forEach(row => {
            sum += row[header.field];
          });
          return this.footerData(sum, header);
        } else {
          var checkedRow = this.value.filter(
            row => this.listCheck.findIndex(id => id == row[this.itemID]) != -1
          );
          var sum = 0;
          checkedRow.forEach(row => {
            sum += row[header.field];
          });
          return this.footerData(sum, header);
        }
      }
    },
    /**
     * Giá trị tại ô dữ liệu
     * CreatedBy: NTDUNG (20/12/2021)
     */
    tableData(row, header, index) {
      var td;
      switch (header.type) {
        case "number":
          if (!(this.editAll && this.editAllState))
            td = this.formatNumber(row[header.field]);
          break;
        case "money":
          td = this.formatMoney(row[header.field]);
          break;
        case "date":
          td = this.formatDate(row[header.field]);
          break;
        case "datetime":
          td = this.formatDateTime(row[header.field]);
          break;
        case "image":
          td = `<div class="fx-row jus-c-center aln-i-center no-select" style="overflow: hidden; max-width: 100%">
            <img style="max-height: ${header.height}; min-height: ${
            header.height
          }" src="${row[header.field]}" atl="Image" />
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
     * Định dạng dữ liệu
     * CreatedBy: NTDUNG(23/12/2021)
     */
    footerData(data, header) {
      var tfoot;
      switch (header.type) {
        case "number":
          tfoot = this.formatNumber(data);
          break;
        case "money":
          tfoot = this.formatMoney(data);
          break;
        default:
          tfoot = data;
          break;
      }
      return tfoot;
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
      var tempArr = [...this.listCheck];
      if (state) {
        tempArr.push(itemID);
      } else {
        tempArr = tempArr.filter(item => {
          return item != itemID;
        });
      }
      this.$emit("changeListCheck", tempArr);
    },
    /**
     * Bỏ chọn tất cả
     * CreatedBy: NTDUNG (21/12/2021)
     */
    uncheckAll() {
      this.$emit("changeListCheck", []);
    },
    /**
     * Bật tắt checkall
     * CreatedBy: NTDUNG (21/12/2021)
     */
    toggleCheckAll(state) {
      var newCheck = [...this.listCheck];
      if (state) {
        this.value.forEach(item => {
          if (!this.listCheck.find(id => item[this.itemID] == id))
            newCheck.push(item[this.itemID]);
        });
      } else {
        newCheck = this.listCheck.filter(id => {
          return !(this.value.findIndex(data => data[this.itemID] == id) != -1);
        });
      }
      this.$emit("changeListCheck", newCheck);
    },
    /**
     * Pin
     * CreatedBy: NTDUNG (21/12/2021)
     */
    pin(index) {
      var leftPos = this.checkboxWidth;
      for (var i = 0; i < index; i++) {
        if (this.listHeader[i] && this.listHeader[i].width) {
          leftPos += this.listHeader[i].width;
        }
      }
      return { position: "sticky", left: leftPos + "px" };
    },
    /**
     * Lưu bảng
     * CreatedBy: NTDUNG (23/12/2021)
     */
    saveTable() {
      this.$emit("saveTable");
      this.editAllState = false;
    },
    /**
     * Vị trí của bản ghi
     * CreatedBy: NTDUNG (23/12/2021)
     */
    posData(pos) {
      var style = {};
      switch (pos) {
        case "left":
          style["justify-content"] = "flex-start";
          break;
        case "right":
          style["justify-content"] = "flex-end";
          break;
        case "center":
          style["justify-content"] = "center";
          break;
      }
      return style;
    },
    /**
     * Xử lý khi thay đổi màn hình
     * CreatedBy: NTDUNG (25/12/2021)
     */
    handleResize(e) {
      this.$refs.listgridTable.style.width = `${this.$refs.listgridContent.offsetWidth}px`;
      this.$refs.listgridTable.style.minWidth = `${this.$refs.listgridContent.offsetWidth}px`;
      this.$refs.listgridTable.style.maxWidth = `${this.$refs.listgridContent.offsetWidth}px`;
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
    },
    "value.length": function(val) {
      if (!val && this.pagingInfo.pageNum > 1) {
        this.changePaging(
          this.pagingInfo.pageNum - 1,
          this.pagingInfo.pageSize
        );
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/listgrid/listgridadvance.scss";
</style>
