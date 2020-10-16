<template>
  <div class="translations">
    <ui-header-bar title="@translation.list" :count="count" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-add-button :route="createRoute" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" @count="count = $event" />
    </div>
  </div>
</template>


<script>
  import TranslationsApi from 'zero/resources/translations.js';
  import Overlay from 'zero/services/overlay.js';
  import AddOverlay from './translation';

  export default {
    data: () => ({
      count: 0,
      createRoute: zero.alias.settings.translations + '-create',
      tableConfig: zero.renderers.translation.list
    }),

    props: ['id'],

    watch: {
      '$route': function (route)
      {
        this.handleRouteChange();
      }
    },

    created()
    {
      this.tableConfig.items = TranslationsApi.getAll;

      this.handleRouteChange();
    },

    methods: {
      goBack()
      {
        this.$router.go(-1);
      },

      handleRouteChange()
      {
        if (this.id)
        {
          this.edit(this.id);
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
          component: AddOverlay,
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