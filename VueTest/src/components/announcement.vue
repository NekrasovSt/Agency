<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-cell mdl-cell--12-col mdl-card mdl-shadow--4dp">
      <!-- Photo -->
      <div class="mdl-cell--12-col mdl-grid">
        <div class="mdl-cell  mdl-cell--4-col mdl-card__media">
          <img class="article-image" src="@/assets/example-work06.jpg">
        </div>
        <div class="mdl-cell  mdl-cell--4-col mdl-card__media">
          <img class="article-image" src="@/assets/example-work07.jpg">
        </div>
        <div class="mdl-cell  mdl-cell--4-col mdl-card__media">
          <img class="article-image" src="@/assets/example-work08.jpg">
        </div>
        <div class="mdl-cell  mdl-cell--4-col mdl-card__media">
          <img class="article-image" src="@/assets/example-work09.jpg">
        </div>
      </div>
      <div class="mdl-card__supporting-text" v-if="item"><strong>Край:</strong> {{item.RealEstateObject.Region}}<br/>
        <strong>Город:</strong> {{item.RealEstateObject.City}}<br/>
        <strong>Улица:</strong> {{item.RealEstateObject.Street}}<br/>
        <strong>Номер дома:</strong> {{item.RealEstateObject.Building}}<br/>
        {{item.RealEstateObject.Description}}
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
