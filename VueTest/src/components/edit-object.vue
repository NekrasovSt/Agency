<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp">
      <div class="mdl-card__title">
        <h1 class="mdl-card__title-text">Заполните форму</h1>
      </div>
      <div class="mdl-card__supporting-text  mdl-grid">
        <div class="mdl-cell mdl-cell--4-col mdl-card__media thumb" v-for="(file, index) in images">
          <img :src="source(file)" class="article-image thumb--image"/>
          <div class="mdl-card__menu">
            <button class="mdl-button mdl-button--icon mdl-js-button mdl-js-ripple-effect"
                    @click.prevent="remove(file)">
              <i class="material-icons">delete</i>
            </button>
          </div>
        </div>
      </div>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <label
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent">
          Загрузит файл
          <input type="file" class="file-input" ref="files" multiple v-on:change="handleFilesUpload($refs.files)"/>
        </label>
      </div>
      <form class="mdl-card__supporting-text mdl-grid" name="object-from">
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--limited">
            <v-select
              v-model="currentRegion"
              :filterable="false"
              label="name"
              @search="onRegionSearch"
              placeholder="Выберите край/область..."
              inputId="region"
              :options="regions">
            </v-select>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="region">Край</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--limited">
            <v-select
              v-model="currentCity"
              :disabled="!regionSelected"
              :filterable="false"
              label="name"
              placeholder="Выберите город..."
              @search="onCitySearch"
              inputId="city"
              :options="cities">
            </v-select>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="city">Город</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--limited">
            <v-select
              v-model="currentStreet"
              :disabled="!citySelected"
              :filterable="false"
              label="name"
              placeholder="Выберите улицу..."
              @search="onStreetSearch"
              inputId="street"
              :options="streets">
            </v-select>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="street">Улица</label>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--limited">
            <v-select
              v-model="currentBuilding"
              :disabled="!streetSelected"
              :filterable="false"
              label="name"
              placeholder="Выберите номер дома/строения..."
              @search="onBuildingSearch"
              inputId="building"
              :options="buildings">
            </v-select>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="building">Номер дома/строения</label>
          </div>
        </div>
        <div class="mdl-cell mdl-cell--6-col">
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="text" id="floor" v-model="floor" pattern="^[1-9]\d?\/[1-9]\d?$"
                   required>
            <label class="mdl-textfield__label" for="floor">Этаж/Этажей</label>
            <span class="mdl-textfield__error">1..99/1..99! Например 3/5</span>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="text" id="square" v-model.number="square"
                   pattern="^[0-9]*(\.[0-9]+)?" required>
            <label class="mdl-textfield__label" for="square">Площадъ</label>
            <span class="mdl-textfield__error">Дробное число разделитель точка!</span>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--floating-label">
            <input class="mdl-textfield__input" type="text" id="rooms" v-model.number="rooms" required
                   pattern="^[1-9]\d?$">
            <label class="mdl-textfield__label" for="rooms">Комнат</label>
            <span class="mdl-textfield__error">Число от 1 до 99!</span>
          </div>
          <div class="mdl-textfield mdl-js-textfield mdl-textfield--limited">
            <v-select
              v-model="realEstateType"
              label="name"
              placeholder="Выберите тип объекта"
              inputId="type"
              :options="realEstateTypes">
            </v-select>
            <label class="mdl-textfield__label mdl-textfield__label--fixed" for="type">Тип недвижимости</label>
          </div>
        </div>
      </form>
      <div class="mdl-grid mdl-card__actions">
        <div class="mdl-layout-spacer"></div>
        <button
          class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect mdl-button--accent"
          v-on:click="send()"
          v-bind:disabled="isInvalid">
          сохранить
        </button>
        <button class="mdl-button mdl-js-button mdl-button--primary mdl-button--colored mdl-js-ripple-effect">
          Отмена
        </button>
      </div>
    </div>

    <div ref="snack" class="mdl-js-snackbar mdl-snackbar">
      <div class="mdl-snackbar__text"></div>
      <button class="mdl-snackbar__action" type="button"></button>
    </div>
  </div>
</template>

<script>
  import axios from 'axios';

  export default {
    name: 'EditObject',
    data() {
      return {
        currentRegion: null,
        currentCity: null,
        currentStreet: null,
        currentBuilding: null,
        square: null,
        regions: [],
        cities: [],
        streets: [],
        buildings: [],
        floor: null,
        images: [],
        rooms: null,
        realEstateType: {id: 'Apartment', name: 'Квартира'},
        realEstateTypes: [
          {id: 'Apartment', name: 'Квартира'},
          {id: 'NewBuilding', name: 'Новостройка'},
          {id: 'House', name: 'Дом'}],
        blockWatch: false,
      };
    },
    computed: {
      regionSelected() {
        return !!this.currentRegion;
      },
      citySelected() {
        return !!this.currentCity;
      },
      streetSelected() {
        return !!this.currentStreet;
      },
      buildingSelected() {
        return !!this.currentBuilding;
      },
      isInvalid() {
        return !this.regionSelected ||
          !this.citySelected ||
          !this.streetSelected ||
          !this.buildingSelected ||
          !/^[1-9]\d?\/[1-9]\d?$/.test(this.floor) ||
          !/^[0-9]*(\.[0-9]+)?/.test(this.square) ||
          !/^[1-9]\d?$/.test(this.rooms);
      },
    },
    methods: {
      remove(file) {
        let index = this.images.indexOf(file);
        this.images.splice(index, 1);
      },
      source(file) {
        return URL.createObjectURL(file);
      },
      handleFilesUpload(event) {
        let files = event.files;//this.$refs.files.files;
        for (let i = 0; i < files.length; i++) {
          if (!this.images.some(
            j => j.name === files[i].name && j.size === files[i].size)) {
            this.images.push(files[i]);
          }
        }
      },
      send() {
        let formData = new FormData();
        this.images.forEach((i, j) => {
          formData.append(`file`, i);
        });
        const model = {
          Id: this.$route.params.id || 0,
          Rooms: this.rooms,
          Square: this.square,
          Floor: this.floor,
          Region: this.currentRegion.name,
          City: this.currentCity.name,
          Street: this.currentStreet.name,
          Building: this.currentBuilding.name,
          Code: this.currentBuilding.id,
          RealEstateType: this.realEstateType.id,
        };
        const blob = new Blob([JSON.stringify(model)], {
          type: 'application/json',
        });
        formData.append('document', blob);
        let promise;
        if (this.$route.params.id === undefined) {
          promise = axios.post('/odata/RealEstateObject', formData, {headers: {'Content-type': 'multipart/form-data'}});
        }
        else {
          promise = axios.put(`/odata/RealEstateObject(${this.$route.params.id})`, formData,
            {headers: {'Content-type': 'multipart/form-data'}});
        }
        promise.then(() => {
          this.$refs.snack.MaterialSnackbar ? this.$refs.snack.MaterialSnackbar.showSnackbar({message: 'Данные успешно сохранены!'}) : 0;
          this.$router.push({name: 'objectList'});
        }).catch((error) => {
          this.$refs.snack.MaterialSnackbar ? this.$refs.snack.MaterialSnackbar.showSnackbar({message: 'Произошла ошибка!'}) : 0;
        });
      },
      onRegionSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetRegion?query=${search}`).then(response => {
          this.regions = response.data.result;
        }).finally(() => {
          loading(false);
        });
      },
      onCitySearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetCity?query=${search}&parentId=${this.currentRegion.id}`).then(response => {
          this.cities = response.data.result;
        }).finally(() => {
          loading(false,
          );
        });
      },
      onStreetSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetStreet?query=${search}&parentId=${this.currentCity.id}`).then(response => {
          this.streets = response.data.result;
        }).finally(() => {
          loading(false);
        });
      },
      onBuildingSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetBuilding?query=${search}&parentId=${this.currentStreet.id}`).then(response => {
          this.buildings = response.data.result;
        }).finally(() => {
          loading(false);
        });
      },
    },
    created() {
      if (this.$route.params.id !== undefined) {
        this.blockWatch = true;
        let url = `odata/RealEstateObject(${this.$route.params.id})`;
        axios.get(url).then(response => {
          this.item = response.data;
          this.rooms = response.data.Rooms;
          this.floor = response.data.Floor;
          this.square = response.data.Square;

          return axios.get(`api/Address/GetParents?code=${this.item.Code}&building=${this.item.Building}`);
        }).then(response => {
          this.currentRegion = response.data.parents.find(parent => parent.contentType === 'region');
          this.currentCity = response.data.parents.find(parent => parent.contentType === 'city');
          this.currentStreet = response.data.parents.find(parent => parent.contentType === 'street');
          this.currentBuilding = response.data;
        }).catch(error => {

        }).finally(() => {
          this.blockWatch = false;
        });
      }
    },
    watch: {
      currentRegion: function (nw, old) {
        if (this.blockWatch)
          return;
        this.currentCity = null;
      },
      currentCity: function (nw, old) {
        if (this.blockWatch)
          return;
        this.currentStreet = null;
      },
      currentStreet: function (ne, old) {
        if (this.blockWatch)
          return;
        this.currentBuilding = null;
      }
    },
    mounted() {
      if (window.componentHandler)
        componentHandler.upgradeAllRegistered();
    },
  };
</script>

<style>
  .thumb {
    position: relative;
  }

  .thumb--image {
    max-height: 200px;
    object-fit: contain;

  }

  .file-input {
    display: none;
  }

</style>
