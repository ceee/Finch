<template>
  <div class="translations">
    <ui-header-bar title="@translation.list" :count="count" :back-button="true">
      <ui-table-filter :attach="$refs.table" />
      <ui-add-button :route="createRoute" blueprint-alias="translation" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table ref="table" config="translations" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/helpers/overlay.js';

  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.settings.translations + '-create'
    }),

    watch: {
      '$route': function (route)
      {
        this.handleRouteChange();
      }
    },

    created()
    {
      this.handleRouteChange();
    },

    methods: {
      goBack()
      {
        this.$router.go(-1);
      },

      handleRouteChange()
      {
        if (this.$route.params.id)
        {
          this.edit(this.$route.params.id);
        }
        else if (this.$route.name === this.createRoute)
        {
          this.edit();
        }
        else
        {
          Overlay.close();
        }
      },

      edit(id)
      {
        Overlay.open({
          component: () => import('./translation.vue'),
          width: 700,
          model: { id }
        }).then(res =>
        {
          this.$router.go(-1);
        }, () =>
        {
          this.$router.go(-1);
        });
      }
    }
  }
</script>