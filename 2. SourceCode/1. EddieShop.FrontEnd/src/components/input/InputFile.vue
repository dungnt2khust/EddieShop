<template lang="">
  <div class="inputfile fx-col">
    <div class="w-full">
      <input
        ref="inputFile"
        type="file"
        v-on="inputListeners"
        v-validate="rules"
        class="inputfile__main d-none"
      />
      <div v-if="fileName" class="inputfile__preview flex-1 p-v-15">
        <img
          @click="showPreview = true"
          class="w-full h-full cur-p"
          :src="srcPreview"
          :alt="fileName"
        />
      </div>
      <ed-button
        :method="chooseFile"
        :label="fileName ? 'Đổi ảnh' : 'Chọn ảnh'"
        :styleBtn="0"
      />
      <vue-easy-lightbox
        :visible="showPreview"
        :imgs="[{ src: 'data:image/gif;base64,' + fileData, title: fileName }]"
        :index="0"
        @hide="handleHide"
      ></vue-easy-lightbox>
    </div>
    <div
      v-if="errMsg"
      class="input__error txt-reg-1 txt-s-12 m-t-10"
      style="color: red;"
    >
      {{ errMsg }}
    </div>
  </div>
</template>
<script>
export default {
  name: "InputFile",
  inject: ["parentValidator"],
  props: {
    rules: {
      type: [Object, String],
      default: () => {}
    },
    errMsg: {
      type: [Number, String],
      default: ""
    }
  },
  data() {
    return {
      files: [],
      srcPreview: "",
      fileName: "",
      reader: new FileReader(),
      showPreview: false,
      fileData: ""
    };
  },
  created() {
    this.$validator = this.parentValidator;
  },
  mounted() {
    this.reader.onloadend = () => {
      this.readerImage();
    };
  },
  computed: {
    /**
     * Lắng nghe sự kiện input
     * CreatedBy: NTDUNG (19/12/2021)
     */
    inputListeners() {
      return Object.assign({}, this.$listeners, {
        change: e => {
          var vm = this;
          this.files = e.target.files;
          this.fileName = this.files[0].name;

          // Tạo đường dẫn preview
          this.srcPreview = URL.createObjectURL(this.files[0]);
          // Timeout xoá đường dẫn
          setTimeout(() => {
            URL.revokeObjectURL(this.srcPreview);
          }, 100);

          // Convert sang bytearray và truyền ra
          this.reader.readAsArrayBuffer(this.files[0]);
        }
      });
    }
  },
  methods: {
    /**
     * Chuyển sang mảng ký tự
     * CreatedBy: NTDUNG (19/12/2021)
     */
    readerImage() {
      // var binaryString2 = btoa(String.fromCharCode(...(new Uint8Array(this.reader.result))))
      var binaryString = [],
        bytes = new Uint8Array(this.reader.result),
        length = bytes.length;
      for (var i = 0; i < length; i++) {
        binaryString[i] = String.fromCharCode(bytes[i]);
      }
      this.fileData = btoa(binaryString.join(""));
      this.$emit("input", this.fileData);
    },
    /**
     * Chọn file
     * CreatedBy: NTDUNG (20/12/2021)
     */
    chooseFile() {
      this.$refs.inputFile.click();
    },
    /**
     * Tắt preview
     * CreatedBy: NTDUNG (19/12/2021)
     */
    handleHide() {
      this.showPreview = false;
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/input/inputfile.scss";
</style>
