<script>
  export default {
    name: 'uiModulePreviewInner',
    props: {
      value: {
        type: Object,
        default: () => { }
      },
      options: {
        type: Object
      },
      template: {
        type: String,
        default: null
      }
    },
    render: function (h)
    {
      if (!this.template)
      {
        return;
      }
      return h(
        {
          template: '<div class="ui-module-preview-inner">' + this.template + '</div>',
          props: {
            model: {
              type: Object,
              default: () => { }
            },
            options: {
              type: Object
            },
          }
        },
        {
          props: {
            model: this.value,
            options: this.options
          }
        });
    },
  }
</script>