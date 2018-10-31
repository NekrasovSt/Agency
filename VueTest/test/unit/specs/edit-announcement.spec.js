import moxios from "moxios";
import EditAnnouncement from '@/components/edit-announcement';
import {mount, shallowMount} from "@vue/test-utils";
import announcement from "../schemas/announcement";
import {matchers} from 'jest-json-schema';

expect.extend(matchers);

describe('edit-announcement.vue', () => {
  let wrapper, pushCallback;
  const $route = {
    params: {},
    query: {}
  };
  beforeEach(function () {
    pushCallback = jest.fn();
    moxios.install();
    wrapper = shallowMount(EditAnnouncement, {
      mocks: {
        $route,
        $router: {
          push: pushCallback
        }
      },
      stubs: ['router-link']
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

    wrapper = shallowMount(EditAnnouncement, {
      mocks: {
        $route: {params: {id: 23}, query: {}},
        $router: {
          push: pushCallback
        }
      },
      stubs: ['router-link']
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

  it('загрузка на редактирование', done => {
    wrapper = shallowMount(EditAnnouncement, {
      mocks: {
        $route: {
          params: {id: 10},
          query: {}
        },
        $router: {
          push: pushCallback
        }
      },
      stubs: ['router-link']
    });
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/odata/Announcement(10)?$expand=RealEstateObject');
      expect(recent.config.method).toEqual('get');
      const json = recent.config.data;
      recent.respondWith({
        status: 200,
        response: {Id: 10, Description: 'Bla bla', RealEstateObject: {}}
      }).then(() => {
        expect(wrapper.vm.announcement).toBeDefined();
        expect(wrapper.vm.announcement.RealEstateObject).toBeDefined();
        expect(wrapper.vm.announcement.RealEstateObject.title).toBeDefined();
        done();
      });
    });
  });
  it('создание на базе объекта, проверка загрузки', done => {
    wrapper = shallowMount(EditAnnouncement, {
      mocks: {
        $route: {
          query: {basedOn: 99},
          params: {}
        }
      },
      stubs: ['router-link']
    });
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/odata/RealEstateObject(99)');
      expect(recent.config.method).toEqual('get');
      const json = recent.config.data;
      recent.respondWith({
        status: 200,
        response: {Id: 10, Description: 'Bla bla'}
      }).then(() => {
        expect(wrapper.vm.announcement).toBeDefined();
        expect(wrapper.vm.announcement.RealEstateObject).toBeDefined();
        expect(wrapper.vm.announcement.RealEstateObject.title).toBeDefined();
        done();
      });
    });
  });
});
