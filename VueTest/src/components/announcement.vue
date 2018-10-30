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
      <div class="mdl-card__actions mdl-card--border mdl-grid">
        <div class="mdl-layout-spacer"></div>
        <router-link class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent" to="/object-list">
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
      let url = `odata/Announcement(${this.$route.params.id})?$expand=RealEstateObject`;
      axios.get(url).then(response => {
        this.item = response.data;
      }).catch(error => {

      }).finally(() => {

      });
    },
  };
</script>

<style scoped>

</style>
