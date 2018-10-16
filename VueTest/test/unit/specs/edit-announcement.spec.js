import moxios from "moxios";
import EditAnnouncement from '@/components/edit-announcement';
import {mount} from "@vue/test-utils";
import announcement from "../schemas/announcement";
import {matchers} from 'jest-json-schema';

expect.extend(matchers);

describe('edit-announcement.vue', () => {
  let wrapper, pushCallback;
  const $route = {
    params: {},
  };
  beforeEach(function () {
    pushCallback = jest.fn();
    moxios.install();
    wrapper = mount(EditAnnouncement, {
      mocks: {
        $route,
        $router: {
          push: pushCallback
        }
      },
    });
  });

  afterEach(function () {
    moxios.uninstall();
  });
  test('не валидные данные', () => {
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {RealEstateObject: null, title: 'name'}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {RealEstateObject: {Id: 100, title: 'name'}}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {Price: -100, RealEstateObject: {Id: 100, title: 'name'}}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {Price: 10000, RealEstateObject: {Id: 100, title: 'name'}}});
    expect(wrapper.vm.isInvalid).toBeFalsy();
  });
  it('отправка нового объекта', (done) => {
    wrapper.setData({announcement: {Price: 10000, RealEstateObject: {Id: 100, title: 'name'}}});
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/odata/Announcement');
      expect(recent.config.method).toEqual('post');
      const json = recent.config.data;

      expect(json).toMatchSchema(announcement);
      recent.respondWith({
        status: 200,
        response: {}
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(1);
        expect(pushCallback.mock.calls[0][0]).toEqual({name: 'objectList'});
        done();
      });
    });
  });
  it('обновление объекта', (done) => {

    wrapper = mount(EditAnnouncement, {
      mocks: {
        $route: {params: {id: 23}},
        $router: {
          push: pushCallback
        }
      },
    });

    wrapper.setData({announcement: {Price: 10000, RealEstateObject: {Id: 100, title: 'name'}}});
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/odata/Announcement(23)');
      expect(recent.config.method).toEqual('put');
      const json = recent.config.data;

      expect(json).toMatchSchema(announcement);
      recent.respondWith({
        status: 200,
        response: {}
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(1);
        expect(pushCallback.mock.calls[0][0]).toEqual({name: 'objectList'});
        done();
      });
    });
  });
});
