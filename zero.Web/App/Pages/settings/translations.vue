<template>
  <div class="translations">
    <ui-header-bar title="@translation.list" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-button label="@ui.add" icon="fth-plus" @click="add" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import TranslationsApi from 'zero/resources/translations.js';
  import Overlay from 'zero/services/overlay.js';
  import AddOverlay from './translation';

  const baseRoute = zero.alias.sections.settings + '-' + zero.alias.settings.translations;
  const createRoute = baseRoute + '-create';

  export default {
    data: () => ({
      tableConfig: {}
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
      let link = item =>
      {
        return {
          name: baseRoute + '-edit',
          params: { id: item.id }
        };
      };

      this.tableConfig = {
        labelPrefix: '@translation.fields.',
        allowOrder: false,
        search: null,
        columns: {
          key: {
            as: 'text',
            link: link
          },
          value: {
            as: 'text',
            link: link
          }
        },
        items: TranslationsApi.getAll
      };

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
        else if (this.$route.name === createRoute)
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
        }).then(() =>
        {

        }, () =>
        {
          this.$router.go(-1);
        });
      },

      add()
      {
        this.$router.push({ name: createRoute });
      }
    }
  }
</script>