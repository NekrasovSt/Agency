import {mount} from '@vue/test-utils';
import moxios from 'moxios';
import RealEstateObjectList from '@/components/real-estate-object-list';


describe('real-estate-object-list.vue', () => {
  let wrapper;
  beforeEach(function () {
    moxios.install();
    wrapper = mount(RealEstateObjectList);
  });

  afterEach(function () {
    moxios.uninstall();
  });
  test('запрос на сервер после создания компонента', done => {
    let obj = mount(RealEstateObjectList);
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe('odata/RealEstateObject?$top=100&$skip=0&$count=true');
      done();
    });
  });
  test('нет больше записей', () => {
    wrapper.setData({top: 100, skip: 100, count: 200});
    expect(wrapper.vm.canNext).toBeFalsy();
  });
  test('есть записи', () => {
    wrapper.setData({top: 100, skip: 100, count: 300});
    expect(wrapper.vm.canNext).toBeTruthy();
  });
  test('нет предыдущих страниц', () => {
    wrapper.setData({top: 100, skip: 0, count: 200});
    expect(wrapper.vm.canBack).toBeFalsy();
  });
  test('есть предыдущих страницы', () => {
    wrapper.setData({top: 100, skip: 100, count: 300});
    expect(wrapper.vm.canBack).toBeTruthy();
  });
  test('вперед', done => {
    wrapper.setData({top: 100, skip: 100, count: 300});
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe('odata/RealEstateObject?$top=100&$skip=200&$count=true');
      done();
    });
    wrapper.vm.next();
  });
  test('назад', done => {
    wrapper.setData({top: 100, skip: 100, count: 300});
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe('odata/RealEstateObject?$top=100&$skip=0&$count=true');
      done();
    });
    wrapper.vm.back();
  });
  test('фильтр', done => {
    wrapper.setData({filter: 'Ленина'});
    moxios.wait(() => {
      let request = moxios.requests.mostRecent();
      expect(request.url).toBe("odata/RealEstateObject?$top=100&$skip=0&$count=true&$filter=contains(toLower(Street),toLower('Ленина'))");
      done();
    });
    wrapper.vm.refresh();
  });
});
