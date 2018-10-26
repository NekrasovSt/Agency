import Vue from 'vue';
import Router from 'vue-router';
import EditObject from '@/components/edit-object.vue';
import MainPage from '@/components/main-page.vue';
import Mortgage from '@/components/mortgage.vue';
import Certificate from '@/components/certificate.vue';
import Support from '@/components/support.vue';
import ObjectList from '@/components/object-list.vue';
import Announcement from '@/components/announcement.vue';
import EditAnnouncement from '@/components/edit-announcement.vue';
import Login from '@/components/login.vue';
import auth from '@/miscellaneous/auth'
import RealEstateObjectList from '@/components/real-estate-object-list.vue';

Vue.use(Router);

const router = new Router({
  routes: [
    {
      path: '/',
      redirect: '/main'
    },
    {
      path: '/edit-object/:id?',
      name: 'editObject',
      component: EditObject,
      meta: {requiresAuth: true}
    },
    {
      path: '/object-list/:type?',
      name: 'objectList',
      component: ObjectList,
    },
    {
      path: '/main',
      name: 'main',
      component: MainPage,
    },
    {
      path: '/mortgage',
      name: 'mortgage',
      component: Mortgage,
    }
    ,
    {
      path: '/certificate',
      name: 'certificate',
      component: Certificate,
    }
    ,
    {
      path: '/support',
      name: 'support',
      component: Support,
    },
    {
      path: '/announcement/:id',
      name: 'announcement',
      component: Announcement,
    },
    {
      path: '/edit-announcement/:id?',
      name: 'edit-announcement',
      component: EditAnnouncement,
      meta: {requiresAuth: true}
    },
    {
      path: '/login',
      name: 'login',
      component: Login,
    },
    {
      path: '/real-estate-object-list',
      name: 'real-estate-object-list',
      component: RealEstateObjectList,
      meta: {requiresAuth: true}
    }
  ],
});
router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth) { // check the meta field
    if (auth.isAuthenticated) { // check if the user is authenticated
      next() // the next method allow the user to continue to the router
    }
    else {
      next('/') // Redirect the user to the main page
    }
  }
  else {
    next();
  }
});
export default router;
