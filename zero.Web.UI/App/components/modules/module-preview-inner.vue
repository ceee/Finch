<script>
  import Vue from 'vue';

  export default {
    name: 'uiModulePreviewInner',

    props: {
      value: {
        type: Object,
        default: () => { }
      },
      template: {
        type: String,
        required: true
      }
    },

    render: function (h)
    {
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