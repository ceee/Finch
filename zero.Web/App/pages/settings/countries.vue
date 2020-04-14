<template>
  <div class="countries">
    <ui-header-bar title="Countries" :on-back="goBack">
      <ui-table-filter :filter="false" />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table :config="tableConfig" />
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
        sort: false,
        columns: {
          flag: {
            label: '',
            as: 'html',
            render: item => `<i class="flag flag-${item.code.toLowerCase()}"></i>`,
            width: 62
          },
          name: {
            as: 'text',
            bold: true
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