import ObjectList from '@/components/object-list';
import {mount} from '@vue/test-utils';
import moxios from 'moxios';
import {
  count,
  realEstateTypes,
} from '../../../src/miscellaneous/real-estate-types';

describe('object-list.vue', () => {
  const $route = {
    params: {},
  };
  let wrapper;
  test('рендер', () => {
    expect(wrapper.find('h2').text()).toEqual('Параметры фильтрации');
  });
  test('если нет параметра маршрута нет выставленых галочек', () => {
    expect(wrapper.vm.$route.params.type).toBeUndefined();
    expect(wrapper.vm.types.length).toBe(0);
    expect(wrapper.findAll('input[type="checkbox"]').
      filter(w => w.element.checked).length).toBe(0);
  });
  test('установка параметра фильтрации при переходе по ссылке', () => {
    const wrp = mount(ObjectList, {
      mocks: {
        $route: {
          params: {type: 'apartment'},
        },
      },
    });
    expect(wrp.vm.$route.params.type).toBe('apartment');
    expect(wrp.find('input[value="Apartment"]').element.checked).toBeTruthy();
    expect(wrp.findAll('input[type="checkbox"]').
      filter(w => w.element.checked).length).toBe(1);
  });
  beforeEach(function() {
    moxios.install();
    wrapper = mount(ObjectList, {
      mocks: {
        $route,
      },
    });
  });

  afterEach(function() {
    moxios.uninstall();
  });
  test('запрос на сервер после создания компонента', done => {
    mount(ObjectList, {
      mocks: {
        $route: {params: {}},
      },
    });
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe('odata/Announcement?$expand=RealEstateObject');
      done();
    });
  });
  test('устанавливаем колиство комнат', done => {

    wrapper.find('#room2').setChecked(true);
    wrapper.vm.refresh();
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).
        toBe(
          'odata/Announcement?$expand=RealEstateObject&$filter=(RealEstateObject/Rooms eq 2)');
      done();
    });
  });
  test('выбрали все комнаты', done => {
    wrapper.find('#room1').setChecked(true);
    wrapper.find('#room2').setChecked(true);
    wrapper.find('#room3').setChecked(true);
    wrapper.find('#room4').setChecked(true);
    wrapper.vm.refresh();
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe('odata/Announcement?$expand=RealEstateObject');
      done();
    });
  });
  test('устанавливаем тип недвижимости', done => {
    //Сброс всех галочек
    Object.keys(realEstateTypes).forEach(key => {
      wrapper.find('#' + key).setChecked(false);
    });
    wrapper.find('#Apartment').setChecked(true);
    wrapper.find('#House').setChecked(true);
    wrapper.vm.refresh();
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).
        toBe(
          'odata/Announcement?$expand=RealEstateObject&$filter=(RealEstateObject/RealEstateType eq \'Apartment\' or RealEstateObject/RealEstateType eq \'House\')');
      done();
    });

  });
  test('не валидные значения цена "До" < "От"', () => {
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.find('#priceTo').setValue(30);
    wrapper.find('#priceFrom').setValue(50);
    expect(wrapper.vm.isInvalid).toBeTruthy();
  });
  test('указали цена "От"', (done) => {
    wrapper.find('#priceFrom').setValue(50);
    wrapper.vm.refresh();
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).
        toBe(
          'odata/Announcement?$expand=RealEstateObject&$filter=(Price ge 50)');
      done();
    });
  });
  test('указали цена "До"', (done) => {
    wrapper.find('#priceTo').setValue(40);
    wrapper.vm.refresh();
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).
        toBe(
          'odata/Announcement?$expand=RealEstateObject&$filter=(Price le 40)');
      done();
    });
  });
});
