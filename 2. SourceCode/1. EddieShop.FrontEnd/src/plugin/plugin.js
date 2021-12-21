import Vue from 'vue'

// Components
import FlagIcon from 'vue-flag-icon'
import Popup from './common/popup.js'
import Tooltip from './common/tooltip.js'
import Notify from './common/notify.js'
import Dialog from './common/dialog.js'
import Toast from './common/toast.js'
import Loading from './common/loading.js'
import VTooltip from 'v-tooltip'
import ClickOutside from 'vue-click-outside'
import VueEasyLightbox from 'vue-easy-lightbox'
import Paginate from 'vuejs-paginate'
import VueColumnsResizable from 'vue-columns-resizable'
// Plugins
import EventBus from '@/bus/eventbusGlobal.js'
import VeeValidate from 'vee-validate'
// Account Auth
import AccountAPI from '@/api/components/Account/AccountAPI.js'

Vue.use(FlagIcon);
Vue.use(VTooltip);
Vue.use(VueEasyLightbox);
Vue.use(VeeValidate);
Vue.use(VueColumnsResizable);


Vue.prototype.$bus = EventBus;
Vue.prototype.$popup = Popup;
Vue.prototype.$tooltip = Tooltip;
Vue.prototype.$notify = Notify;
Vue.prototype.$loading = Loading;
Vue.prototype.$account = AccountAPI;
Vue.prototype.$dialog = Dialog;
Vue.prototype.$toast = Toast;
Vue.prototype.$appInfo = {
    appName: "EddieShop"
}
// Directive
Vue.directive("ClickOutside", ClickOutside);
// Chart
import VueApexCharts from 'vue-apexcharts'
Vue.use(VueApexCharts)
Vue.component('apexchart', VueApexCharts)

const BarChart = () => import('@/components/chart/BarChart.vue');
Vue.component('EdBarChart', BarChart);

const LineChart = () => import('@/components/chart/LineChart.vue');
Vue.component('EdLineChart', LineChart);

const AreaChart = () => import('@/components/chart/AreaChart.vue');
Vue.component('EdAreaChart', AreaChart);

const RadarChart = () => import('@/components/chart/RadarChart.vue');
Vue.component('EdRadarChart', RadarChart);

// Pagination
Vue.component("paginate", Paginate);

export default Vue;