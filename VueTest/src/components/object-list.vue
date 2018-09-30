<template>
  <div class="mdl-grid portfolio-max-width">
    <div class="mdl-grid mdl-cell mdl-cell--12-col mdl-cell--4-col-tablet mdl-card mdl-shadow--4dp">
      <div class="mdl-card__media mdl-cell mdl-cell--12-col-tablet">
        <img class="article-image" src=" images/example-blog01.jpg" border="0" alt="">
      </div>
      <div class="mdl-cell mdl-cell--8-col" v-for="item in announcements">
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
      };
    },
    mounted() {
      axios.get(`odata/Announcement?$expand=RealEstateObject`).then(response = > {
        this.announcements = response.data.value;
    }).
      finally(() = > {};
    )
    },
  };
</script>

<style scoped>

</style>
