<template lang="">
  <div
    :class="[
      'row fx-wrap',
      rowLg || rowW ? 'row-lg-' + (rowLg || rowW) : '',
      rowXl || rowW ? 'row-xl-' + (rowXl || rowW) : '',
      rowXs || rowW ? 'row-xs-' + (rowXs || rowW) : '',
      rowSm || rowW ? 'row-sm-' + (rowSm || rowW) : '',
      orderLg ? 'order-lg-' + orderLg : '',
      orderXl ? 'order-xl-' + orderXl : '',
      orderXs ? 'order-xs-' + orderXs : '',
      orderSm ? 'order-sm-' + orderSm : '', 
    ]"
    :style="customizeStyle(styleCustom)"
  >
    <slot></slot>
  </div>
</template>
<script>
export default {
  name: "row",
  props: {
    rowW: {
      type: [Number, String],
      default: 12
    },
    rowXs: {
      type: [Number, String],
      default: 0
    },
    rowSm: {
      type: [Number, String],
      default: 0
    },
    rowLg: {
      type: [Number, String],
      default: 0
    },
    rowXl: {
      type: [Number, String],
      default: 0
    },
    orderXs: {
      type: [Number, String],
      default: 0
    },
    orderSm: {
      type: [Number, String],
      default: 0
    },
    orderLg: {
      type: [Number, String],
      default: 0
    },
    orderXl: {
      type: [Number, String],
      default: 0
    }, 
    height: {
      type: [Number, String],
      default: null
    }
  },
  data() {
    return {
      styleCustom: {
        height: this.height
      }
    };
  }
};
</script>
<style lang="scss">
@mixin setrow($name) {
  @for $row from 1 to 13 {
    $flexBasis: percentage(1 / 12 * $row);
    .#{"row-" + $name + "-" + $row} {
      flex-basis: $flexBasis !important;
    }
  }
}
@include setrow(w);
@media (max-width: 739px) {
  @include setrow(sm);
}
/* >= Tablet */
@media (min-width: 740px) {
}
/* PC medium resolution > */
@media (min-width: 1113px) {
}
/* Tablet - PC low resolution */
@media (min-width: 740px) and (max-width: 1023px) {
  @include setrow(xs);
}
/* > PC low resolution */
@media (min-width: 1024px) and (max-width: 1239px) {
  @include setrow(xl);
}
/* PC height resolution */
@media (min-width: 1240px) {
  @include setrow(lg);
}
</style>
