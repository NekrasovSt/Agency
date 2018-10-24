import moxios from "moxios";
import App from '@/App';
import {shallowMount} from "@vue/test-utils";
import auth from '@/miscellaneous/auth'
import axios from 'axios';
import VueRouter from 'vue-router'
import Vue from 'vue'
import router from '@/router'

Vue.use(VueRouter);



describe('app.vue', () => {
  let wrapper, pushCallback;
  beforeEach(function () {
    pushCallback = jest.fn();
    moxios.install();
    wrapper = shallowMount(App, {
      router,
      mocks: {
        // $router: {
        //   push: pushCallback
        // }
      },
    });
    wrapper.vm.$router.push = pushCallback;
  });


  afterEach(function () {
    moxios.uninstall();
    wrapper.destroy();
  });
  it('add Bearer token', (done)=>{
    auth.accessToken = 'token';
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.config.headers.Authorization).toEqual('Bearer token');
      done();
    });
    axios.get('/test 1');
  });
  it('401 перенаправляем на логин', (done)=>{
    auth.accessToken = 'token';
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      recent.respondWith({
        status: 401,
        response: {}
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(1);
        expect(pushCallback.mock.calls[0][0]).toEqual({name: 'login'});
        done();
      });
    });
    axios.get('/test 2').catch(()=>{

    });
  });
});
