<template>
  <div class="ui-tree">
    tree
  </div>
</template>


<script>
  export default {
    name: 'uiTree',

    props: {
      get: {
        type: Function,
        required: true
      }
    },

    created ()
    {
      this.load();
    },

    methods: {

      // loads children of the given parent id or on root if empty
      load(parent)
      {
        let promise = this.get(parent);

        //if (typeof promise !== 'object' || !promise['then'] || typeof promise.then !== 'function')

        promise.then(response =>
        {
          console.info(response);
        })
        .catch(error =>
        {
          // TODO handle errors
        });
      }

    }
  }
</script>