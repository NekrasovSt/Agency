import ObjectList from '@/components/object-list';
import {mount} from '@vue/test-utils';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';


describe('edit-object.vue', () => {
  const $route = {
    params: {},
  };
  const wrapper = mount(ObjectList, {
    mocks: {
      $route,
    },
  });
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
    expect(wrp.find('input[value="apartment"]').element.checked).toBeTruthy();
    expect(wrp.findAll('input[type="checkbox"]').
      filter(w => w.element.checked).length).toBe(1);
  });
  test('запрос на сервер', done => {
    let mock = new MockAdapter(axios);
    mock.onGet(/Announcement/).reply(function(config) {
      return [200, {}];
    });
    mount(ObjectList, {
      mocks: {
        $route: {params: {}},
      },
    });
  });
});
