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
        <EdListGridTable
          class="flex-1 w-full"
          :listData="listProduct"
          :dblClick="productDetail"
          :listHeader="listHeader"
          :pagingInfo="pagingInfo"
          @edit="editProduct"
          @delete="deleteProduct"
          @deleteMulti="deleteMulti"
          @changePagingFilter="changePagingFilter"
          v-model="listCheck"
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
        </EdListGridTable>

      </EdFrame>
      <AddProduct
        @addProduct="addProduct"
        @close="showAddProduct = false"
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
          pin: true
        },
        {
          title: " Hình ảnh",
          field: "Image",
          type: "image",
          width: 300,
          height: "100px",
          pin: true
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
          title: "Số lượng",
          field: "Quantity",
          type: "number",
          width: 100,
          headerPos: "right",
          dataPos: "right"
        },
        {
          title: "Tổng tiền",
          field: "TotalPrice",
          type: "money",
          width: 100,
          headerPos: "right",
          dataPos: "right"
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
        filterData: {}
      }
    };
  },
  mounted() {
    this.getProductsFilterPaging(this.currOption);
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
      ProductAPI.getFilterPaging(
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
            this.listCheck = []
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
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getProducts();
          break;
        case 1:
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"],
            RangeDates: [
              {
                FieldName: "ModifiedDate",
                FromDate: new Date(2021, 11, 22),
                ToDate: new Date(2021, 11, 24)
              }
            ]
          };
          this.getProducts();
          break;
        case 2:
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
          this.getProducts();
          break;
        case 3:
          this.pagingInfo.filterData = {
            TotalFields: ["Price", "OldPrice"]
          };
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
            ProductAPI.delete(product.ProductID)
              .then(res => {
                console.log(res);
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
    editProduct(product) {
      console.log(product);
    },
    /**
     * Thêm sản phẩm
     * CreatedBy: NTDUNG (20/12/2021)
     */
    addProduct(product) {
      ProductAPI.post(product)
        .then(res => {
          console.log(res);
          this.$toast.success("Thêm mới sản phẩm thành công");
          this.showAddProduct = false;
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
    deleteMulti() {
      this.$dialog.confirm("Bạn có muốn xoá tất cả những sản phẩm được chọn?", {
        YES: () => {
          ProductAPI.deleteMulti(this.listCheck)
            .then(res => {
              this.$toast.success("Xoá thành công");
              this.getProductsFilterPaging();
              console.log(res)
            })
            .catch(err => {
              this.$toast.error("Xoá thất bại");
              console.log(res);
            }) 
        }
      });
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
