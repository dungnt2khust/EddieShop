<template lang="">
  <div class="textarea">
    <textarea
      :rows="row"
      :cols="col"
      :style="customizeStyle(styleCustom)"
      v-on="textareaListeners"
      :value="value"
      :class="{'input--invalid': errorMessage != ''}"
      :title="errorMessage"
      @blur="handleOnBlur"
    />
  </div>
</template>
<script>
export default {
  name: "Textarea",
  props: {
    col: {
      type: Number,
      default: 1
    },
    row: {
      type: Number,
      default: 5
    },
    required: {
      type: Boolean,
      default: false
    },
    resize: {
      type: String,
      default: "none"
    },
    width: {
        type: String,
        default: null
    },
    height: {
        type: String,
        default: null
    },
    value: {
      type: [Number, String],
      default: null
    }
  },
  data() {
    return {
      styleCustom: {
        "resize": this.resize,
        "width": this.width,
        "height": this.height
      },
      errorMessage: ''
    };
  },
  computed: {
    /**
     * Lắng nghe textarea
     * CreatedBy: NTDUNG (24/11/2021)
     */
    textareaListeners() {
      return Object.assign({}, this.listeners, {
        input: (event) => {
          this.$emit('input', event.target.value);
        },
        focus: () => {
          this.errorMessage = "";
        }
      })
    }
  },
  methods: {
    handleOnBlur() {
      if (this.required && !this.value) {
        this.errorMessage = "This field is required";
      }
    }
  }
};
</script>
<style lang="scss">
@import "@/assets/scss/components/textarea/textarea.scss";
</style>
