<template>
  <div class="countries">
    <ui-header-bar title="Countries" :back-button="true">
      <ui-table-filter v-model="tableConfig" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table v-model="tableConfig" />
    </div>
  </div>
</template>


<script>
  import CountriesApi from 'zero/resources/countries.js';

  export default {
    data: () => ({
      tableConfig: {}
    }),

    created()
    {
      this.tableConfig = {
        labelPrefix: '@country.fields.',
        allowOrder: false,
        search: null,
        columns: {
          flag: {
            label: '',
            as: 'html',
            render: item => `<i class="flag flag-${item.code.toLowerCase()}"></i>`,
            width: 62
          },
          name: {
            as: 'text',
            bold: true,
            link: item =>
            {
              return {
                name: zero.alias.sections.settings + '-' + zero.alias.settings.countries + '-edit',
                params: { id: item.id }
              };
            }
          },
          code: 'text',
          isPreferred: {
            as: 'bool',
            width: 200
          },
          isActive: {
            as: 'bool',
            label: '@ui.active',
            width: 200
          }
        },
        items: CountriesApi.getAll
      };
    },


    methods: {
      goBack()
      {
        this.$router.go(-1);
      }
    }
  }
</script>