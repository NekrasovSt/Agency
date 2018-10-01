<template>

  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp">
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Параметры фильтрации</h2>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-cell mdl-cell--4-col">
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="apartment">
            <input type="checkbox" id="apartment" class="mdl-checkbox__input" value="apartment" v-model="types">
            <span class="mdl-checkbox__label">Квартира</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="new-building">
            <input type="checkbox" id="new-building" class="mdl-checkbox__input" value="NewBuilding" v-model="types">
            <span class="mdl-checkbox__label">Новостройка</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="house">
            <input type="checkbox" id="house" class="mdl-checkbox__input" value="House" v-model="types">
            <span class="mdl-checkbox__label">Дом</span>
          </label>
        </div>
      </div>
    </div>
    <div class="mdl-grid mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp"
         v-for="item in announcements">
      <div class="mdl-card__media mdl-cell mdl-cell--12-col-tablet">
        <img class="article-image" src=" images/example-blog01.jpg" border="0" alt="">
      </div>
      <div class="mdl-cell mdl-cell--8-col">
        <h2 class="mdl-card__title-text">Velit anim eiusmod labore sit amet</h2>
        <div class="mdl-card__supporting-text padding-top">
          <span>{{item.creationDate|format}}</span>
          <div id="tt1" class=" icon material-icons portfolio-share-btn">share</div>
          <div class="mdl-tooltip" for="tt1">
            Share via social media
          </div>
        </div>
        <div class="mdl-card__supporting-text no-left-padding">
          <p>Excepteur reprehenderit sint exercitation ipsum consequat qui sit id velit elit. Velit anim eiusmod labore
            sit amet. Voluptate voluptate irure occaecat deserunt incididunt esse in. Qui ullamco consectetur aute
            fugiat officia ullamco proident Lorem ad irure. Sint eu ut consectetur ut esse veniam laboris adipisicing
            aliquip minim anim labore commodo.</p>
          <span>Category: <a href="#">Latest</a></span>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    name: 'ObjectList',
    data() {
      return {
        announcements: null,
        types: [],
      };
    },
    created() {
      if (this.$route.params.type) {
        this.types.push(this.$route.params.type);
      }
      axios.get(`odata/Announcement?$expand=RealEstateObject`).then(response => {
        this.announcements = response.data.value;
      }).
        finally(() => {
        });
    },
  };
</script>

<style scoped>

</style>
