<template lang="">
  <div
    :class="[
      'col',
      colLg || colW ? 'col-lg-' + (colLg || colW) : '',
      colXl || colW ? 'col-xl-' + (colXl || colW) : '',
      colXs || colW ? 'col-xs-' + (colXs || colW) : '',
      colSm || colW ? 'col-sm-' + (colSm || colW) : '',
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
  name: "Col",
  props: {
    colW: {
      type: [Number, String],
      default: 12
    },
    colXs: {
      type: [Number, String],
      default: 0
    },
    colXl: {
      type: [Number, String],
      default: 0
    },
    colSm: {
      type: [Number, String],
      default: 0
    },
    colLg: {
      type: [Number, String],
      default: 0
    },
    orderXs: {
      type: [Number, String],
      default: 0
    },
    orderXl: {
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
@mixin setCol($name) {
  @for $col from 1 to 13 {
    $flexBasis: percentage(1 / 12 * $col);
    .#{"col-" + $name + "-" + $col} {
        flex-basis: $flexBasis !important;
    }
  }
}
@include setCol(w);

@media (max-width: 739px) {
  @include setCol(sm);
}
/* >= Tablet */
@media (min-width: 740px) {
}
/* PC medium resolution > */
@media (min-width: 1113px) {
}
/* Tablet - PC low resolution */
@media (min-width: 740px) and (max-width: 1023px) {
  @include setCol(xs);
}
/* > PC low resolution */
@media (min-width: 1024px) and (max-width: 1239px) {
  @include setCol(xl);
}
/* PC height resolution */
@media (min-width: 1240px) {
   @include setCol(lg);
}
</style>
