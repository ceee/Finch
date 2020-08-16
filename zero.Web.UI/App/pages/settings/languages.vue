<template>
  <div class="languages">
    <ui-header-bar title="@language.list" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-add-button :route="createRoute" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import LanguagesApi from 'zero/resources/languages.js';

  export default {
    data: () => ({
      createRoute: zero.alias.sections.settings + '-' + zero.alias.settings.languages + '-create',
      tableConfig: {}
    }),

    created()
    {
      this.tableConfig = {
        labelPrefix: '@language.fields.',
        allowOrder: false,
        search: null,
        columns: {
          name: {
            label: '@ui.name',
            as: 'text',
            bold: true,
            shared: true,
            link: item =>
            {
              return {
                name: zero.alias.sections.settings + '-' + zero.alias.settings.languages + '-edit',
                params: { id: item.id }
              };
            }
          },
          code: 'text',
          isDefault: {
            as: 'bool',
            width: 200
          }
        },
        items: LanguagesApi.getAll
      };
    }
  }
</script>