import Vue from 'vue'
import App from './App'
import router from './router'
import vSelect from 'vue-select'
import './styles.css'
import dateFilter from './filters/date'
import estateType from './filters/estate-type'
import VueUploadComponent from 'vue-upload-component'

Vue.component('v-select', vSelect);
Vue.use(dateFilter);
Vue.use(estateType);
Vue.component('file-upload', VueUploadComponent);

Vue.config.productionTip = false;

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  components: {App},
  template: '<App/>'
});
