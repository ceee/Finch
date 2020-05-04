<template>
  <div class="languages">
    <ui-header-bar title="@language.list" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
      <ui-button label="Add" icon="fth-plus" />
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
            link: item =>
            {
              return {
                name: zero.alias.sections.settings + '-' + zero.alias.settings.languages + '-edit',
                params: { id: item.id }
              };
            }
          },
          code: 'text',
          isActive: {
            as: 'bool',
            label: '@ui.active',
            width: 200
          },
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