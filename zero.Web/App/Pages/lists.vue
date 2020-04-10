<template>
  <div class="lists">
    <ui-header-bar title="Lists">
      <ui-button type="light" label="Add list" icon="fth-plus" />
      <ui-table-filter />
    </ui-header-bar>
    <div class="ui-blank-box">
      <ui-table :config="tableConfig" />
    </div>
  </div>
</template>


<script>
  export default {
    name: 'app-lists',

    data: () => ({
      tableConfig: {}
    }),

    created()
    {
      this.tableConfig = {
        labelPrefix: '@order.fields.',
        columns: {
          no: 'text',
          createdDate: {
            label: '@ui.createdDate',
            as: 'datetime',
            size: 's'
          },
          username: 'text',
          price: {
            as: 'price',
            size: 's',
            sort: false
          },
          isPublished: {
            label: 'Published',
            as: 'bool',
            size: 'xs'
          },
          status: {
            as: 'html',
            render: item => '<b>' + item.status + '</b>'
          }
        },
        items: this.getItems
      };
    },


    methods: {

      getItems(config)
      {
        console.info('getitems with config', config);

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
              username: 'Christian Klika',
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
      }

    }
  }
</script>