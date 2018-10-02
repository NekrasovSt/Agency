<template>

  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-js-progress mdl-progress--accent" style="height: 4px"
         v-bind:class="{'mdl-progress mdl-progress__indeterminate': loading}"></div>
    <div class="mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp ">
      <div class="mdl-card__title">
        <h2 class="mdl-card__title-text">Параметры фильтрации</h2>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <!--Тип недвижимости-->
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
        <!--Количество комнат-->
        <div class="mdl-cell mdl-cell--4-col">
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room1">
            <input type="checkbox" id="room1" class="mdl-checkbox__input" value="1" v-model="rooms">
            <span class="mdl-checkbox__label">1 комната</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room2">
            <input type="checkbox" id="room2" class="mdl-checkbox__input" value="2" v-model="rooms">
            <span class="mdl-checkbox__label">2 комнаты</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room3">
            <input type="checkbox" id="room3" class="mdl-checkbox__input" value="3" v-model="rooms">
            <span class="mdl-checkbox__label">3 комнаты</span>
          </label>
          <label class="mdl-checkbox mdl-js-checkbox mdl-js-ripple-effect" for="room4">
            <input type="checkbox" id="room4" class="mdl-checkbox__input" value="4" v-model="rooms">
            <span class="mdl-checkbox__label">4 и более комнат</span>
          </label>
        </div>
        <div class="mdl-cell mdl-cell--4-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="number" pattern="^[0-9]*" id="priceFrom" min="0"
                   v-model.number="priceFrom">
            <label class="mdl-textfield__label" for="priceFrom">цена от</label>
            <span class="mdl-textfield__error">Должно быть число больше 0!</span>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="number" pattern="^[0-9]*" id="priceTo" min="0"
                   v-model.number="priceTo">
            <label class="mdl-textfield__label" for="priceTo">цена до</label>
            <span class="mdl-textfield__error">Должно быть число больше 0!</span>
          </div>
        </div>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <button
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          v-on:click="refresh()"
          v-bind:disabled="isInvalid">
          найти
        </button>
      </div>
    </div>
    <div class="mdl-cell mdl-grid  mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp"
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
        rooms: [],
        priceFrom: null,
        priceTo: null,
        loading: false,
      };
    },
    computed: {
      isInvalid() {
        if (isNaN(this.priceFrom) || isNaN(this.priceTo))
          return true;

        return this.priceTo !== '' && this.priceFrom !== '' &&
          this.priceTo !== null && this.priceFrom !== null &&
          this.priceFrom > this.priceTo;
      },
    },
    methods: {
      refresh() {
        let url = `odata/Announcement?$expand=RealEstateObject`;

        let filter = [];
        if (this.rooms.length > 0 && this.rooms.length < 4) {
          filter.push('(' + this.rooms.map(j => `RealEstateObject/Rooms eq ${j}`).join(' or ') + ')');
        }
        if (this.priceFrom !== null && this.priceFrom !== '') {
          filter.push(`(Price ge ${this.priceFrom})`);
        }
        if (this.priceTo !== null && this.priceTo !== '') {
          filter.push(`(Price le ${this.priceTo})`);
        }
        if (filter.length !== 0) {
          url += '&$filter=' + filter.join(' and ');
        }
        this.loading = true;
        axios.get(url).then(response => {
          this.announcements = response.data.value;
        }).catch(error => {

        }).finally(() => {
          setTimeout(()=>{
            this.loading = false;
          },300);

        });
      },
    },
    created() {
      if (this.$route.params.type) {
        this.types.push(this.$route.params.type);
      }
      this.refresh();
    },
  };
</script>

<style scoped>

</style>
