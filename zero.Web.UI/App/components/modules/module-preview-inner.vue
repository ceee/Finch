<script>
  export default {
    name: 'uiModulePreviewInner',

    props: {
      value: {
        type: Object,
        default: () => { }
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
            }
          }
        },
        {
          props: {
            model: this.value
          }
        });
    },
  }
</script>