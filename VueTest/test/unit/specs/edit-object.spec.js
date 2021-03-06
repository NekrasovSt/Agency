import EditObject from '@/components/edit-object';
import {mount, shallowMount} from '@vue/test-utils';
import moxios from 'moxios';
import {matchers} from 'jest-json-schema';
import realEstateTypeSchema from '../schemas/realEstateType';

expect.extend(matchers);

describe('edit-object.vue', () => {
  let wrapper, pushCallback;
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
  beforeEach(function () {
    moxios.install();
    URL.createObjectURL = function (temp) {

    };
    pushCallback = jest.fn();
    wrapper = shallowMount(EditObject, {
      mocks: {
        $route: {
          params: {},
          query: {}
        },
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
  it('если ид нет в параметрах то запрос на сервер не отправляется', (done) => {
    shallowMount(EditObject, {
      mocks: {
        $route: {params: {}},
      },
      stubs: ['router-link']
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
    shallowMount(EditObject, {
      mocks: {
        $route: {params: {id: 1}},
      },
      stubs: ['router-link']
    });
    moxios.wait(() => {
      expect(moxios.requests.at(0).url).toEqual('odata/RealEstateObject(1)');
      expect(moxios.requests.at(1).url).toEqual('api/Address/GetParents?code=000001&building=5');
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
        $route: {params: {}}
      },
    });
    expect(wrp.vm.images.length).toEqual(0);
    wrp.vm.handleFilesUpload({files: event});
    expect(wrp.vm.images.length).toEqual(2);
  });
  it('удаляем файл', () => {
    const event = [
      new File(['foo'], 'test1.txt'),
      new File(['foo2'], 'test2.txt')];
    wrapper.vm.images = event;
    expect(wrapper.vm.images.length).toEqual(2);
    wrapper.vm.remove(new File(['foo2'], 'test2.txt'));
    expect(wrapper.vm.images.length).toEqual(1);
  });
  it('форма не валидна, пока все не заполнено', () => {
    wrapper.vm.blockWatch = true;
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({currentRegion: {id: '000001', name: ''}});
    wrapper.setData({currentCity: {id: '000001', name: ''}});
    wrapper.setData({currentStreet: {id: '000001', name: ''}});
    wrapper.setData({currentBuilding: {id: '000001', name: ''}});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({floor: '2/5'});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({square: 23.5});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({rooms: 3});
    expect(wrapper.vm.isInvalid).toBeFalsy();
  });
  it('форма не валидна, не верный формат этажей', () => {
    wrapper.vm.blockWatch = true;
    wrapper.setData({currentRegion: {id: '000001', name: ''}});
    wrapper.setData({currentCity: {id: '000001', name: ''}});
    wrapper.setData({currentStreet: {id: '000001', name: ''}});
    wrapper.setData({currentBuilding: {id: '000001', name: ''}});
    wrapper.setData({floor: '2/5'});
    wrapper.setData({square: 23.5});
    wrapper.setData({rooms: 3});
    expect(wrapper.vm.isInvalid).toBeFalsy();

    wrapper.setData({floor: '2/'});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({floor: '/5'});
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.setData({floor: '/'});
    expect(wrapper.vm.isInvalid).toBeTruthy();
  });
  it('отправка нового объекта', (done) => {
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      floor: '2/5',
      square: 23.5,
      rooms: 3,
    });
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/odata/RealEstateObject');
      expect(recent.config.method).toEqual('post');
      const documentBlob = recent.config.data.getAll('document')[0];
      const json = JSON.parse(String.fromCharCode.apply(null,
        new Uint16Array(documentBlob._buffer)));

      expect(json).toMatchSchema(realEstateTypeSchema);
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
  it('отправка нового объекта - переход с нового объявления', (done) => {
    pushCallback = jest.fn();
    wrapper = shallowMount(EditObject, {
      mocks: {
        $route: {
          params: {},
          query: {basedOn: 'new'}

        },
        $router: {
          push: pushCallback
        }
      },
    });

    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      floor: '2/5',
      square: 23.5,
      rooms: 3,
    });
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      recent.respondWith({
        status: 200,
        response: {Id: 10}
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(1);
        expect(pushCallback.mock.calls[0][0]).toEqual({name: 'edit-announcement', query: {basedOn: 10}});
        done();
      });
    });
  });
  it('отправка нового объекта - переход с редактирования объявления', (done) => {
    pushCallback = jest.fn();
    wrapper = shallowMount(EditObject, {
      mocks: {
        $route: {
          params: {},
          query: {basedOn: '15'}

        },
        $router: {
          push: pushCallback
        }
      },
      stubs: ['router-link']
    });

    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      floor: '2/5',
      square: 23.5,
      rooms: 3,
    });
    expect(wrapper.vm.isInvalid).toBeFalsy();
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      recent.respondWith({
        status: 200,
        response: {Id: 10}
      }).then(() => {
        expect(pushCallback.mock.calls.length).toBe(1);
        expect(pushCallback.mock.calls[0][0]).toEqual({name: 'edit-announcement', query: {basedOn: 10}, params: {id: '15'}});
        done();
      });
    });
  });
  it('обновление объекта', (done) => {
    expect(wrapper.vm.isInvalid).toBeTruthy();
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      floor: '2/5',
      square: 23.5,
      rooms: 3,
    });
    expect(wrapper.vm.isInvalid).toBeFalsy();

    wrapper.vm.$route.params.id = 1;
    wrapper.vm.send();
    moxios.wait(() => {
      const recent = moxios.requests.mostRecent();
      expect(recent.url).toEqual('/odata/RealEstateObject(1)');
      expect(recent.config.method).toEqual('put');
      const documentBlob = recent.config.data.getAll('document')[0];
      const json = JSON.parse(String.fromCharCode.apply(null,
        new Uint16Array(documentBlob._buffer)));

      expect(json).toMatchSchema(realEstateTypeSchema);
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
  it('Меняем регион', () => {
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''}
    });

    expect(wrapper.vm.currentRegion).toBeDefined();
    expect(wrapper.vm.currentCity).toBeDefined();
    expect(wrapper.vm.currentStreet).toBeDefined();
    expect(wrapper.vm.currentBuilding).toBeDefined();
    wrapper.vm.blockWatch = false;
    wrapper.setData({currentRegion: null});
    wrapper.setData({currentRegion: {id: '000002', name: '2'}});

    expect(wrapper.vm.currentRegion).toBeDefined();
    expect(wrapper.vm.currentCity).toBeNull();
    expect(wrapper.vm.currentStreet).toBeNull();
    expect(wrapper.vm.currentBuilding).toBeNull();
  });
  it('Меняем город', () => {
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''}
    });

    expect(wrapper.vm.currentRegion).toBeDefined();
    expect(wrapper.vm.currentCity).toBeDefined();
    expect(wrapper.vm.currentStreet).toBeDefined();
    expect(wrapper.vm.currentBuilding).toBeDefined();
    wrapper.vm.blockWatch = false;
    wrapper.setData({currentCity: null});
    wrapper.setData({currentCity: {id: '000002', name: '2'}});

    expect(wrapper.vm.currentRegion).toBeDefined();
    expect(wrapper.vm.currentCity).toBeDefined();
    expect(wrapper.vm.currentStreet).toBeNull();
    expect(wrapper.vm.currentBuilding).toBeNull();
  });
  it('Меняем улицу', () => {
    wrapper.vm.blockWatch = true;
    wrapper.setData({
      currentRegion: {id: '000001', name: ''},
      currentCity: {id: '000001', name: ''},
      currentStreet: {id: '000001', name: ''},
      currentBuilding: {id: '000001', name: ''}
    });

    expect(wrapper.vm.currentRegion).toBeDefined();
    expect(wrapper.vm.currentCity).toBeDefined();
    expect(wrapper.vm.currentStreet).toBeDefined();
    expect(wrapper.vm.currentBuilding).toBeDefined();
    wrapper.vm.blockWatch = false;
    wrapper.setData({currentStreet: null});
    wrapper.setData({currentStreet: {id: '000002', name: '2'}});

    expect(wrapper.vm.currentRegion).toBeDefined();
    expect(wrapper.vm.currentCity).toBeDefined();
    expect(wrapper.vm.currentStreet).toBeDefined();
    expect(wrapper.vm.currentBuilding).toBeNull();
  });
});
