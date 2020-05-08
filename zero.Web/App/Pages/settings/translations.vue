<template>
  <div class="translations">
    <ui-header-bar title="Translations" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-button label="Add" icon="fth-plus" @click="add" />
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

  const editRouteName = zero.alias.sections.settings + '-' + zero.alias.settings.translations + '-edit';
  const createRouteName = zero.alias.sections.settings + '-' + zero.alias.settings.translations + '-create';

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
      this.tableConfig = {
        labelPrefix: '@translation.fields.',
        allowOrder: false,
        search: null,
        columns: {
          key: {
            as: 'text',
            link: item =>
            {
              return {
                name: editRouteName,
                params: { id: item.id }
              };
            }
          },
          value: {
            as: 'text',
            link: item =>
            {
              return {
                name: editRouteName,
                params: { id: item.id }
              };
            }
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
        else if (this.$route.name === createRouteName)
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
        this.$router.push({
          name: createRouteName
        });
      }
    }
  }
</script>