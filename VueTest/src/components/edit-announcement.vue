<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp">
      <div class="mdl-card__title">
        <h1 class="mdl-card__title-text">Объявление</h1>
      </div>
      <div class="mdl-card__supporting-text  mdl-grid">
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--limited">
            <v-select
              v-model="announcement.RealEstateObject"
              :filterable="false"
              @search="onObjectSearch"
              placeholder="Выберите объект недвижимости"
              inputId="object"
              label="title"
              :options="objects">
            </v-select>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="object">Объект</label>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="text" id="price" v-model.number="announcement.Price"
                   pattern="^[0-9]*(\.[0-9]+)?" required>
            <label class="mdl-textfield__label" for="price">Цена</label>
            <span class="mdl-textfield__error">Дробное число разделитель точка!</span>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label"
               v-if="announcement && announcement.RealEstateObject">
            <input class="mdl-textfield__input" type="text" id="region" v-model="announcement.RealEstateObject.Region"
                   readonly>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="region">Край область</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label"
               v-if="announcement && announcement.RealEstateObject">
            <input class="mdl-textfield__input" type="text" id="city" v-model="announcement.RealEstateObject.City"
                   readonly>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="city">Город</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label"
               v-if="announcement && announcement.RealEstateObject">
            <input class="mdl-textfield__input" type="text" id="street" v-model="announcement.RealEstateObject.Street"
                   readonly>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="street">Улица</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label"
               v-if="announcement && announcement.RealEstateObject">
            <input class="mdl-textfield__input" type="text" id="building"
                   v-model="announcement.RealEstateObject.Building"
                   readonly>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="building">Строение/Здание</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label"
               v-if="announcement && announcement.RealEstateObject">
            <input class="mdl-textfield__input" type="text" id="square" v-model="announcement.RealEstateObject.Square"
                   readonly>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="square">Площадь</label>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--6-col radio-container mdl-textfield">
          <label class="mdl-textfield__label mdl-textfield__label--fixed">Тип объявления</label>
          <label class="mdl-radio mdl-js-radio mdl-js-ripple-effect" v-bind:for="key"
                 v-for="(value, key) in announcementTypes">
            <input type="radio" v-bind:id="key" class="mdl-radio__button" v-bind:value="key" name="types"
                   v-model="announcement.AnnouncementType">
            <span class="mdl-radio__label">{{value}}</span>
          </label>
        </div>

      </div>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <button
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          v-on:click="send()">
          сохранить
        </button>
        <button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect">
          Отмена
        </button>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';
  import {announcementTypes} from '../miscellaneous/announcement-types'

  export default {
    name: "EditAnnouncement",
    data() {
      return {
        announcement: {AnnouncementType: 'Buy', RealEstateObject: {}},
        objects: [],
        announcementTypes: announcementTypes
      }
    },
    computed: {
      isInvalid() {
        return !this.announcement ||
          !this.announcement.RealEstateObject ||
          this.announcement.RealEstateObject.Id === undefined ||
          !/^[0-9]+(\.[0-9]+)?/.test(this.announcement.Price);
      }
    },
    methods: {
      label(obj) {
        return `г.${obj.City} ул.${obj.Street} ${obj.Building}`;
      },
      onObjectSearch(search, loading) {
        loading(true);

        let url = search ? `odata/RealEstateObject?$filter=contains(toLower(Street),toLower('${search}'))` : 'odata/RealEstateObject';
        axios.get(url).then(response => {
          this.objects = response.data.value.map(i => {
            i.title = this.label(i);
            return i;
          })
        }).finally(() => {
          loading(false);
        });
      }
    },
    mounted() {
      if (window.componentHandler)
        componentHandler.upgradeAllRegistered();
    }
  }
</script>

<style scoped>
  .radio-container {
    display: flex;
    justify-content: space-between;
  }
</style>
