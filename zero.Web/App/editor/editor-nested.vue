<template>
  <div class="editor-nested">
    <editor-component v-for="comp in component.components" v-model="val" :component="comp" :deep="true" />
  </div>
  <!--<input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-localize:placeholder="" />-->
</template>


<script>
  import EditorComponent from 'zero/editor/editor-component';

  export default {
    props: {
      value: {
        type: [Object, Array]
      },
      config: Object,
      component: Object
    },

    components: { EditorComponent },

    data: () => ({
      val: ''
    }),

    created()
    {
      
    }
  }
</script>

<style lang="scss">
  .editor-nested
  {
    border: 1px solid var(--color-line-light);
    border-radius: var(--radius);
    background: var(--color-bg-xxlight);
    padding: var(--padding);

    .ui-property + .ui-property
    {
      margin-top: 5px;
    }
  }
</style>