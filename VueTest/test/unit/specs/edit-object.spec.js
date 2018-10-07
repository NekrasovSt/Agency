import EditObject from '@/components/edit-object';
import {mount} from '@vue/test-utils';

describe('edit-object.vue', () => {
  const wrapper = mount(EditObject);
  it('все селекты кроме области не доступны в начале', () => {
    expect(wrapper.vm.regionSelected).toBeFalsy();
    expect(wrapper.vm.citySelected).toBeFalsy();
    expect(wrapper.vm.streetSelected).toBeFalsy();
    expect(wrapper.vm.buildingSelected).toBeFalsy();
  });
  it('выбран регион', () => {
    wrapper.setData({currentRegion: {id: '000001', name: ''}});
    expect(wrapper.vm.regionSelected).toBeTruthy();
  });
  it('выбран город', () => {
    wrapper.setData({currentCity: {id: '000001', name: ''}});
    expect(wrapper.vm.citySelected).toBeTruthy();
  });
  it('выбрана улица', () => {
    wrapper.setData({currentStreet: {id: '000001', name: ''}});
    expect(wrapper.vm.streetSelected).toBeTruthy();
  });
  it('выбрано здание', () => {
    wrapper.setData({currentBuilding: {id: '000001', name: ''}});
    expect(wrapper.vm.buildingSelected).toBeTruthy();
  });
  it('рендер', () => {
    expect(wrapper.find('h1').text()).toEqual('Заполните форму');
  });
});
