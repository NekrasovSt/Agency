import Vue from 'vue'
import App from './App'
import router from './router'
import vSelect from 'vue-select'
import './styles.css'
import dateFilter from './filters/date'
import estateType from './filters/estate-type'

Vue.component('v-select', vSelect);
Vue.use(dateFilter);
Vue.use(estateType);
Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: {App},
  template: '<App/>'
});
