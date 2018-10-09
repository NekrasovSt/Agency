import EditObject from '@/components/edit-object';
import {mount} from '@vue/test-utils';
import moxios from 'moxios';

describe('edit-object.vue', () => {
  const wrapper = mount(EditObject, {
    mocks: {
      $route: {params: {}},
    },
  });
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
  beforeEach(function() {
    moxios.install();
  });
  afterEach(function() {
    moxios.uninstall();
  });
  it('если ид нет в параметрах то запрос на сервер не отправляется', (done) => {
    mount(EditObject, {
      mocks: {
        $route: {params: {}},
      },
    });
    setTimeout(() => {
      expect(moxios.requests.count()).toEqual(0);
      done();
    });
  });
  it('есть ид в параметрах запрос на сервер отправляется', (done) => {
    moxios.stubRequest(`odata/RealEstateObject(1)`, {
      status: 200,
      responseText: {
        Code: '000001',
        Building: '5',
      },
    });
    moxios.stubRequest(`api/Address/GetParents?code=000001&building=5`, {
      status: 200,
      responseText:
        {
          id: '5900000100001720001',
          name: '5',
          zip: 614026,
          type: 'дом',
          typeShort: 'д',
          okato: '57401378000',
          contentType: 'building',
          parents: [
            {
              id: '5900000000000',
              name: 'Пермский',
              zip: 614575,
              type: 'Край',
              typeShort: 'край',
              okato: '57000000000',
              contentType: 'region',
            },
            {
              id: '5900000100000',
              name: 'Пермь',
              zip: 614025,
              type: 'Город',
              typeShort: 'г',
              okato: '57401000000',
              contentType: 'city',
            },
            {
              id: '59000001000017200',
              name: 'Героя Васькина',
              zip: 614026,
              type: 'Улица',
              typeShort: 'ул',
              okato: '57401378000',
              contentType: 'street',
            },
          ],
        },
    });
    mount(EditObject, {
      mocks: {
        $route: {params: {id: 1}},
      },
    });
    moxios.wait(() => {
      expect(moxios.requests.at(0).url).toEqual('odata/RealEstateObject(1)');
      expect(moxios.requests.at(1).url).
        toEqual('api/Address/GetParents?code=000001&building=5');
      expect(moxios.requests.count()).toEqual(2);
      done();
    });
  });
  it('Устанавливаем файл, причем два одинаковых', () => {
    const event = [
      new File(['foo'], 'test1.txt'),
      new File(['foo2'], 'test2.txt'),
      new File(['foo'], 'test1.txt')];

    const wrp = mount(EditObject, {
      mocks: {
        $route: {params: {}},
      },
    });
    expect(wrp.vm.images.length).toEqual(0);
    // const input = wrapper.find('input[type="file"]');
    wrp.vm.handleFilesUpload({files: event});
    console.log(window.URL.createObjectURL);
    expect(wrp.vm.images.length).toEqual(2);
  });
});
