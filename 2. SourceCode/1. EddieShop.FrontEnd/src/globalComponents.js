import Vue from 'vue'

import BaseContentArea from '@/layouts/basecontentarea/BaseContentArea.vue'
Vue.component('BaseContentArea', BaseContentArea);

import FullScreen from '@/layouts/FullScreen.vue'
Vue.component('FullScreen', FullScreen);

import Logo from '@/layouts/logo/Logo.vue'
Vue.component('EdLogo', Logo);

import GridProduct from '@/layouts/gridproduct/GridProduct.vue'
Vue.component('EdGridProduct', GridProduct);

import ListProduct from '@/layouts/listproduct/ListProduct.vue'
Vue.component('EdListProduct', ListProduct);

const BaseContentFrame = () => import('@/layouts/basecontentframe/BaseContentFrame.vue');
Vue.component('BaseContentFrame', BaseContentFrame);

const Col = () => import('@/layouts/col/Col.vue');
Vue.component('EdCol', Col);

const Row = () => import('@/layouts/row/Row.vue');
Vue.component('EdRow', Row);

const Frame = () => import('@/layouts/frame/Frame.vue');
Vue.component('EdFrame', Frame);

const Popup = () => import('@/components/popup/Popup.vue');
Vue.component('EdPopup', Popup);

const Label = () => import('@/components/label/Label.vue');
Vue.component('EdLabel', Label);

const Input = () => import('@/components/input/Input.vue');
Vue.component('EdInput', Input);

const InputFile = () => import('@/components/input/InputFile.vue');
Vue.component('EdInputFile', InputFile);

const Textarea = () => import('@/components/textarea/Textarea.vue');
Vue.component('EdTextarea', Textarea);

const Button = () => import('@/components/button/Button.vue');
Vue.component('EdButton', Button);

const Icon = () => import('@/components/icon/Icon.vue');
Vue.component('EdIcon', Icon);

const INumber = () => import('@/components/number/Number.vue');
Vue.component('EdNumber', INumber);

const SelectBox = () => import('@/components/selectbox/selectbox.vue')
Vue.component('EdSelectBox', SelectBox);

const ListGrid = () => import('@/components/listgrid/ListGrid.vue');
Vue.component('EdListGrid', ListGrid);

const ListGridTable = () => import('@/components/listgrid/ListGridTable.vue');
Vue.component('EdListGridTable', ListGridTable);

const CheckBox = () => import('@/components/checkbox/CheckBox.vue');
Vue.component('EdCheckBox', CheckBox);

const Radio = () => import('@/components/radio/Radio.vue');
Vue.component('EdRadio', Radio);

const Dialog = () => import('@/components/dialog/Dialog.vue');
Vue.component('EdDialog', Dialog);

const Datepicker = () => import('@/components/datepicker/Datepicker.vue');
Vue.component('EdDatepicker', Datepicker);

const Toast = () => import('@/components/toast/Toast.vue');
Vue.component('EdToast', Toast);

const Search = () => import('@/components/search/Search.vue');
Vue.component('EdSearch', Search);

const Pagination = () => import('@/components/pagination/Pagination.vue');
Vue.component('EdPagination', Pagination);

const NoContent = () => import('@/components/nocontent/NoContent.vue');
Vue.component('NoContent', NoContent);
