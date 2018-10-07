import moxios from "moxios";
import Announcement from '@/components/announcement';
import {mount} from "@vue/test-utils";

describe('announcement.vue', () => {
  let wrapper;
  const $route = {
    params: {id: 1},
  };
  beforeEach(function () {
    moxios.install();
    wrapper = mount(Announcement, {
      mocks: {
        $route,
      },
    });
  });

  afterEach(function () {
    moxios.uninstall();
  });
  test('запрос на сервер после создания компонента', done => {
    mount(Announcement, {
      mocks: {
        $route: {params: {id: 1}},
      },
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
