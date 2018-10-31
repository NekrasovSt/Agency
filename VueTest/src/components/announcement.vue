<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp mdl-grid">
      <!-- Photo -->
      <div class="mdl-cell--12-col mdl-grid" v-if="item">
        <div class="mdl-cell  mdl-cell--4-col mdl-card__media"
             v-for="file in item.RealEstateObject.RealEstateObjectFile">
          <img class="article-image" v-bind:src="'/photos/' + file.Name">
        </div>
      </div>
      <div class="mdl-cell mdl-cell--6-col mdl-card__supporting-text" v-if="item">
        Край: <strong>{{item.RealEstateObject.Region}}</strong><br/>
        Город: <strong>{{item.RealEstateObject.City}}</strong><br/>
        Улица:<strong> {{item.RealEstateObject.Street}}</strong><br/>
        Номер дома: <strong>{{item.RealEstateObject.Building}}</strong> <br/>
      </div>
      <div class="mdl-cell mdl-cell--6-col mdl-card__supporting-text" v-if="item">
        Этаж/Этажей: <strong>{{item.RealEstateObject.Floor}}</strong><br/>
        Площадь: <strong>{{item.RealEstateObject.Square}}</strong><br/>
        Комнат:<strong>{{item.RealEstateObject.Rooms}}</strong><br/>
        Цена: <strong>{{item.Price}}</strong><br/>
      </div>
      <div class="mdl-cell mdl-cell--12-col mdl-card__supporting-text" v-if="item">
        {{item.RealEstateObject.Description}}
      </div>
      <div class="mdl-cell mdl-cell--12-col map" ref="map">


      </div>
      <div class="mdl-card__actions mdl-card--border mdl-grid">
        <div class="mdl-layout-spacer"></div>
        <router-link
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          to="/object-list">
          Назад
        </router-link>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    name: 'Announcement',
    data() {
      return {item: null};
    },
    created() {
      let self = this;
      let url = `odata/Announcement(${this.$route.params.id})?$expand=RealEstateObject`;
      let promise = axios.get(url);
      promise.then(response => {
        this.item = response.data;
      }).catch(error => {

      }).finally(() => {

      });
      if (!window.ymaps)
        return;
      ymaps.ready(() => {
        self.myMap = new ymaps.Map(self.$refs.map, {
          center: [58.005818, 56.234205],
          zoom: 10,
          controls: ['geolocationControl', 'fullscreenControl', 'zoomControl'],
        });
        promise.then(response => {
          let address = `${response.data.RealEstateObject.City}, ${response.data.RealEstateObject.Street}, ${response.data.RealEstateObject.Building}`;
          return ymaps.geocode(address);
        }).then(res => {
          let firstGeoObject = res.geoObjects.get(0),
            // Координаты геообъекта.
            coords = firstGeoObject.geometry.getCoordinates(),
            // Область видимости геообъекта.
            bounds = firstGeoObject.properties.get('boundedBy');

          firstGeoObject.options.set('preset', 'islands#darkBlueDotIconWithCaption');
          // Получаем строку с адресом и выводим в иконке геообъекта.
          firstGeoObject.properties.set('iconCaption', firstGeoObject.getAddressLine());

          // Добавляем первый найденный геообъект на карту.
          self.myMap.geoObjects.add(firstGeoObject);
          // Масштабируем карту на область видимости геообъекта.
          self.myMap.setBounds(bounds, {
            // Проверяем наличие тайлов на данном масштабе.
            checkZoomRange: true
          });
        });
      });
    },
    beforeDestroy() {
      this.myMap.destroy();
    }
  };
</script>

<style scoped>
  .map {
    height: 400px;
  }
</style>
