import Vue from 'vue'
import Router from 'vue-router'
import EditObject from '@/components/edit-object.vue'
import MainPage from '@/components/main-page.vue'
import Mortgage from '@/components/mortgage.vue'
import Certificate from '@/components/certificate.vue'
import Support from '@/components/support.vue'
import ObjectList from '@/components/object-list.vue'

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/edit-object',
      name: 'editObject',
      component: EditObject
    },
    {
      path: '/object-list/:type',
      name: 'objectList',
      component: ObjectList
    },
    {
      path: '/',
      name: 'main',
      component: MainPage
    },
    {
      path: '/mortgage',
      name: 'mortgage',
      component: Mortgage
    }
    ,
    {
      path: '/certificate',
      name: 'certificate',
      component: Certificate
    }
    ,
    {
      path: '/support',
      name: 'support',
      component: Support
    }
  ]
})
