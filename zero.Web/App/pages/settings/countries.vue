<template>
  <div class="lists">
    <ui-header-bar title="Countries" :on-back="goBack">
      <ui-button type="light" label="Add" icon="fth-plus" />
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
            width: 60
          },
          name: {
            as: 'text',
            bold: true
          },
          code: 'text',
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

      getItems(config)
      {
        return new Promise(resolve =>
        {
          resolve([
            {
              no: 1,
              createdDate: '2020-03-05T10:17:25.229+01:00',
              username: 'Tobias Klika',
              price: 70.90,
              status: 'Open',
              isPublished: true
            },
            {
              no: 2,
              createdDate: '2020-03-05T10:17:25.229+01:00',
              username: 'Fox Tales',
              price: 12.95,
              status: 'Processing',
              isPublished: true
            },
            {
              no: 3,
              createdDate: '2020-03-07T17:17:25.229+01:00',
              username: 'Christian Klika, das ist mein Name und der könnte noch viel länger sein',
              price: 123.00,
              status: 'Completed'
            },
            {
              no: 1,
              createdDate: '2020-03-05T10:17:25.229+01:00',
              username: 'Tobias Klika',
              price: 70.90,
              status: 'Open',
              isPublished: true
            },
            {
              no: 2,
              createdDate: '2020-03-05T10:17:25.229+01:00',
              username: 'Fox Tales',
              price: 12.95,
              status: 'Processing'
            },
          ]);
        });
      },

      goBack()
      {
        this.$router.go(-1);
      }

    }
  }
</script>