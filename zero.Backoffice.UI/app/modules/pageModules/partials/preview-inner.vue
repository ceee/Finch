<script lang="ts">
  import { defineComponent, h } from 'vue';
  import { compile } from '@vue/compiler-dom';

  export default defineComponent({

    name: 'uiModulePreviewInner',

    compatConfig: {
      MODE: 3
    },

    props: {
      model: {
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

    setup(props, { slots, attrs, emit })
    {
      if (!props.template)
      {
        return;
      }

      return () => h(compile(props.template), {
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
          model: props.model,
          options: props.options
        }
      });
    }
  })
</script>