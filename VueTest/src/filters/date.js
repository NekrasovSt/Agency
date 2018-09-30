import moment from 'moment'

export default {
  install(Vue) {
    Vue.filter('format', function (value) {
      return moment(value).format("HH:mm:ss DD.MM.YYYY");
    });
  }
}

