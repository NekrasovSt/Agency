import {getNameFromType} from '../miscellaneous/real-estate-types';

export default {
  install(Vue) {
    Vue.filter('estate-type', function(value) {
      return getNameFromType(value);
    });
  },
};
