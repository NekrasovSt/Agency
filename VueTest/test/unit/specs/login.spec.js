import {mount} from '@vue/test-utils';
import moxios from 'moxios';
import Login from '@/components/login';
import loginSchema from "../schemas/login";
import {matchers} from 'jest-json-schema';
import realEstateTypeSchema from "../schemas/realEstateType";

expect.extend(matchers);


describe('login.vue', () => {
  let wrapper, pushCallback;

  beforeEach(function () {
    moxios.install();
    pushCallback = jest.fn();
    wrapper = mount(Login, {
      mocks: {
        $router: {
          push: pushCallback
        }
      },
    });
  });

  afterEach(function () {
    moxios.uninstall();
  });

  it('валидность логина', () => {
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({login: '', password: ''});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({login: 'Name', password: ''});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({login: '', password: 'Pasw'});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({login: 'Name', password: 'Pasw'});
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.setData({login: '0', password: '0'});
    expect(wrapper.vm.isInvalid).toBeFalsy();
  });
  it('логин ошибка', (done) => {
    wrapper.setData({login: 'Name', password: 'Pasw'});
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/Token');
      expect(recent.config.method).toEqual('post');
      const json = JSON.parse(recent.config.data);

      expect(json).toMatchSchema(loginSchema);
      recent.respondWith({
        status: 400,
        response: {}
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(0);
        done();
      });
    });
  });
  it('логин ок', (done) => {
    wrapper.setData({login: 'Name', password: 'Pasw'});
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/Token');
      expect(recent.config.method).toEqual('post');
      const json = JSON.parse(recent.config.data);

      expect(json).toMatchSchema(loginSchema);
      recent.respondWith({
        status: 200,
        response: 'some token'
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(1);
        expect(pushCallback.mock.calls[0][0]).toEqual({name: 'objectList'});
        done();
      });
    });
  });
});
