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

  export default {
    data: () => ({
      tableConfig: {}
    }),

    props: ['id'],

    watch: {
      'id': function (id)
      {
        if (id) this.edit(id);
        else Overlay.close();
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
                name: zero.alias.sections.settings + '-' + zero.alias.settings.translations + '-edit',
                params: { id: item.id }
              };
            }
          },
          value: {
            as: 'text',
            link: item =>
            {
              return {
                name: zero.alias.sections.settings + '-' + zero.alias.settings.translations + '-edit',
                params: { id: item.id }
              };
            }
          }
        },
        items: TranslationsApi.getAll
      };

      if (this.id)
      {
        this.edit(this.id);
      }
    },

    methods: {
      goBack()
      {
        this.$router.go(-1);
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
          name: zero.alias.sections.settings + '-' + zero.alias.settings.translations + '-edit',
          params: { id: 'new' }
        });
      }
    }
  }
</script>