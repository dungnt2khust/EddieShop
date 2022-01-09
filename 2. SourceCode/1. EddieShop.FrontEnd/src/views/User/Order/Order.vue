<template lang="">
  <base-content-area
    class="jus-c-center"
    title="Đơn hàng của bạn"
    pHor="20px"
    bgColor="#fff"
  >
    <template v-slot:content>
      <EdFrame class="h-full fx-col" autoScroll>
        <EdListGridAdvance
          class="flex-1 w-full"
          :listHeader="listHeader"
          :pagingInfo="pagingInfo"
          :showSearch="false"
          :showCheck="false"
          :showPaging="false"
          :editRow="false"
          :deleteRow="false"
          @changePagingFilter="changePagingFilter"
          v-model="listOrder"
          itemID="_OrderID"
        >
        </EdListGridAdvance>
      </EdFrame>
    </template>
  </base-content-area>
</template>
<script>
// Plugins
import OrderAPI from "@/api/components/Order/OrderAPI.js";
export default {
  name: "Order",
  data() {
    return {
      pagingInfo: {
        filterString: "",
        pageNum: 1,
        pageSize: 10,
        filterData: {
          TotalFields: [],
          Sorts: []
        }
      },
      listHeader: [
        {
          title: "Mã đơn hàng",
          field: "OrderCode",
          type: "text",
          width: 130,
          headerPos: "left",
          dataPos: "left",
          pin: true
        },
        {
          title: "Mô tả",
          field: "Description",
          width: 500,
          type: "text"
        }
      ],
      listOrder: []
    };
  },
  mounted() {
    this.getOrderData();
  },
  methods: {
    /**
     * Lấy dữ liệu đơn hàng
     * CreatedBy: NTDUNG (09/01/2021)
     */
    getOrderData() {
      OrderAPI.GetFilterPaging(
        this.pagingInfo.filterString,
        this.pagingInfo.pageNum,
        this.pagingInfo.pageSize,
        this.pagingInfo.filterData
      )
        .then(res => {
          if (res.data.Success) {
            this.listOrder = res.data.Data.Records;
            this.pagingInfo.totalPage = res.data.Data.TotalPage;
            this.pagingInfo.totalRecord = res.data.Data.TotalRecord;
            this.pagingInfo.totalData = res.data.Data.TotalDatas;
          }
        })
        .catch(err => {
          console.log(err);
          this.$toast.error("Có lỗi xảy ra");
        });
    },
    /**
     * Paging filter thay đổi
     * CreatedBy: NTDUNG (21/12/2021)
     */
    changePagingFilter(pageNum, pageSize, filterString) {
      this.pagingInfo.pageNum = pageNum;
      this.pagingInfo.pageSize = pageSize;
      this.pagingInfo.filterString = filterString;
      this.getOrderData();
    },
  }
};
</script>
<style lang=""></style>
