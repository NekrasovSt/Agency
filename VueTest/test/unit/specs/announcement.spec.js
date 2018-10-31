import moxios from "moxios";
import Announcement from '@/components/announcement';
import {mount, shallowMount} from "@vue/test-utils";

describe('announcement.vue', () => {
  let wrapper;
  const $route = {
    params: {id: 1},
  };
  beforeEach(function () {
    moxios.install();
    wrapper = shallowMount(Announcement, {
      mocks: {
        $route,
      },
      stubs: ['router-link']
    });
  });

  afterEach(function () {
    moxios.uninstall();
  });
  test('запрос на сервер после создания компонента', done => {
    shallowMount(Announcement, {
      mocks: {
        $route: {params: {id: 1}},
      },
      stubs: ['router-link']
    });
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe('odata/Announcement(1)?$expand=RealEstateObject');
      request.respondWith({
        status: 200,
        response: {
          RealEstateObject: {}
        }
      }).then(() => {

        done();
      });
    });
  });

});
