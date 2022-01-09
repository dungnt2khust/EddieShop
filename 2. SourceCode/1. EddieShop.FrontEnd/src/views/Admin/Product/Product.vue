<template lang="">
  <base-content-area
    class="jus-c-center"
    title="Quản lý sản phẩm"
    pHor="20px"
    bgColor="#fff"
  >
    <template v-slot:header>
      <div class="fx-row jus-c-fend">
        <ed-button
          :method="
            () => {
              productInfo = {};
              modePopup = false;
              showAddProduct = true;
            }
          "
          width="fit-content"
          label="Thêm sản phẩm"
          styleBtn="1"
        />
      </div>
    </template>
    <template v-slot:content>
      <EdFrame class="h-full fx-col" autoScroll>
        <EdListGridAdvance
          class="flex-1 w-full"
          :listData="listProduct"
          :listHeader="listHeader"
          :listCheck="listCheck"
          :pagingInfo="pagingInfo"
          @dblClick="productDetail"
          @edit="showEditProduct"
          @Delete="deleteProduct"
          @DeleteMulti="DeleteMulti"
          @changePagingFilter="changePagingFilter"
          @changeListCheck="changeListCheck($event)"
          v-model="listProduct"
          itemID="ProductID"
        >
          <template v-slot:header>
            <ed-select-box
              class="h-full"
              :options="options"
              v-model="currOption"
              width="200px"
              query="name"
            />
          </template>
        </EdListGridAdvance>
      </EdFrame>
      <AddProduct
        @addProduct="addProduct"
        @editProduct="editProduct"
        @close="showAddProduct = false"
        :mode="modePopup"
        v-model="productInfo"
        v-if="showAddProduct"
      />
    </template>
  </base-content-area>
</template>
<script>
// Plugins
import ProductAPI from "@/api/components/Product/ProductAPI.js";
// Components
import AddProduct from "@/views/Admin/Product/AddProduct.vue";

export default {
  name: "Product",
  components: {
    AddProduct
  },
  data() {
    return {
      options: [
        { name: "Tất cả sản phẩm" },
        { name: "Hàng đổi trả" },
        { name: "Hàng tồn kho" },
        { name: "Hàng 2hand" }
      ],
      currOption: 0,
      listProduct: [],
      showAddProduct: false,
      modePopup: false,
      listCheck: [],
      listHeader: [
        {
          title: "Mã sản phẩm",
          field: "ProductCode",
          type: "text",
          width: 130,
          headerPos: "left",
          dataPos: "left",
          pin: true
        },
        {
          title: "Tên sản phẩm",
          field: "ProductName",
          type: "text",
          width: 150,
          headerPos: "left",
          dataPos: "left",
          wrap: true,
          pin: true
        },
        {
          title: " Hình ảnh",
          field: "Image",
          type: "image",
          width: 300,
          height: "100px"
        },
        {
          title: "Giá",
          field: "Price",
          type: "money",
          width: 100,
          headerPos: "right",
          dataPos: "right"
        },
        {
          title: "Số lượng khả dụng",
          field: "Quantity",
          type: "number",
          width: 100,
          headerPos: "right",
          dataPos: "right",
          total: true
        },
        {
          title: "Đã bán",
          field: "Sold",
          type: "number",
          width: 100,
          headerPos: "right",
          dataPos: "right",
          total: true
        },
        {
          title: "Tổng số lượng",
          field: "Quantity",
          type: "number",
          width: 100,
          headerPos: "right",
          dataPos: "right",
          total: true
        }, 
        {
          title: "Tổng tiền đã bán",
          field: "SoldTotalPrice",
          type: "money",
          width: 100,
          headerPos: "right",
          dataPos: "right",
          total: true
        },
        {
          title: "Tổng tiền",
          field: "TotalPrice",
          type: "money",
          width: 100,
          headerPos: "right",
          dataPos: "right",
          total: true
        },
        {
          title: "Mô tả",
          field: "Description",
          width: 500,
          type: "text"
        },
        {
          title: "Hot",
          field: "Hot",
          type: "text",
          dataPos: "center",
          width: 100
        }
      ],
      pagingInfo: {
        filterString: "",
        pageNum: 1,
        pageSize: 10,
        filterData: {
          TotalFields: ["Quantity", "TotalPrice", "Sold", "SoldTotalPrice"],
          Sorts: [{Field: "ProductCode", Desc: true}]
        }
      },
      productInfo: {},
    };
  },
  mounted() {
    this.getProductsFilterPaging();
  },
  methods: {
    /**
     * Đi đến trang chi tiết sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    productDetail(product) {
      this.$router.push(`/admin/product/product-detail/${product.ProductID}`);
    },
    /**
     * Lấy dữ liệu danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021)
     */
    getProducts() {
      ProductAPI.GetFilterPaging(
        this.pagingInfo.filterString,
        this.pagingInfo.pageNum,
        this.pagingInfo.pageSize,
        this.pagingInfo.filterData
      )
        .then(res => {
          if (res.data.Success) {
            this.listProduct = res.data.Data.Records;
            this.pagingInfo.totalPage = res.data.Data.TotalPage;
            this.pagingInfo.totalRecord = res.data.Data.TotalRecord; 
            this.pagingInfo.totalData = res.data.Data.TotalDatas;
          }
        })
        .catch(err => {
          console.log(err);
        });
    },
    /**
     * Tạo ra lọc cho danh sách sản phẩm
     * CreatedBy: NTDUNG (08/12/2021) */
    getProductsFilterPaging() {
      switch (this.currOption) {
        case 0:
          this.getProducts();
          break;
        case 1:
          this.pagingInfo.filterData.RangeDates = [
            {
              FieldName: "ModifiedDate",
              FromDate: new Date(2021, 11, 22),
              ToDate: new Date(2021, 11, 24)
            }
          ];
          this.getProducts();
          break;
        case 2:
          this.getProducts();
          break;
        case 3:
          this.getProducts();
          break;
      }
    },
    /**
     * Xoá sản phẩm
     * CreatedBy: NTDUNG (19/12/2021)
     */
    deleteProduct(product) {
      this.$dialog.confirm(
        `Bạn có muốn xoá <<b>${product.ProductCode} - ${product.ProductName}</b>> không`,
        {
          YES: () => {
            ProductAPI.Delete(product.ProductID)
              .then(res => {
                console.log(res);
                this.listCheck = [];
                this.getProductsFilterPaging();
                this.$toast.success("Xoá thành công");
              })
              .catch(err => {
                console.log(err);
              });
          },
          NO: () => {}
        }
      );
    },
    /**
     * Chỉnh sửa sản phẩm
     * CreatedBy: NTDUNG (19/12/2021)
     */
    showEditProduct(product) {
      ProductAPI.GetByID(product.ProductID)
        .then(res => {
          if (res.data.Success) {
            this.productInfo = res.data.Data;
            this.modePopup = true;
            this.showAddProduct = true;
          }
        })
        .catch(err => {
          this.$toast.error("Có lỗi xảy ra");
          console.log(err);
        });
    },
    /**
     * Thêm sản phẩm
     * CreatedBy: NTDUNG (20/12/2021)
     */
    addProduct() {
      this.productInfo.Sold = 0;
      ProductAPI.post(this.productInfo)
        .then(res => {
          console.log(res);
          this.$toast.success("Thêm mới sản phẩm thành công");
          this.showAddProduct = false;
          this.listCheck = [];
          this.getProductsFilterPaging();
        })
        .catch(err => {
          this.$toast.error(err.response.data.Msg);
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
      this.getProductsFilterPaging();
    },
    /**
     * Xoá nhiều
     * CreatedBy: NTDUNG (21/12/2021)
     */
    DeleteMulti() {
      this.$dialog.confirm("Bạn có muốn xoá tất cả những sản phẩm được chọn?", {
        YES: () => {
          ProductAPI.DeleteMulti(this.listCheck)
            .then(res => {
              this.$toast.success("Xoá thành công");
              this.listCheck = [];
              this.getProductsFilterPaging();
              console.log(res);
            })
            .catch(err => {
              this.$toast.error("Xoá thất bại");
              console.log(res);
            });
        }
      });
    },
    /**
     * Chỉnh sửa sản phẩm
     * CreatedBy: NTDUNG (23/12/2021)
     */
    editProduct() {
      ProductAPI.Update(this.productInfo.ProductID, this.productInfo)
        .then(res => {
          this.$toast.success("Chỉnh sửa sản phẩm thành công");
          this.showAddProduct = false;
          this.listCheck = [];
          this.getProductsFilterPaging();
        })
        .catch(err => {
          this.$toast.error(err.response.data.Msg);
        });
    },
    /**
     * Thay đổi danh sách được check
     * CreatedBy: NTDUNG (23/12/2021)
     */
    changeListCheck(newListCheck) {
      this.listCheck = newListCheck;
    }
  },
  watch: {
    currOption: function(value) {
      this.getProductsFilterPaging(value);
    }
  }
};
</script>
<style lang=""></style>
