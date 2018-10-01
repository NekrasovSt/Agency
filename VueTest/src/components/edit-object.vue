<template>
  <div>
    <h1>Заполните форму</h1>
    <v-select
      v-model="currentRegion"
      :filterable="false"
      label="name"
      @search="onRegionSearch"
      :options="regions">
    </v-select>
    <v-select
      v-model="currentCity"
      :disabled="!regionSelected"
      :filterable="false"
      label="name"
      @search="onCitySearch"
      :options="cities">
    </v-select>
    <v-select
      v-model="currentStreet"
      :disabled="!citySelected"
      :filterable="false"
      label="name"
      @search="onStreetSearch"
      :options="streets">
    </v-select>
    <v-select
      v-model="currentBuilding"
      :disabled="!streetSelected"
      :filterable="false"
      label="name"
      @search="onBuildingSearch"
      :options="buildings">
    </v-select>
    <input type="text" v-model="floor" required>
    <input type="number" v-model="square" required>
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
    },
    methods: {
      onRegionSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetRegion?query=${search}`).then(response => {
          this.regions = response.data.result;
        }).
          finally(() => {
            loading(false);
          });
      },
      onCitySearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetCity?query=${search}&parentId=${this.currentRegion.id}`).then(response => {
          this.cities = response.data.result;
        }).
          finally(() => {
            loading(false,
            );
          });
      },
      onStreetSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetStreet?query=${search}&parentId=${this.currentCity.id}`).then(response => {
          this.streets = response.data.result;
        }).
          finally(() => {
            loading(false);
          });
      },
      onBuildingSearch(search, loading) {
        loading(true);
        axios.get(`api/Address/GetBuilding?query=${search}&parentId=${this.currentStreet.id}`).then(response => {
          this.buildings = response.data.result;
        }).
          finally(() => {
            loading(false);
          });
      },
    },
  };
</script>

<style scoped>

</style>
