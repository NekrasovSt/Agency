import moxios from "moxios";
import EditAnnouncement from '@/components/edit-announcement';
import {mount} from "@vue/test-utils";


describe('edit-announcement.vue', () => {
  let wrapper;
  const $route = {
    params: {},
  };
  beforeEach(function () {
    moxios.install();
    wrapper = mount(EditAnnouncement, {
      mocks: {
        $route,
      },
    });
  });

  afterEach(function () {
    moxios.uninstall();
  });
  test('не валидные данные', () => {
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {RealEstateObject: null}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {RealEstateObject: {Id: 100}}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {Price: -100, RealEstateObject: {Id: 100}}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({announcement: {Price: 10000, RealEstateObject: {Id: 100}}});
    expect(wrapper.vm.isInvalid).toBeFalsy();
  });
});
