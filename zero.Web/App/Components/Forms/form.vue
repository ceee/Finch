<template>
  <form class="ui-form" @change="changed">
    <slot />
  </form>
</template>


<script>
  export default {
    name: 'uiForm',

    data: () => ({
      dirty: false,
      root: null
    }),

    created()
    {
      
    },

    mounted()
    {
      this.root = this.findRootComponent();
      this.root.beforeRouteLeave = this.beforeRouteLeave;
      console.info(this, this.root);
    },

    beforeRouteLeave(to, from, next) 
    {
      const answer = window.confirm('Do you really want to leave? you have unsaved changes!')
      if (answer) {
        next()
      } else {
        next(false)
      }
    },

    methods: {

      findRootComponent()
      {
        // find component which is attached to the router
        let current = this;
        do
        {
          if (current.page === true)
          {
            return current;
          }
        }
        while (current = current.$parent);

        console.warn('ui-form: Could not find root component with page=true (on data set)');
      },

      changed()
      {
        this.dirty = true;
      }

    }
  }
</script>