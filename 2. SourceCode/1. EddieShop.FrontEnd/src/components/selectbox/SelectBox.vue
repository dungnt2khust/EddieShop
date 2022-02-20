<template lang="">
  <div class="selectbox h-full" :style="customizeStyle(styleWrapperCustom)">
    <div
      @click="toggleSelectBox"
      v-click-outside="
        () => {
          selectBoxState = false;
        }
      "
      class="selectbox__curr p-h-8 w-full"
      ref="selectbox"
      :style="{ width: width, height: height }"
    >
      <table class="w-full">
        <tbody>
          <tr>
            <td v-for="(field, idx) in listField" :key="idx">
              <div
                v-if="field.type != 'flag'"
                class="fx-row"
                v-html="itemData(options[value], field)"
                :style="posData(field.pos)"
              ></div>
              <div v-else class="fx-row">
                <flag
                  :iso="options[value][field.field]"
                  :class="`scale-${field.scale}`"
                />
              </div>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
    <table
      v-if="selectBoxState"
      class="selectbox__list pos-fixed"
      :style="[
        customizeStyle(styleCustom),
        positionSelectBox($refs.selectbox.getBoundingClientRect())
      ]"
    >
      <tbody>
        <tr
          v-for="(option, index) in options"
          @click="handleChooseOption(index)"
          class="selectbox__item"
          :class="{ selected: index == value }"
          :key="index"
        >
          <td v-for="(field, idx) in listField" class="p-h-8 p-v-4" :key="idx">
            <div
              v-if="field.type != 'flag'"
              class="fx-row"
              v-html="itemData(option, field)"
              :style="posData(field.pos)"
            ></div>
            <div v-else class="fx-row">
              <flag
                :iso="option[field.field]"
                :class="`scale-${field.scale}`"
              />
            </div>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
<script>
export default {
  name: "SelectBox",
  props: {
    disable: {
      type: Boolean,
      default: false
    },
    value: {
      type: [Number, String],
      default: null
    },
    options: {
      type: Array,
      default: () => []
    },
    listStyle: {
      type: String,
      default: null
    },
    width: {
      type: String,
      default: null
    },
    bgColor: {
      type: String,
      default: "#fff"
    },
    up: {
      type: Boolean,
      default: false
    },
    height: {
      type: String,
      default: null
    },
    listField: {
      type: Array,
      default: () => []
    },
    default: {
      type: [Number, String],
      default: null
    }
  },
  data() {
    return {
      styleCustom: {
        "list-style": this.listStyle,
        width: this.width,
        "background-color": this.bgColor
      },
      styleWrapperCustom: {
        "background-color": this.bgColor
      },
      selectBoxState: false,
      distance: 4
    };
  },
  created() {
    this.$bus.$on("hideOnGlobalEvent", () => {
      this.selectBoxState = false;
    });
  },
  methods: {
    itemData(option, field) {
      var item;
      switch (field.type) {
        case "text":
          item = option[field.field];
          break;
        case "icon":
          item = `<div class="${option[field.field]} scale-${
            field.scale
          }"></div>`;
          break;
        default:
          item = option[field.field];
          break;
      }
      return item;
    },
    /**
     * Thông tin toạ độ, kích thước
     * CreatedBy: NTDUNG (24/12/2021)
     */
    positionSelectBox(boundingClientRect) {
      var style = {};
      if (this.up) {
        style.transform = "translateY(-100%)";
        style.top = `${boundingClientRect.bottom -
          boundingClientRect.height -
          this.distance}px`;
      } else {
        style.top = `${boundingClientRect.bottom + this.distance}px`;
      }

      style.bottom = "unset";
      style.left = boundingClientRect.left + "px";
      style.width = boundingClientRect.width + "px";
      return style;
    },
    /**
     * Bật tắt selectbox
     * CreatedBy: NTDUNG (08/12/2021)
     */
    toggleSelectBox() {
      if (!this.disable)
      this.selectBoxState = !this.selectBoxState;
    },
    /**
     * Xử lý chọn lựa chọn mới
     * CreatedBy: NTDUNG (08/12/2021)
     */
    handleChooseOption(index) {
      if (this.value != index) {
        this.$emit("input", index);
        this.$emit("changeOption", index);
      }
      this.selectBoxState = false;
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
    }
  },
  destroyed() {
    this.$bus.$off("hideOnGlobalEvent");
  },
  watch: {
    default: {
      immediate: true,
      handler(val) {
        if (val !== undefined && val !== null)
          this.$emit('input', val);
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/selectbox/selectbox.scss";
</style>
